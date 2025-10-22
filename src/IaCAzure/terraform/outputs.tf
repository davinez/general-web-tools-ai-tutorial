output "public_ip_address" {
  description = "The public IP address of the VM."
  value       = azurerm_public_ip.pip.ip_address
}

output "vm_hostname" {
  description = "The FQDN of the VM."
  value       = azurerm_public_ip.pip.fqdn
}

output "admin_ssh_command" {
  description = "Command to SSH into the VM."
  value       = "ssh -i ${var.public_key_path} ${var.admin_user}@${azurerm_public_ip.pip.ip_address}"
}

output "admin_ssh_key_path" {
  description = "Path to the SSH key."
  value       = var.public_key_path
  sensitive   = true
}