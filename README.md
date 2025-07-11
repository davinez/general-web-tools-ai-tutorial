# general-web-tools-ai-tutorials


# CoreApp

## Database

- Migrations

Run the following commands to create the database schema:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

```ps1
# Create a new migration	
Add-Migration [MigrationName]

# Remove the latest migration	
Remove-Migration

# Update the database to the latest migration	
Update-Database

# Update the database to a specific migration	
Update-Database -Migration [MigrationName]

# List all available migrations	
Get-Migrations

# Generate SQL script for all migrations	
Script-Migration

#Drop the database	
Drop-Database

```


## Troubleshoot

- With docker if running a UI app like Rancher Desktop:

If you get below error when running “docker” command:

````
error during connect: in the default daemon configuration on Windows, the docker client must be run with elevated privileges to connect: Get "http://%2F%2F.%2Fpipe%2Fdocker_engine/v1.49/containers/json": open //./pipe/docker_engine: The system cannot find the file specified.
Please run below script in “Terminal (Admin)” to grant yourself access to “docker_engine” socket:
```
```ps
$account=whoami
$npipe = "\\.\pipe\docker_engine"
$dInfo = New-Object "System.IO.DirectoryInfo" -ArgumentList $npipe
$dSec = $dInfo.GetAccessControl()
$fullControl =[System.Security.AccessControl.FileSystemRights]::FullControl
$allow =[System.Security.AccessControl.AccessControlType]::Allow
$rule = New-Object "System.Security.AccessControl.FileSystemAccessRule" -ArgumentList $account,$fullControl,$allow
$dSec.AddAccessRule($rule)
$dInfo.SetAccessControl($dSec)
```


## AI

- For open web ui and other AI services / tools, run:
```bash
docker compose -f docker-compose.ai.yml up
```



# Pending / Plan

- Adapt Existing functionality of medium example repo in the backend and frontend


- Bookmarks

-- Save folder and bookmarks in each folder 
--- backend
--- frontend


- Implementations Backend

Implement Service Bus
Implement Azure Event Hubs / RabbitMQ
Implement Storage
Implement Redis
Implement CosmosDB
Implement api dedicated to scrap in python
Implement transactions DB
Implement GraphQL
Implements unit tests and integrations tests (Nunit or Xunit)

- Implementations Frontend

Implement Logic in blazor app and in angular app
Implement front end tests (blazor and angular)


- Use Local Emulators

Service Bus
Event Hubs / RabbitMQ
Azurite
Redis
CosmosDB




