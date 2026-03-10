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

######################
The Great Plan
######################

It establishes the "CareerIntelligence AI" context, focusing on a real-world **GenAIOps** approach using a high-performance, cost-effective architecture.

# 🚀 Project Context: CareerIntelligence AI (v3.0)

**Project Vision:** A "Real-World" Market Intelligence platform that transforms messy, multi-source job data (Bulk Screenshots + External APIs) into actionable insights using a **Medallion Data Lakehouse** and **Agentic AI**.

## 🏗️ The 5-Layer AI Architecture

### 1. Ingestion & Safety Layer (The Gatekeeper)

- **Purpose:** Securely land raw data and prevent redundant processing.
- **Tools/Services:** \* **Angular 20:** Frontend using **Signals** (Zoneless) for bulk file uploads.
- **Azure Blob Storage (Bronze):** Stores original images and raw API JSON.
- **Azure AI Content Safety:** Scans uploads for PII and jailbreak attempts.
- **.NET 10 Minimal APIs:** Implements an `IJobProvider` registry with **Daily-Lock caching** (SQL/Redis) to avoid redundant API calls within 24 hours.

### 2. Processing & Normalization Layer (The Factory)

- **Purpose:** Extract, clean, and deduplicate data using a hybrid LLM/SLM approach.
- **Tools/Services:** \* **FastAPI (Python 3.12):** The high-performance "Brain" orchestrating the data flow.
- **Azure AI Document Intelligence:** High-fidelity OCR and layout extraction.
- **Phi-3.5 (SLM):** Used for low-latency, low-cost "Normalization" (e.g., CDMX → Mexico City).
- **GPT-4o (LLM):** Used for complex "Reasoning" tasks like skill extraction and salary estimation.
- **Pydantic v2:** Strict data validation for the **Silver Layer** (Clean JSON) in Blob Storage.

### 3. Knowledge & Intelligence Layer (The Library)

- **Purpose:** Enable high-speed semantic search and trend analysis.
- **Tools/Services:** \* **Azure AI Search (Gold Layer):** High-performance vector store with **Integrated Vectorization**.
- **Azure OpenAI Embeddings (ada-002):** Converts job descriptions into mathematical vectors.
- **Prompt Flow:** Used for **GenAIOps**—evaluating prompt performance, groundedness, and relevance.

### 4. Agentic Orchestration Layer (The Analyst)

- **Purpose:** A "Reasoning" agent that answers user queries by using tools.
- **Tools/Services:** \* **Azure AI Foundry Agent Service:** Manages persistent state, threads, and multi-tool orchestration.
- **Code Interpreter:** Allows the Agent to execute Python for real-time math or visualization.
- **Model Context Protocol (MCP):** Connects the Agent to external data sources and internal .NET tools.

### 5. UI & Observability Layer (The Dashboard)

- **Purpose:** Visualize market trends and interact with the AI Analyst.
- **Tools/Services:** \* **Angular 20 Signals:** Reactive dashboard for real-time market heatmaps.
- **SignalR:** Pushes "Pipeline Finished" notifications from the cloud to the UI.
- **Application Insights:** End-to-end monitoring of token costs, latency, and Agent "tool-call" logic.

## 📋 AI Engineering Master Roadmap

- **Phase 1: Foundation (GenAIOps):** Provision **Azure AI Foundry Hub**; set up .NET `IJobProvider` and Daily Cache logic.
- **Phase 2: Extraction Pipeline:** Build **FastAPI** service for Bulk OCR + Phi-3.5 Normalization; establish **Medallion (Bronze/Silver)** folders.
- **Phase 3: RAG Implementation:** Configure **Azure AI Search** to crawl Silver blobs; test indexing with **Prompt Flow**.
- **Phase 4: Agentic Reasoning:** Deploy the **Foundry Agent** with a "Market Analyst" persona and Search tools.
- **Phase 5: Agentic UI:** Build the Angular Chat + Signals Dashboard; implement **Responsible AI** filters and cost tracking.

