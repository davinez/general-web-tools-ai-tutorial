# Obatin throught terraform command to connect to the VM
output "public_ip_address" {
  description = "The public IP address of the VM."
  value       = data.azurerm_public_ip.pip_data.ip_address
}

# Commands currently not used but may be useful

output "admin_ssh_command" {
  description = "Command to SSH into the VM."
  value       = "ssh -i ${var.public_key_path} ${var.admin_user}@${data.azurerm_public_ip.pip_data.ip_address}"
}

output "admin_ssh_key_path" {
  description = "Path to the SSH key."
  value       = var.public_key_path
  sensitive   = true
}

# Fully Qualified Domain Name.
output "vm_hostname" {
  description = "The FQDN of the VM."
  value       = data.azurerm_public_ip.pip_data.fqdn     
}
