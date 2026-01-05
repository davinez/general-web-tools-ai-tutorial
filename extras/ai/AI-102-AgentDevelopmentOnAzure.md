
# Get started with AI agent development on Azure 

As generative AI models evolve, they are moving beyond simple chat applications to power **intelligent agents** that operate autonomously to automate tasks, orchestrate business processes, and coordinate workloads.

## Single-Agent Scenario

A single agent can be designed to assist with specific corporate tasks, such as managing expense claims.

* **Knowledge Integration:** Uses generative models combined with policy documentation to answer employee queries regarding claim limits and rules.
* **Automation:** Uses programmatic functions to automatically submit recurring expenses (e.g., monthly bills) or route claims to the correct approver based on the amount.

## Multi-Agent Scenario

In complex environments, multiple agents can coordinate to manage integrated workflows across different business processes.

* **Example:** A **travel booking agent** can book flights and hotels, then automatically provide receipts and data to the **expenses agent** to complete the workflow.


# Summary: Understanding AI Agents

## 1. Definition & Core Capabilities
AI agents are smart applications that use language models to understand intent and **take autonomous action**. Unlike basic chatbots, they maintain conversation memory and execute tasks.

### Three Essential Pillars:
* **Knowledge & Reasoning:** Using generative models and grounding data (e.g., corporate policies) to answer accurately.
* **Task Automation:** Executing programmatic functions (e.g., submitting a claim or booking a flight).
* **Decision-Making:** Using business rules to route tasks or select the next logical step.

---

## 2. Agent Scenarios
* **Single-Agent (Expense Agent):** Accepts a prompt $\rightarrow$ Grounds it with policy data $\rightarrow$ Generates a response $\rightarrow$ Submits a claim/payment.
* **Multi-Agent (Travel + Expense):** A Travel Agent books services via APIs, then automatically triggers the Expense Agent to file receipts—completing an end-to-end workflow without manual intervention.

---

## 3. Security Risks & Mitigation

| Risk Area | Description |
| :--- | :--- |
| **Data Leakage** | Agent exposes sensitive/confidential data it shouldn't have shared. |
| **Prompt Injection** | Malicious inputs trick the agent into ignoring instructions or leaking passwords. |
| **Privilege Escalation** | Agent performs unauthorized actions (e.g., deleting records) due to weak controls. |
| **Data Poisoning** | Corrupted training/context data leads to unsafe or fraudulent outputs. |
| **Over-reliance** | Actions taken automatically without necessary human validation. |

### Security Best Practices:
* **Least Privilege (RBAC):** Limit agent access to only the data/tools strictly required.
* **Input Validation:** Use filters to block injection attacks.
* **Human-in-the-Loop:** Require human approval for high-stakes or sensitive actions.
* **Auditability:** Maintain comprehensive logs to trace "who, what, when, and why."
* **Supply Chain Safety:** Regularly audit third-party plugins and APIs.



# Summary: AI Agent Development Options

## 1. Evolution of AI: Traditional vs. Agentic
| Feature | Traditional AI Frameworks | AI Agent Frameworks |
| :--- | :--- | :--- |
| **Focus** | Enhancing apps with intelligence. | Autonomous, goal-oriented systems. |
| **Capabilities** | Personalization, automation, NLP. | Reasoning, acting, learning, and collaboration. |
| **Interaction** | Reactive (responds to input). | Proactive (works to achieve goals). |

## 2. Key Frameworks & Tools

### Pro-Code / Developer Solutions
* **Microsoft Foundry Agent Service:** Managed Azure service; offers multi-model choice, enterprise security, and Azure/OpenAI SDK support.
* **Microsoft Agent Framework:** Lightweight SDK for building and orchestrating multi-agent systems.
* **Microsoft 365 Agents SDK:** For building self-hosted agents delivered via M365, Slack, or Messenger.
* **AutoGen:** Open-source framework optimized for rapid experimentation and research.
* **OpenAI Assistants API:** Specialized for OpenAI models; integrated into Foundry Agent Service for more flexibility.

### Low-Code / No-Code Solutions
* **Microsoft Copilot Studio:** Visual "citizen developer" environment for deploying agents across enterprise channels.
* **Copilot Studio Lite (M365):** Declarative tool for business users to describe and create basic agents for daily tasks.

## 3. Decision Matrix: Choosing the Right Tool

| User Type | Recommended Solution | Primary Use Case |
| :--- | :--- | :--- |
| **Business User (No Code)** | Copilot Studio Lite | Automating personal/everyday tasks. |
| **Power User (Low Code)** | Copilot Studio (Full) | Extending Copilot and Teams workflows. |
| **Pro Dev (M365 Focus)** | M365 Agents SDK | Custom extensions for Microsoft ecosystem. |
| **Pro Dev (Azure Focus)** | Foundry Agent Service | Scalable, backend-heavy Azure solutions. |
| **Pro Dev (Multi-Agent)** | Microsoft Agent Framework | Complex orchestration across environments. |

