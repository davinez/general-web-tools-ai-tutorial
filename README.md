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

## Implementations Backend

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

- Pending to implement
  https://wolverinefx.net/tutorials/ping-pong

https://medium.com/@ms111mithun/mastering-message-queues-leveraging-rabbitmq-locally-and-azure-service-bus-in-production-for-net-277236f25609

https://github.com/dotnet/eShop/tree/main

https://github.com/NimblePros/eShopOnWeb

# Services Urls

- Azure AI Foundry portal
  https://ai.azure.com

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

# The Great Plan

Architecture, By combining a **Timer Trigger** (scheduled) with an **Event-Driven** (reactive) flow, we're building a system that handles both massive batch data and immediate user interaction perfectly.

Using **.NET 10** for the Event Grid logic it's efficient at handling those asynchronous cloud events, while **FastAPI** can stay focused on the "heavy" AI processing.

# 🚀 Project Context: CareerIntelligence AI

**Tech Stack:** \* **Frontend:** Angular 20 (Signals, Zoneless, Tailwind CSS)

- **Gateway:** .NET 10 Minimal APIs (C# 14, Entra ID, SignalR)
- **AI Engine:** FastAPI (Python 3.12+, Pydantic v2, Azure AI Foundry SDK)
- **Infrastructure:** Azure AI Foundry (OpenAI, Search, Vision), Blob Storage, Azure Functions.

## 🏗️ The Refined Architecture

### 1. The Trigger System (Data Ingestion)

- **Automated (The Scraper):** An **Azure Function (Timer Trigger)** runs on a schedule (e.g., daily at 8 AM). It calls the **FastAPI** scraper service to crawl job boards.
- **Manual (The Screenshot):** An **Angular 20** "Quick Capture" feature allows pasting/uploading screenshots. The **.NET 10 API** receives these and saves them to **Azure Blob Storage**.
- **Event-Driven (The Processor):** When a new JSON file (scraper) or Image (manual) hits Blob Storage, an **Event Grid Trigger** kicks off the specific AI pipeline.

### 2. The AI Pipeline (The "Brain")

- **Vision:** **Azure AI Document Intelligence** parses screenshots to extract raw text and layout.
- **Language:** **Azure OpenAI (GPT-4o)** processes the raw text (from scrapers or OCR) to generate structured JSON (Skills, Salary, Seniority, Tech Stack).
- **Knowledge:** **Azure AI Search** stores these jobs as **Vectors**, enabling semantic queries like: _"Find me jobs that match my specific .NET and React experience."_

---

## 📋 Step-by-Step Implementation Roadmap

### Phase 1: Environment & Gateway Foundation

- [ ] **1.1 Hub Setup:** Provision Azure AI Foundry Hub, OpenAI (GPT-4o), and AI Search.
- [ ] **1.2 .NET 10 Gateway:** Setup Minimal API with OpenAPI 3.1. Configure **Azure Blob Storage** integration.
- [ ] **1.3 Angular 20 Skeleton:** Initialize project with **Signals** for state and a simple upload component for screenshots.

### Phase 2: Manual Input & Vision (FastAPI)

- [ ] **2.1 FastAPI OCR Service:** Build the endpoint using **Azure AI Document Intelligence**.
- [ ] **2.2 Event-Driven Flow:** Connect Blob Storage events to trigger the FastAPI processing layer.

### Phase 3: Automated Scraper & Data Lake

- [ ] **3.1 Playwright Scraper:** Build the Python scraping logic in FastAPI.
- [ ] **3.2 Azure Function:** Setup the Timer Trigger to automate the scraper.
- [ ] **3.3 Storage:** Store raw data in **Azure Data Lake Gen2**.

### Phase 4: RAG & Intelligence

- [ ] **4.1 LLM Structuring:** Write the system prompts in **Prompt Flow** to clean and categorize job data.
- [ ] **4.2 Vector Indexing:** Configure **Azure AI Search** to index the structured jobs.
- [ ] **4.3 Semantic Querying:** Add an endpoint to .NET to "search jobs by resume."

### Phase 5: Agentic UI & GenAIOps

- [ ] **5.1 Career Agent:** Deploy an **Azure AI Foundry Agent** to chat with the user about market trends.
- [ ] **5.2 Real-time Dashboard:** Use **SignalR** to push scraper updates to the Angular dashboard.
- [ ] **5.3 Monitoring:** Implement **Application Insights** and Content Safety filters.

---

### 🏁 Where shall we begin?

Since the architecture depends on data being uploaded and stored correctly first, I recommend starting with **Phase 1.2 and 1.3**.

**Would you like me to provide the .NET 10 code for the Minimal API Gateway (handling the image upload) or the Angular 20 "Quick Capture" component using Signals?**
