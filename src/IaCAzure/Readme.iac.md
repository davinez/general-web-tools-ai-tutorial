# Deploying Chatterbox TTS on an Azure Spot VM

* **Data:** This guide uses the VM's local disk for Docker volumes. If the VM is evicted and *deleted*, your downloaded models (`/cache`) and voices (`/voices`) will be **lost**.

- Alternatives

[Azure File Share](https://learn.microsoft.com/en-us/azure/storage/files/storage-how-to-use-files-linux).

Managed disk mount to VM


## Prerequisites

1.  **Azure Account** & **Azure CLI**

- Log in: 

1a- `az login`
1b- `az login --tenant c57ddd06-765a-4d58-8155-c6821999efd6`
2- `az account set --subscription "<YOUR_SUBSCRIPTION_ID>"`

After logging in, you can run any az commands, such as
``` bash
az account show
az group list
az vm list
```
2.  **Terraform** inside wsl

3.  **Ansible** (Only if you use Approach B), inside wsl

- You'll also need the Docker collection: 
```bash
ansible-galaxy collection install community.docker
```

4.  **SSH Key Pair**:

```bash
# -t ed25519 Specifies the algorithm.
# -C "your_email...": A comment to help you identify the key
ssh-keygen -t ed25519 -C "your_email@example.com"
```


## Step 1: Provision Infrastructure with Terraform

1- `cp terraform.tfvars.example terraform.tfvars` and fill variables
2a- * **If using Approach A (cloud-init):** 

Edit `cloud-init.yaml` if needed
    
2b- **If using Approach B (Ansible):** 

Open `main.tf` and **comment out the `custom_data` line** in the `azurerm_linux_virtual_machine` resource. 

3-  Initialize and apply Terraform:
```bash
    terraform init
    terraform plan -out=spot.plan
    terraform apply "spot.plan"
```
4-  Terraform will output the `public_ip_address` of your new VM.


## Step 2: Deploy Your Application (Choose One)

### Approach A: `cloud-init` (Automated Bash Script)

If you correctly set your Git repo URL in `cloud-init.yaml` and *did not* comment out the `custom_data` line in Terraform, **you are already done.**

The VM will boot, install dependencies, clone your repo, and run `docker compose up` automatically.

* **To check progress:**
```bash
# Get the IP from terraform output
VM_IP=$(terraform output -raw public_ip_address)
    
# SSH in and tail the logs
ssh -i ~/.ssh/id_rsa azureuser@$VM_IP "tail -f /var/log/cloud-init-output.log"
```
* **To see your containers:**
```bash
ssh -i ~/.ssh/id_rsa azureuser@$VM_IP "docker ps"
```

### Approach B: Ansible (Manual Push)

If you *commented out* the `custom_data` line, the VM is waiting for you.

1.  Navigate to the `configuration/ansible/` directory.
2.  Copy the example inventory:
    * `cp inventory.ini.example inventory.ini`
3.  **Edit `inventory.ini`**:
    * Add the `public_ip_address` from your Terraform output.
4.  **Edit `playbook.yml`**:
    * **Change the `git clone` URL** to your repository.
5.  Run the Ansible playbook:
```bash
# This will SSH, install docker-compose, clone, and run your app
ansible-playbook -i inventory.ini playbook.yml
```

## Step 3: Access Your Application

Once `docker compose` is finished (either via `cloud-init` or Ansible), your services will be running.

* **React Frontend:** `http://<VM_PUBLIC_IP>:4321`
* **API Health Check:** `http://<VM_PUBLIC_IP>:4123/health`

*(Note: The `frontend` service in your compose file maps host port `4321` to its internal port `80`. The `chatterbox-tts` API maps host `4123` to internal `4123`.)*


### How to use a Custom Image?

The Marketplace image is the easiest way. But if you wanted to build your *own* (e.g., using [Azure Image Builder](https://learn.microsoft.com/en-us/azure/virtual-machines/image-builder-overview) or [Packer](https://www.packer.io/)) to pre-bake your application code:

1.  You would build your image and it would have a Resource ID like `/subscriptions/.../resourceGroups/.../providers/Microsoft.Compute/images/myCustomGpuImage`.
2.  In `terraform/main.tf`, you would **remove** the `plan` block and **change** the `source_image_reference` block to:
```terraform
    source_image_id = "/subscriptions/..." # ID of your custom image
```

### How to manage secrets (like Azure keys)?

* **For your Application (in Docker Compose):**
1.  **Good:** Use a `.env` file. Your `docker-compose.yml` already reads from one (e.g., `${PORT:-4123}`). You could have Ansible or `cloud-init` create this `.env` file on the VM.
2.  **Better:** Use **Azure Key Vault** with **Managed Identity**.
  * In `terraform/main.tf`, I've already added an `identity` bloto the VM. This gives the VM its own "name" in Azure AD.
  * You create an Azure Key Vault and give this VM's identi**permission** to read secrets.
  * Your Python API (in `main.py`) would use the Azure IdentiSDK. It would look like this (no keys needed!
  ```python
  # Example in your Python API
  from azure.keyvault.secrets import SecretClient
  from azure.identity import DefaultAzureCredenti
  # This automatically uses the VM's Managed Identity
  credential = DefaultAzureCredential()
  client = SecretClient(vault_url="[https://YOUR-VAULT-NAME.vauazure.net/](https://YOUR-VAULT-NAME.vault.azure.net/)credential=credential)
  
  # Now you can get secrets
  api_key = client.get_secret("my-api-key").value
  ```

### How do I delete and remove everything?

1.  Go to the `terraform/` directory.
2.  Run the destroy command:
```bash
terraform destroy
```

Terraform will delete the **entire resource group**, including the VM, the public IP, the network, and all associated disks. Everything will be gone, and you will stop being charged.