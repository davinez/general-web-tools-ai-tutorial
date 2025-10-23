terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "chatterbox-spot-rg"
  location = var.location
}

resource "azurerm_public_ip" "pip" {
  name                = "chatterbox-pip"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  allocation_method   = "Dynamic"
  sku                 = "Standard"
}

resource "azurerm_virtual_network" "vnet" {
  name                = "chatterbox-vnet"
  address_space       = ["10.0.0.0/16"]
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_subnet" "subnet" {
  name                 = "default"
  resource_group_name  = azurerm_resource_group.rg.name
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.1.0/24"]
}

resource "azurerm_network_security_group" "nsg" {
  name                = "chatterbox-nsg"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  security_rule {
    name                       = "AllowSSH"
    priority                   = 1001
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "22"
    source_address_prefix      = var.my_ip # IMPORTANT: Locks SSH to your IP
    destination_address_prefix = "*"
  }

  security_rule {
    name                       = "AllowFrontend"
    priority                   = 1002
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "4321" # React App Port
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }

  security_rule {
    name                       = "AllowApi"
    priority                   = 1003
    direction                  = "Inbound"
    access                     = "Allow"
    protocol                   = "Tcp"
    source_port_range          = "*"
    destination_port_range     = "4123" # Chatterbox API Port
    source_address_prefix      = "*"
    destination_address_prefix = "*"
  }
}

resource "azurerm_network_interface" "nic" {
  name                = "chatterbox-nic"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = azurerm_subnet.subnet.id
    private_ip_address_allocation = "Dynamic"
    public_ip_address_id          = azurerm_public_ip.pip.id
  }
}

resource "azurerm_network_interface_security_group_association" "nsg_assoc" {
  network_interface_id      = azurerm_network_interface.nic.id
  network_security_group_id = azurerm_network_security_group.nsg.id
}

# https://learn.microsoft.com/en-us/azure/virtual-machines/sizes/gpu-accelerated/ncast4v3-series?tabs=sizebasic
resource "azurerm_linux_virtual_machine" "vm" {
  name                = "chatterbox-vm"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  size                = "Standard_NC4as_T4_v3"
  admin_username      = var.admin_user
  network_interface_ids = [
    azurerm_network_interface.nic.id,
  ]

  # --- Spot VM Configuration ---
  priority          = "Spot"
  eviction_policy   = "Delete" # Deallocates on eviction. Use "Delete" if you don't need the disk.
  max_bid_price     = 0.22  # Use -1 for Azure Spot price (pay-as-you-go cap)

 # az vm image list --all --publisher Canonical --sku="22_04-lts-gen2""
  source_image_reference {
  # Defines the specific VM image to use from the gallery.
  publisher = "Canonical"               
  offer     = "0001-com-ubuntu-server-jammy" 
  sku       = "22_04-lts-gen2"          
  version   = "latest"                 
 }

  # --- Managed Identity (for Azure Key Vault) ---
  identity {
    type = "SystemAssigned"
  }

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  admin_ssh_key {
    username   = var.admin_user
    public_key = file(var.public_key_path)
  }

  # --- Approach A: cloud-init (Bash) ---
  # This runs the 'cloud-init.yaml' script on first boot.
  # Comment this line out if you want to use Approach B (Ansible).
  custom_data = filebase64("${path.module}/cloud-init.yaml")

  # --- Boot Diagnostics ---
  boot_diagnostics {
    storage_account_uri = null # Set to a storage account if you need boot logs
  }
}

# This will read the public IP's data after it has been created and assigned by Azure.
data "azurerm_public_ip" "pip_data" {
  name                = azurerm_public_ip.pip.name
  resource_group_name = azurerm_resource_group.rg.name

  # This tells Terraform to wait until the VM is finished
  # before trying to read the dynamic IP.
  depends_on = [
    azurerm_linux_virtual_machine.vm
  ]
}