> **Note:** Choice is often influenced by existing infrastructure, language preference, and the need for multi-agent coordination versus simple task automation.

**Tip**

1- For more information about Foundry Agent Service, see the Microsoft Foundry Agent Service documentation. (https://learn.microsoft.com/en-us/azure/ai-services/agents/)
2- For more information about using the OpenAI Assistants API in Azure, see Getting started with Azure OpenAI Assistants. (https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/assistant)
3- For more information about AutoGen, see the (https://microsoft.github.io/autogen/stable/index.html)
4- For more information about Microsoft 365 Agents SDK, see the Microsoft 365 Agents SDK documentation. (https://learn.microsoft.com/en-us/microsoft-365/agents-sdk/)
5- For more information about Microsoft Copilot Studio, see the Microsoft Copilot Studio documentation. (https://learn.microsoft.com/en-us/microsoft-copilot-studio/)
6- For more information about authoring agents with Copilot Studio lite experience, see the Build agents with Copilot Studio lite experience.
(https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/copilot-studio-agent-builder-build)


# Summary: Microsoft Foundry Agent Service

## 1. Overview
A managed Azure service for creating, testing, and scaling AI agents. It bridges the gap between simple chat and complex enterprise automation by providing a secure, governed runtime.

* **Development Paths:** 
    * **Visual:** Low-code experience via the Microsoft Foundry portal.
    * **Code-First:** Professional development using the Foundry SDK (Python, C#, etc.).

## 2. Core Components of an Agent

* **Model:** The "brain" that powers reasoning. Supports OpenAI models (GPT-4o) and a broad catalog of open-source models (Llama, etc.).
* **Knowledge (Grounding):** Connects the agent to data to ensure factual accuracy. 
    * *Sources:* Bing Search, Azure AI Search, or custom enterprise documents.
* **Tools (Actions):** Programmatic functions that allow the agent to *do* things.
    * *Built-in:* Code Interpreter (runs Python), Bing Search.
    * *Custom:* Azure Functions or custom code for API integrations.
* **Threads:** The conversation "container" that maintains message history and stores generated assets (like files).

## 3. Key Operational Primitives
* **Instructions:** System prompts that define the agent's persona, goals, and constraints.
* **Runs:** The execution of an agent on a specific thread. During a run, the agent identifies intent, selects tools, and generates responses.
* **Foundry IQ:** A specialized retrieval system (RAG) that allows multiple agents to share a single, scaleable knowledge base.

## 4. Why Use Foundry Agent Service?
* **Security:** Native integration with Microsoft Entra ID (RBAC) and VNet isolation.
* **Observability:** Built-in tracing and logging to monitor agent decisions and performance.
* **Model Router:** Automatically routes tasks to the most efficient/cost-effective model.

**Tip**

1- For more information about Foundry Agent Service, see Microsoft Foundry Agent Service documentation.
https://learn.microsoft.com/en-us/azure/ai-services/agents/




# Exercise: Explore AI Agent development

https://learn.microsoft.com/en-us/training/modules/ai-agent-fundamentals/5-exercise


# Summary: What is an AI Agent

## 1. Definition
A **fully managed service** for building, deploying, and scaling secure, extensible AI agents. 

## 2. Key Technical Advantages
* **Serverless Architecture:** Developers do not need to manage underlying compute or storage.
* **Low-Code Efficiency:** Reduces the total volume of code required to deploy agents.
* **Customization:** Supports custom instructions and integration with advanced tools/APIs.
* **Security & Compliance:** Designed for high-stakes industries (like Healthcare) requiring robust data protection.

## 3. Practical Application (Healthcare Example)
* **Use Cases:** Automating patient inquiries, scheduling, and real-time medical data retrieval.
* **Primary Benefit:** Offloads infrastructure and security overhead so teams can focus on the agent's logic and quality rather than the backend.


## 1. What is an AI Agent?
An **AI Agent** is a software service that uses generative AI to understand context and perform tasks **autonomously** on behalf of a user.
* **Core Difference:** Unlike standard chat models, agents don't just "talk"—they **take action** (execute workflows, use tools, and access grounding data).
* **Key Traits:** Goal-oriented, context-aware, and capable of operating without constant human intervention.

## 2. Why Use AI Agents?
* **Automation:** Handles repetitive tasks to free up human creativity.
* **Decision-Making:** Processes massive data sets for real-time, autonomous insights.
* **Scalability:** Grows business operations without needing more staff.
* **24/7 Availability:** Continuous operation for customer service or monitoring.

## 3. Common Use Cases
| Type | Example Task |
| :--- | :--- |
| **Personal Productivity** | Scheduling, drafting emails (e.g., M365 Copilot). |
| **Research** | Monitoring market trends and stock performance. |
| **Sales** | Automating lead qualification and follow-ups. |
| **Customer Service** | Handling refunds and routine inquiries (e.g., Cineplex). |
| **Developer** | Bug fixing and code review (e.g., GitHub Copilot). |

## 4. Critical Security Risks & Mitigations
| Risk | Description |
| :--- | :--- |
| **Prompt Injection** | Malicious inputs trick the agent into unauthorized actions. |
| **Data Leakage** | Unintentional exposure of sensitive/private data. |
| **Privilege Escalation** | Agent accesses systems/data beyond its intended scope. |
| **Over-Reliance** | Executing unintended actions without human oversight. |

### Mitigation Strategies (Security-by-Design):
* **RBAC:** Enforce Role-Based Access Control and Least Privilege.
* **Human-in-the-Loop:** Gating sensitive actions (e.g., payments) for human approval.
* **Sandboxing:** Isolating operations to prevent system-wide breaches.
* **Prompt Filtering:** Validation layers to block injection attacks.


**Tip**

1- To learn more about GitHub Copilot, explore the GitHub Copilot fundamentals learning path.
https://learn.microsoft.com/en-us/training/paths/copilot/

2- You can explore more about agents in general with the Fundamentals of AI agents module.
https://learn.microsoft.com/en-us/training/modules/ai-agent-fundamentals


# Summary: How to use Microsoft Foundry Agent Service


![alt text](images-cert/image8.png)

## Overview

Microsoft Foundry Agent Service is a **fully managed service** that allows developers to build, deploy, and scale extensible AI agents. It eliminates the need for managing underlying compute and storage resources, significantly reducing coding effort (often requiring fewer than 50 lines of code).

## Purpose and Use Cases
The service is designed for scenarios requiring advanced language models for workflow automation. It enables agents to:
* **Answer questions** using real-time or proprietary data.
* **Make decisions** and perform actions based on user input.
* **Automate workflows** by combining generative AI with tools that interact with real-world data.
* **Common Applications:** Customer support, data analysis, automated reporting, and report generation.

## Key Features
* **Automatic Tool Calling:** Manages the full lifecycle of running models, invoking tools, and returning results.
* **Managed Conversation State:** Uses "threads" to securely manage conversation states automatically.
* **Out-of-the-box Tools:** Includes support for file retrieval, code interpretation, Bing search, Azure AI Search, and Azure Functions.
* **Flexibility:** Allows for specific model selection (Azure OpenAI) and customizable storage (platform-managed or bring-your-own Azure Blob storage).
* **Security:** Provides enterprise-grade security, including keyless authentication and data privacy compliance.

## Architecture and Resources
The service provisions necessary cloud resources through Azure and AI Foundry. At a minimum, an **Azure AI hub** and an **Azure AI project** are required.

### Deployment Configurations:
1.  **Basic Agent Setup:** Includes Azure AI hub, Azure AI project, and Foundry Tools.
2.  **Standard Agent Setup:** Includes the basic setup plus Azure Key Vault, Azure AI Search, and Azure Storage.

Resources can be deployed via the Microsoft Foundry portal or through predefined **bicep templates**.


# Summary: Develop agents with the Microsoft Foundry Agent Service

![alt text](images-cert/image9.png)

## Overview
Foundry Agent Service simplifies agent creation by supporting **client-side function calling** and connections to **Azure Functions** or **OpenAPI** tools with minimal code. 

> **Note on Selection:** > * Use **Copilot Studio** for Microsoft 365 integrations.
> * Use **Semantic Kernel Agents Framework** for multi-agent orchestration.

## The Development Lifecycle (Implementation Steps)
To integrate an agent into an app via SDK (Python, etc.) or REST API, follow these high-level steps:

1.  **Connect:** Link to the AI Foundry project using the project endpoint and Entra ID authentication.
2.  **Reference/Create Agent:** Define the agent by specifying:
    * **Model Deployment:** The specific model (e.g., GPT-4o) used for reasoning.
    * **Instructions:** Defining behavior and functionality.
    * **Tools:** Resources the agent can invoke.
3.  **Create a Thread:** Establish a stateful chat session that retains history and data.
4.  **Message & Invoke:** Add user messages to the thread and trigger the agent.
5.  **Status Check:** Monitor the thread status; once ready, retrieve messages and artifacts.
6.  **Chat Loop:** Repeat the message and retrieval steps until the session ends.
7.  **Cleanup:** Delete the agent and thread to manage resources and data privacy.


## Agent Tools
Tools allow agents to perform tasks beyond simple text generation. They are categorized into two types:

### 1. Knowledge Tools (Grounding)
Used to enhance the agent's context with real-world or proprietary data:
* **Bing Search:** Real-time web data.
* **File Search:** Data from files in a vector store.
* **Azure AI Search:** Results from custom search indexes.
* **Microsoft Fabric:** Data from Fabric data stores.

### 2. Action Tools (Execution)
Used to perform tasks or run computations:
* **Code Interpreter:** A secure sandbox for running model-generated Python (e.g., for math or data visualization).
* **Custom Functions:** Implementation of your own local code.
* **Azure Functions:** Serverless cloud code execution.
* **OpenAPI Spec:** Calling external APIs via standard specifications.


**Tip**

1- This Fundamentals of AI Agents unit explores more of the options for building agents.
https://learn.microsoft.com/en-us/training/modules/ai-agent-fundamentals/3-agent-development


# Exercise - Build an AI agent

https://learn.microsoft.com/en-us/training/modules/develop-ai-agent-azure/5-exercise



# Unit: Develop AI agents with the Microsoft Foundry extension in Visual Studio Code 

# Summary: Get started with the Microsoft Foundry extension

![alt text](images-cert/image10.png)

## Overview

The **Microsoft Foundry extension** integrates enterprise-grade AI agent development directly into Visual Studio Code. It allows developers to build, test, and deploy agents without leaving their primary code editor.

## Core Capabilities
* **Agent Discovery and Management:** Browse, create, and manage agents within existing projects.
* **Visual Agent Designer:** An intuitive graphical interface to configure instructions, models, and capabilities.
* **Integrated Testing (Playground):** Real-time interaction with agents to refine behavior before deployment.
* **Code Generation:** Automatically generates sample integration code to connect agents to applications.
* **Deployment Pipeline:** Direct deployment from VS Code to Microsoft Foundry for production.

## Key Features
* **Tool Integration:** Seamless support for **RAG** (Retrieval-Augmented Generation), Search capabilities, custom actions, and **Model Context Protocol (MCP)** servers.
* **Project Integration:** Direct connection to Microsoft Foundry projects and infrastructure resources.

## Installation and Setup
1.  **Open Extensions:** Press `Ctrl+Shift+X` in VS Code.
2.  **Search:** Look for "Microsoft Foundry."
3.  **Install:** Select the extension and verify successful installation via status messages.

## Standard Development Workflow
The extension enables rapid prototyping through a streamlined seven-step process:

1.  **Install & Configure** the extension.
2.  **Connect** to a Microsoft Foundry project.
3.  **Create/Import** an agent using the Visual Designer.
4.  **Configure** instructions and add tools (Knowledge or Action).
5.  **Test** using the built-in playground.
6.  **Iterate** based on performance and test results.
7.  **Generate Code** to integrate the agent into the final application.


# Summary: Develop AI agents in Visual Studio Code


![alt text](images-cert/image11.png)

## 1. Overview
The **Microsoft Foundry extension** for Visual Studio Code allows for designing, configuring, and testing AI agents within a unified development environment. It leverages the **Microsoft Foundry Agent Service**, a managed Azure service built on the OpenAI Assistants API.

### Key Service Features:
* **Model Choice:** Support for multiple AI models beyond OpenAI.
* **Enterprise Security:** Built-in features for production-grade security.
* **Data Integration:** Connections to Azure data services.
* **Tooling:** Access to built-in and custom tools.

## 2. Agent Creation & Prerequisites
### Prerequisites:
1.  Complete extension setup and sign in to Azure.
2.  Create or select a **Microsoft Foundry project**.
3.  Deploy an AI model to be used by the agent.

### Creation Steps:
1.  Open the **Microsoft Foundry Extension** view in VS Code.
2.  Navigate to **Resources** and click the **+ (plus)** icon next to **Agents**.
3.  The extension opens two views simultaneously:
    * **Agent Designer:** A visual interface for configuration.
    * **YAML File:** The direct configuration file (`.yaml`) containing metadata, model options, and instructions.

## 3. Configuration & Instruction Design
### Basic Properties:
* **Name & Description:** Identity and purpose of the agent.
* **Model Selection:** The specific deployment the agent will use.
* **System Instructions:** Definitions of behavior, personality, and response style.
* **Agent ID:** Automatically generated upon creation.

### Best Practices for Instructions:
* **Specificity:** Define clear actions and behaviors.
* **Context & Boundaries:** Explain the environment and what the agent *cannot* do.
* **Examples & Personality:** Provide sample interactions and establish a specific tone.

## 4. Deployment & Management
### Deployment Process:
1.  Click **"Create on Microsoft Foundry"** in the Designer view.
2.  Once complete, refresh the **Azure Resources** view to verify the agent appears in the list.

### Management Options:
* **Edit/Redeploy:** Modify configurations and use **"Update on Microsoft Foundry"**.
* **Integration:** Use **"Open Code File"** to generate sample code for application integration.
* **Playground:** Use **"Open Playground"** for real-time testing and validation.

## 5. Testing and Conversation Concepts
The extension uses a specific structure to manage agent interactions:
* **Threads:** Conversation sessions that store messages and context.
* **Messages:** Individual units of interaction (text, images, files).
* **Runs:** Single executions where the agent processes the thread based on its configuration.



# Summary: Extend AI agent capabilities with tools

![alt text](images-cert/image12.png)


## 1. Overview of Agent Tools
Tools are programmatic functions that allow agents to automate actions and access data beyond their initial training. When an agent identifies a need, it can:
* **Invoke** the specific tool automatically.
* **Process** the data/results.
* **Incorporate** the findings into a final user response.

## 2. Built-in Tools in Microsoft Foundry
These tools are production-ready and require no additional setup:

| Tool | Capability | Use Case |
| :--- | :--- | :--- |
| **Code Interpreter** | Writes/executes Python code. | Math, data analysis, charts, file processing. |
| **File Search** | Retrieval-augmented generation (RAG). | Indexing PDFs/Word docs; searching knowledge bases. |
| **Grounding with Bing** | Real-time internet search. | Current events, trending topics, and citations. |
| **OpenAPI Tools** | Connects to external APIs. | Integrating with 3rd-party services via OpenAPI 3.0. |
| **MCP** | Standardized tool interfaces. | Using community-driven tools. |

## 3. Adding Tools via Visual Studio Code
The extension simplifies the integration process through a visual interface:
1.  **Select Agent:** Choose the target agent in the extension view.
2.  **Navigate to Tools:** Open the Tools section in the configuration panel.
3.  **Browse Library:** Select from available built-in or custom tools.
4.  **Configure Assets:** If adding "File Search," you can create or select a **vector store asset** to host uploaded files.
5.  **Test:** Use the **Playground** to verify the tool triggers correctly.

## 4. Model Context Protocol (MCP)
MCP is an open standard designed to simplify how agents connect to external systems.

### Key Benefits:
* **Standardization:** A consistent protocol for all tool communications.
* **Reusability:** Components work across different agents/implementations.
* **Community Support:** Access to tools via MCP registries.
* **Simplified Integration:** Consistent interfaces reduce custom coding.


## 5. Management & Best Practices
To ensure reliable performance in production:
* **Identify Requirements:** Only add tools that match the agent's specific role.
* **Start Simple:** Prioritize built-in tools before developing custom ones.
* **Rigorous Testing:** Validate tool behavior in the playground under various scenarios.
* **Monitor Usage:** Track tool effectiveness and performance to optimize response times.


# Exercise - Build an AI agent using the Microsoft Foundry extension

https://learn.microsoft.com/en-us/training/modules/develop-ai-agents-vs-code/5-exercise



# Unit: Integrate custom tools into your agent  

# Summary: Why use custom tools

![alt text](images-cert/image13.png)


## 1. Core Value Proposition
Custom tools move agents from general assistants to specialized workers by providing:
* **Enhanced Productivity:** Automates repetitive, high-volume tasks unique to a specific company.
* **Improved Accuracy:** Reduces human error by providing consistent, logic-based outputs from trusted data sources.
* **Tailored Solutions:** Addresses niche business needs that off-the-shelf software cannot solve.

## 2. The Decision Process (How Agents Use Tools)
When an agent receives a prompt, it follows a specific reasoning path to decide if a custom tool is required:

1.  **Intent Recognition:** The user asks a specific question (e.g., "What is my order status?").
2.  **Tool Selection:** The agent checks its available "tool belt" and identifies a custom API/tool that can answer the query.
3.  **Execution:** The agent calls the tool (e.g., queries a CRM API).
4.  **Response Synthesis:** The tool returns raw data; the agent formats it into a natural language response for the user.

## 3. Industry-Specific Scenarios
Custom tools enable deep integration across various business functions:

| Industry/Dept | Tool Integration | Key Functionality |
| :--- | :--- | :--- |
| **Customer Support** | CRM Systems | Retrieve order history, process refunds, and update shipping. |
| **Inventory** | Management Systems | Check stock levels, predict restocking needs, and auto-order. |
| **Healthcare** | Patient Records/EMR | Access records, suggest slots, and send automated reminders. |
| **IT Helpdesk** | Ticketing Systems | Troubleshoot technical issues and track ticket resolution status. |
| **E-Learning** | LMS Systems | Recommend courses and track student progress based on data. |

## 4. Operational Impacts
* **Speed:** Faster resolution of queries (Customer Support/IT).
* **Optimization:** Better resource utilization and supply chain efficiency (Inventory/Healthcare).
* **Engagement:** Increased user interaction and personalized learning (E-Learning).
* **Scalability:** Allows departments to handle higher volumes without increasing headcount.


# Summary: Options for implementing custom tools

![alt text](images-cert/image14.png)

The Microsoft Foundry Agent Service provides several options for implementing custom tools, allowing AI agents to integrate with existing infrastructure, web services, and external applications.

### 1. Function Calling (Custom Functions)
* **Description:** Allows you to describe the structure of custom functions to an agent. 
* **How it works:** The agent dynamically identifies the correct function and arguments based on definitions.
* **Key Benefit:** Useful for integrating custom logic and workflows using various programming languages.

### 2. Azure Functions
* **Description:** Supports the creation of intelligent, event-driven applications with minimal overhead.
* **Key Features:**
    * **Triggers:** Determine when a function executes.
    * **Bindings:** Streamline connections to input or output data sources.
* **Key Benefit:** Simplifies interaction between AI agents and external systems.

### 3. OpenAPI Specification Tools
* **Description:** Connects agents to external APIs using the OpenAPI 3.0 standard.
* **How it works:** Uses standard descriptions of HTTP APIs to help the agent understand API functionality.
* **Key Benefit:** Provides standardized, automated, and scalable API integrations, including the ability to generate client code and tests.

### 4. Azure Logic Apps
* **Description:** A low-code/no-code action.
* **Key Benefit:** Facilitates workflows and connects apps, data, and services without extensive manual coding.



# Summary: How to integrate custom tools

![alt text](images-cert/image15.png)

Custom tools allow agents to interact with external systems and process real-time data. The agent determines when to call these tools based on the prompt's context, rather than through explicit code execution by the developer.

## 1. Function Calling

Ideal for executing predefined functions dynamically within the agent's code to retrieve data or process queries.

**Workflow:**

* Define a standard Python function.
* Register it with the `FunctionTool` and `ToolSet`.
* Enable auto-function calls in the agent client.

```python
import json

# Define the function
def recent_snowfall(location: str) -> str:
    """
    Fetches recent snowfall totals for a given location.
    :param location: The city name.
    :return: Snowfall details as a JSON string.
    """
    mock_snow_data = {"Seattle": "0 inches", "Denver": "2 inches"}
    snow = mock_snow_data.get(location, "Data not available.")
    return json.dumps({"location": location, "snowfall": snow})

user_functions: Set[Callable[..., Any]] = {
    recent_snowfall,
}

# Register and initialize
functions = FunctionTool(user_functions)
toolset = ToolSet()
toolset.add(functions)
agent_client.enable_auto_function_calls(toolset=toolset)

# Create agent
agent = agent_client.create_agent(
    model="gpt-4o-mini",
    name="snowfall-agent",
    instructions="You are a weather assistant tracking snowfall. Use the provided functions to answer questions.",
    toolset=toolset
)

```

## 2. Azure Functions

Best for serverless computing and event-driven workflows (e.g., HTTP requests or queue messages).

**Workflow:**

* Deploy an Azure Function.
* Define an `AzureFunctionTool` in the agent configuration, specifying parameters and storage queues for input/output.

```python
storage_service_endpoint = "https://<your-storage>.queue.core.windows.net"

azure_function_tool = AzureFunctionTool(
    name="get_snowfall",
    description="Get snowfall information using Azure Function",
    parameters={
            "type": "object",
            "properties": {
                "location": {"type": "string", "description": "The location to check snowfall."},
            },
            "required": ["location"],
        },
    input_queue=AzureFunctionStorageQueue(
        queue_name="input",
        storage_service_endpoint=storage_service_endpoint,
    ),
    output_queue=AzureFunctionStorageQueue(
        queue_name="output",
        storage_service_endpoint=storage_service_endpoint,
    ),
)

agent = agent_client.create_agent(
    model=os.environ["MODEL_DEPLOYMENT_NAME"],
    name="azure-function-agent",
    instructions="You are a snowfall tracking agent. Use the provided Azure Function to fetch snowfall based on location.",
    tools=azure_function_tool.definitions,
)

```

## 3. OpenAPI Specification

Uses standardized REST API definitions (OpenAPI 3.0) to interact with external services. Supported auth types: Anonymous, API Key, and Managed Identity.

**Workflow:**

* Create an OpenAPI JSON specification file.
* Load the spec and define the `OpenApiTool`.

### API Specification (snowfall_openapi.json)

```json
{
  "openapi": "3.0.0",
  "info": {
    "title": "Snowfall API",
    "version": "1.0.0"
  },
  "paths": {
    "/snow": {
      "get": {
        "summary": "Get snowfall information",
        "parameters": [
          {
            "name": "location",
            "in": "query",
            "required": true,
            "schema": {
              "type": "string"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "Successful response",
            "content": {
              "application/json": {
                "schema": {
                  "type": "object",
                  "properties": {
                    "location": {"type": "string"},
                    "snow": {"type": "string"}
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}

```

### Agent Registration

```python
from azure.ai.agents.models import OpenApiTool, OpenApiAnonymousAuthDetails

with open("snowfall_openapi.json", "r") as f:
    openapi_spec = json.load(f)

auth = OpenApiAnonymousAuthDetails()
openapi_tool = OpenApiTool(name="snowfall_api", spec=openapi_spec, auth=auth)

agent = agent_client.create_agent(
    model="gpt-4o-mini",
    name="openapi-agent",
    instructions="You are a snowfall tracking assistant. Use the API to fetch snowfall data.",
    tools=[openapi_tool]
)
```

## Key Concept: Declarative Integration

The solution is **declarative**. Developers do not write logic to call these tools; instead, by providing meaningful names and documentation for the parameters, the agent "figures out" when and how to call the functions based on the user's prompt.


## Unit Summary:

In this module, we covered the benefits of integrating custom tools into Foundry Agent Service to boost productivity and provide tailored business solutions. By providing custom tools to our agent, we can optimize processes to meet specific needs, resulting in better responses from your agent.

The techniques learned in this module enable businesses to generate marketing materials, improve communications, and analyze market trends more effectively, all through custom tools. The integration of various tool options in the AI Agent Service, from Azure Functions to OpenAPI specifications, allows for the creation of intelligent, event-driven applications that use well-established patterns already used in many businesses.


# Exercise - Build an agent with custom tools

https://learn.microsoft.com/en-us/training/modules/build-agent-with-custom-tools/5-exercise



# Unit: Develop a multi-agent solution with Microsoft Foundry Agent Service


# Summary: Understand connected agents

![alt text](images-cert/image16.png)

## Overview
As AI workflows grow in complexity, single-agent systems can become unmanageable. The **Microsoft Foundry Agent Service** addresses this by enabling a system of connected agents—multiple specialized agents working together cohesively.

## What are Connected Agents?
Connected agents allow for the division of large tasks into smaller, specialized roles. This system eliminates the need for custom orchestrators or hardcoded routing logic.

### Core Architecture
* **Main Agent:** Acts as the central hub; interprets user input and delegates tasks.
* **Sub-agents:** Specialized units designed for specific functions (e.g., document summarization, data retrieval, or policy validation).
* **Mechanism:** Tasks are routed using **natural language** rather than complex manual coding.

## Key Benefits
The division of labor through connected agents provides several advantages:
* **Simplified Workflows:** Breaks down complex processes into manageable parts.
* **Modular Development:** Solutions are easier to build, debug, and reuse across different projects.
* **Reliability and Traceability:** Clear separation of duties makes it easier to test individual agents and identify issues.
* **Extensibility:** New agents can be added or swapped without reworking the entire system.
* **Scalability:** Aligns AI operations with real-world business logic.

## Strategic Value
Using connected agents is particularly effective for:
* Handling sensitive tasks independently (e.g., private data processing).
* Generating personalized content.
* Building flexible systems that do not require custom orchestration logic.


# Summary: Design a multi-agent solution with connected agents

![alt text](images-cert/image17.png)

## Core Design Philosophy
Success in a multi-agent system relies on **clear role definition**. The system operates on a hub-and-spoke model where a central agent manages collaboration between specialized sub-agents.

## Roles and Responsibilities

### 1. The Main Agent (Orchestrator)
The main agent is the "brain" of the operation. Its duties include:
* **Interpreting:** Understanding the user's intent.
* **Selecting:** Choosing the right connected agent for the job.
* **Forwarding:** Providing necessary context and instructions to the sub-agent.
* **Aggregating:** Summarizing results from various agents into a final response.

### 2. Connected Agents (Domain Specialists)
These agents are built with a **single responsibility** (e.g., retrieving stock prices or validating compliance). Their duties include:
* Executing specific actions based on clear prompts.
* Using specialized tools to complete tasks.
* Returning results exclusively to the main agent.

## Implementation Steps
To build this solution in Microsoft Foundry Agent Service, follow these steps:

1. **Initialize the Client:** Connect to the Microsoft Foundry project.
2. **Create the Connected Agent:** Define the sub-agent using `create_agent` and provide specific instructions for its role.
3. **Initialize the ConnectedAgentTool:** Wrap the sub-agent definition into a tool. Assign a **name and description** so the main agent understands its purpose.
4. **Create the Main Agent:** Use `create_agent` and include the sub-agents in the `tools` property.
5. **Manage the Conversation:** * Create a **Thread** to maintain context.
    * Create a **Message** containing the user's request.
6. **Run the Workflow:** Execute a "run." The main agent will delegate tasks and compile the response.
7. **Handle Results:** Review the final output. Note that only the main agent’s response is visible to the user; the sub-agent interactions happen in the background.

## Benefits of the Design
* **Debuggable:** Easier to isolate issues within a single-purpose agent.
* **Reusable:** Connected agents can be used across different solutions.
* **Flexible:** Provides a foundation that scales as business needs grow.


## Unit Summary:

In this module, you learned how to design and implement multi-agent solutions using Microsoft Foundry Agent Service.

Connected agents let you break down complex tasks by assigning them to specialized agents that work together within a coordinated system. You explored how to define clear roles for main and connected agents, delegate tasks using natural language, and design modular workflows that are easier to scale and maintain. You also practiced building a multi-agent solution. Great work!

# Exercise - Develop a multi-agent app with Microsoft Foundry

https://learn.microsoft.com/en-us/training/modules/develop-multi-agent-azure-ai-foundry/4-exercise


# Unit: Integrate MCP Tools with Azure AI Agents

## Summary: Understand MCP tool discovery

![alt text](images-cert/image18.png)

## The Problem
Managing AI tools manually (registering, updating, and hardcoding integrations) becomes unmanageable as systems scale. **Dynamic Tool Discovery** allows agents to find and use tools automatically at runtime.

## Microsoft Connector Protocol (MCP)
The **Microsoft Connector Protocol (MCP)** is a standardized framework that allows AI agents to connect seamlessly to external applications and services. 

### Core Advantages
| Feature | Benefit |
| :--- | :--- |
| **Dynamic Discovery** | Agents receive a "live list" of tools and descriptions; no manual coding for updates. |
| **LLM Interoperability** | Works across different models; switch LLMs without rewriting integrations. |
| **Standardized Security** | Single authentication method for multiple servers; no need for separate API keys. |

## What is Dynamic Tool Discovery?

It is a mechanism where an agent queries a centralized **MCP Server**—acting as a live catalog—to see what tools are available.
* **No Hardcoding:** The agent doesn't need to "know" the tools beforehand.
* **Always Current:** Agents always use the latest version of a tool without code changes.
* **Offloaded Logic:** Managing tool details moves from the agent code to a dedicated service.

## How it Works: The MCP Pipeline
1. **The MCP Server:** Hosts tools defined with the `@mcp.tool` decorator (executable functions).
2. **The MCP Client:** Connects to the server and fetches these tools dynamically.
3. **The AI Agent:** Generates "function wrappers" and adds them to its definitions to respond to user requests.

## Why Use It?
* **Scalability:** Add/update tools without redeploying the agent.
* **Modularity:** Keeps the agent focused on delegation, not tool management.
* **Maintainability:** Centralized management reduces errors and duplication.
* **Flexibility:** Supports rapid evolution of tools and complex, multi-team environments.


## Summary: Integrate agent tools using an MCP server and client

![alt text](images-cert/image19.png)


The Model Context Protocol (MCP) allows Azure AI Agents to dynamically connect to tools by separating tool management from agent logic.

## 1. The MCP Server
The **MCP Server** serves as a tool registry.
* **Initialization:** Created using `FastMCP("server-name")`.
* **Automation:** Uses Python type hints and docstrings to auto-generate tool definitions.
* **Hosting:** Definitions are served over HTTP.
* **Benefit:** Tools can be updated or added on the server without modifying or redeploying the agent.

## 2. The MCP Client
The **MCP Client** acts as the bridge between the server and the Azure AI Agent Service. It performs three main tasks:
1.  **Discovery:** Finds tools using `session.list_tools()`.
2.  **Generation:** Creates Python function stubs to wrap the tools.
3.  **Registration:** Connects these functions to the agent so they can be called like native functions.

## 3. Tool Registration & Execution
* **Invocation:** Tools are called via `session.call_tool(tool_name, tool_args)`.
* **Structure:** Each tool must be wrapped in an **async function**.
* **Bundling:** Functions are bundled into a `FunctionTool` to be included in the agent's toolset at runtime.

## 4. Workflow Overview
1.  **Host:** Define tools on the server using the `@mcp.tool` decorator.
2.  **Connect:** Client initializes a connection to the server.
3.  **Fetch:** Client retrieves definitions via `list_tools()`.
4.  **Wrap:** Each tool is placed in an async function calling `call_tool`.
5.  **Bundle:** Tools are converted into `FunctionTool` objects.
6.  **Register:** `FunctionTool` is added to the agent's toolset.

**Result:** The agent can access and invoke tools through natural language, creating a clean separation between tool maintenance and agent logic.



## Summary: Use Azure AI agents with MCP servers


