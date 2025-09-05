# general-web-tools-ai-tutorials


# CoreApp

## Database

- Migrations

Run the following commands to create the database schema:

```bash
# Create a new migration
dotnet ef migrations add Initial-Migration --output-dir Infrastructure/Data/Migrations

# Remove the latest migration
dotnet ef migrations remove

# Update the database to the latest migration
dotnet ef database update

# Update the database to a specific migration
dotnet ef database update [MigrationName]

# List all available migrations
dotnet ef migrations list

# Generate SQL script for all migrations
dotnet ef migrations script

# Drop the database
dotnet ef database drop
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

# Drop the database	
Drop-Database

```

# Run App
 
- Locally
http://localhost:8082/swagger


# Linux

sudo apt update && sudo apt upgrade -y



## Troubleshoot

- With docker if running a UI app like Rancher Desktop:

If you get below error when running "docker" command:

```
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

- Logic in Angular and Blazor App
- Bookarks:
-- SignalR to angular client to .net api and status of processing bookmark



- Playground:

##  Implementations Backend

Implement Service Bus
Implement Azure Event Hubs / RabbitMQ
Implement Storage
Implement Redis
Implement CosmosDB
Implement api dedicated to scrap in python
Implement transactions DB
Implement GraphQL
Implements unit tests and integrations tests (Nunit or Xunit)

## Use Local Emulators

Service Bus
Event Hubs / RabbitMQ
Azurite
Redis
CosmosDB
Docker - Implemented
SQL Server - Implemented



# Reference 

## Used in this APP
 
- Angular Template
https://github.com/ng-matero/ng-matero
https://nzbin.gitbook.io/ng-matero/permissions
https://ng-matero.github.io/ng-matero/dashboard

- .NET API, Real World Example 
https://github.com/gothinkster/aspnetcore-realworld-example-app


-- Reference
https://github.com/bitwarden

https://github.com/Kareadita/Kavita/tree/develop/UI/Web/src/app


* Pending to implement
https://wolverinefx.net/tutorials/ping-pong

https://medium.com/@ms111mithun/mastering-message-queues-leveraging-rabbitmq-locally-and-azure-service-bus-in-production-for-net-277236f25609

https://github.com/dotnet/eShop/tree/main

https://github.com/NimblePros/eShopOnWeb




## Demos Databases

- sql-server-samples
https://github.com/microsoft/sql-server-samples/tree/master/samples/databases


## Real World App Frameworks General

https://codebase.show/projects/realworld?category=backend

- Net
https://github.com/gothinkster/aspnetcore-realworld-example-app

- Angular
https://github.com/stefanoslig/angular-ngrx-nx-realworld-example-app

- Blazor
https://github.com/JoeyMckenzie/BlazorConduit


## .NET

- Collection of blazor projects
https://github.com/AdrienTorris/awesome-blazor


- CRUD with .net 8
https://github.com/thbst16/dotnet-blazor-crud


- Front Angular, Back .NET
https://github.com/Kareadita/Kavita/tree/develop


- The Bank API is a design reference project suitable to bootstrap development for a compliant and modern API.

- Libraries
https://github.com/martinothamar/Mediator


- eShop Microservices
https://github.com/dotnet/eShop/tree/main

- eShop Monolithic
https://github.com/NimblePros/eShopOnWeb


# Angular

- Collection tools
https://github.com/PatrickJS/awesome-angular

- Spotify
https://github.com/trungvose/angular-spotify


