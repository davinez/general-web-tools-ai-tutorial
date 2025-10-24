variable "location" {
  description = "Azure region to deploy resources."
  type        = string
  default     = "westus2" 
}

# https://learn.microsoft.com/en-us/azure/reliability/regions-list
# az vm list-skus --size 'Standard_NC4as_T4_v3' --resource-type virtualMachines --all --zone --output table
# az vm list-skus --location westus2 --size Standard_N --all --output table
variable "locationvm" {
  description = "Azure region to deploy vm"
  type        = string
  default     = "westus2" # Check for NCasT4_v3 availability in your region
}

variable "admin_user" {
  description = "Administrator username for the VM."
  type        = string
  default     = "azureuser"
}

variable "public_key_path" {
  description = "Path to your SSH public key file."
  type        = string
}

variable "my_ip" {
  description = "Your local public IP for SSH access. Get it from 'curl ifconfig.me'"
  type        = string
  sensitive   = true
}