##############

### Topics

##############

# AI Services, Tools, Libraries, and Frameworks

## Azure AI Services & Tools

- Azure AI Vision
- Azure AI Language
- Azure AI Foundry
- Azure AI Foundry Agent Service
- Azure AI Foundry Models API
- Azure AI Foundry SDK
- Azure AI Foundry portal
- Azure AI Foundry extension for VS Code
- Azure AI Services SDKs
- Azure OpenAI (OpenAI models, GPT-4, GPT-4o, etc.)
- Azure AI Search
- Azure Key Vault
- Azure Storage
- Azure AI hub
- Azure Machine Learning studio
- Prompt Flow (in Azure AI Foundry and Azure ML Studio)
- Microsoft Foundry Agent Service
- Microsoft Foundry extension (VS Code)
- Microsoft Agent Framework
- Microsoft 365 Agents SDK
- Microsoft Copilot Studio (Full and Lite)
- AutoGen (open-source framework)
- OpenAI Assistants API
- Bing Search (as a grounding tool)
- File Search (RAG)
- Code Interpreter (Python execution)
- OpenAPI Tools (external API integration)
- Model Context Protocol (MCP)
- Microsoft Fabric (data store integration)
- GitHub Copilot

## Libraries, Models, and Catalogs

- Large Language Models (LLMs): GPT-4, Mistral Large, Llama3 70B, Llama 405B, Command R+
- Small Language Models (SLMs): Phi3, Mistral OSS, Llama3 8B
- Hugging Face model catalog
- GitHub Marketplace models
- DALL·E 3, Stability AI (image generation)
- Ada, Cohere (embedding models)
- Core42 JAIS (Arabic LLM)
- Nixtla TimeGEN-1 (time-series forecasting)
- DeepSeek-R1, o1 (reasoning models)
- Phi3-vision (multi-modal)
- Cohere Command R+
- Meta, Databricks, Snowflake, Nvidia models

# Methodologies, Concepts, and Abstract Topics

## AI & Agent Concepts

- Generative AI
- Language Models (LLMs, SLMs)
- Foundation Models
- Multi-modal Models
- Retrieval Augmented Generation (RAG)
- Prompt Engineering
- Fine-tuning
- Model Inference API
- Model deployment (standard, serverless, managed compute)
- Model selection and evaluation (benchmarks, manual/automated)
- Model lifecycle and GenAIOps
- System Prompts and Guidance/Templates
- Embeddings and semantic search
- Function calling and JSON support
- Chat completion models
- Reasoning models
- Multi-agent orchestration

## Agent Development & Architecture

- Single-Agent and Multi-Agent Scenarios
- Knowledge Integration (grounding)
- Task Automation
- Decision-Making
- Threads, Messages, Runs (conversation/session management)
- Tools: Knowledge Tools (grounding), Action Tools (execution)
- Custom Tools (API integration, Azure Functions, OpenAPI)
- Visual Agent Designer (VS Code)
- Agent Instructions (persona, goals, constraints)
- Agent Testing (Playground)
- Deployment Pipeline (VS Code to Foundry)
- Model Router (automatic model selection)
- Foundry IQ (shared RAG knowledge base)
- RBAC (Role-Based Access Control)
- Human-in-the-Loop
- Sandboxing
- Prompt Filtering
- Auditability and Observability

## Responsible AI Principles

- Fairness
- Reliability and Safety
- Privacy and Security
- Inclusiveness
- Transparency
- Accountability

## Development & Operations

- Project/resource provisioning (Portal, BICEP/ARM, CLI)
- Centralized access control and cost management
- Regional availability considerations
- Single vs. Multi-service resources
- Connections and Runtimes (Prompt Flow)
- Variants (Prompt Flow)
- Metrics: Groundedness, Relevance, Coherence, Fluency, Similarity
- End-user feedback and monitoring
- Experimentation, Evaluation, Production lifecycle
