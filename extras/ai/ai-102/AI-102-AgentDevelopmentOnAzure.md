https://learn.microsoft.com/training/paths/develop-ai-agents-on-azure/

# Get started with AI agent development on Azure

As generative AI models evolve, they are moving beyond simple chat applications to power **intelligent agents** that operate autonomously to automate tasks, orchestrate business processes, and coordinate workloads.

## Single-Agent Scenario

A single agent can be designed to assist with specific corporate tasks, such as managing expense claims.

- **Knowledge Integration:** Uses generative models combined with policy documentation to answer employee queries regarding claim limits and rules.
- **Automation:** Uses programmatic functions to automatically submit recurring expenses (e.g., monthly bills) or route claims to the correct approver based on the amount.

## Multi-Agent Scenario

In complex environments, multiple agents can coordinate to manage integrated workflows across different business processes.

- **Example:** A **travel booking agent** can book flights and hotels, then automatically provide receipts and data to the **expenses agent** to complete the workflow.

# Summary: Understanding AI Agents

## 1. Definition & Core Capabilities

AI agents are smart applications that use language models to understand intent and **take autonomous action**. Unlike basic chatbots, they maintain conversation memory and execute tasks.

### Three Essential Pillars:

- **Knowledge & Reasoning:** Using generative models and grounding data (e.g., corporate policies) to answer accurately.
- **Task Automation:** Executing programmatic functions (e.g., submitting a claim or booking a flight).
- **Decision-Making:** Using business rules to route tasks or select the next logical step.

---

## 2. Agent Scenarios

- **Single-Agent (Expense Agent):** Accepts a prompt $\rightarrow$ Grounds it with policy data $\rightarrow$ Generates a response $\rightarrow$ Submits a claim/payment.
- **Multi-Agent (Travel + Expense):** A Travel Agent books services via APIs, then automatically triggers the Expense Agent to file receipts—completing an end-to-end workflow without manual intervention.

---

## 3. Security Risks & Mitigation

| Risk Area                | Description                                                                        |
| :----------------------- | :--------------------------------------------------------------------------------- |
| **Data Leakage**         | Agent exposes sensitive/confidential data it shouldn't have shared.                |
| **Prompt Injection**     | Malicious inputs trick the agent into ignoring instructions or leaking passwords.  |
| **Privilege Escalation** | Agent performs unauthorized actions (e.g., deleting records) due to weak controls. |
| **Data Poisoning**       | Corrupted training/context data leads to unsafe or fraudulent outputs.             |
| **Over-reliance**        | Actions taken automatically without necessary human validation.                    |

### Security Best Practices:

- **Least Privilege (RBAC):** Limit agent access to only the data/tools strictly required.
- **Input Validation:** Use filters to block injection attacks.
- **Human-in-the-Loop:** Require human approval for high-stakes or sensitive actions.
- **Auditability:** Maintain comprehensive logs to trace "who, what, when, and why."
- **Supply Chain Safety:** Regularly audit third-party plugins and APIs.

# Summary: AI Agent Development Options

## 1. Evolution of AI: Traditional vs. Agentic

| Feature          | Traditional AI Frameworks         | AI Agent Frameworks                             |
| :--------------- | :-------------------------------- | :---------------------------------------------- |
| **Focus**        | Enhancing apps with intelligence. | Autonomous, goal-oriented systems.              |
| **Capabilities** | Personalization, automation, NLP. | Reasoning, acting, learning, and collaboration. |
| **Interaction**  | Reactive (responds to input).     | Proactive (works to achieve goals).             |

## 2. Key Frameworks & Tools

### Pro-Code / Developer Solutions

- **Microsoft Foundry Agent Service:** Managed Azure service; offers multi-model choice, enterprise security, and Azure/OpenAI SDK support.
- **Microsoft Agent Framework:** Lightweight SDK for building and orchestrating multi-agent systems.
- **Microsoft 365 Agents SDK:** For building self-hosted agents delivered via M365, Slack, or Messenger.
- **AutoGen:** Open-source framework optimized for rapid experimentation and research.
- **OpenAI Assistants API:** Specialized for OpenAI models; integrated into Foundry Agent Service for more flexibility.

### Low-Code / No-Code Solutions

- **Microsoft Copilot Studio:** Visual "citizen developer" environment for deploying agents across enterprise channels.
- **Copilot Studio Lite (M365):** Declarative tool for business users to describe and create basic agents for daily tasks.

## 3. Decision Matrix: Choosing the Right Tool

| User Type                   | Recommended Solution      | Primary Use Case                           |
| :-------------------------- | :------------------------ | :----------------------------------------- |
| **Business User (No Code)** | Copilot Studio Lite       | Automating personal/everyday tasks.        |
| **Power User (Low Code)**   | Copilot Studio (Full)     | Extending Copilot and Teams workflows.     |
| **Pro Dev (M365 Focus)**    | M365 Agents SDK           | Custom extensions for Microsoft ecosystem. |
| **Pro Dev (Azure Focus)**   | Foundry Agent Service     | Scalable, backend-heavy Azure solutions.   |
| **Pro Dev (Multi-Agent)**   | Microsoft Agent Framework | Complex orchestration across environments. |

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

- **Development Paths:**
  - **Visual:** Low-code experience via the Microsoft Foundry portal.
  - **Code-First:** Professional development using the Foundry SDK (Python, C#, etc.).

## 2. Core Components of an Agent

- **Model:** The "brain" that powers reasoning. Supports OpenAI models (GPT-4o) and a broad catalog of open-source models (Llama, etc.).
- **Knowledge (Grounding):** Connects the agent to data to ensure factual accuracy.
  - _Sources:_ Bing Search, Azure AI Search, or custom enterprise documents.
- **Tools (Actions):** Programmatic functions that allow the agent to _do_ things.
  - _Built-in:_ Code Interpreter (runs Python), Bing Search.
  - _Custom:_ Azure Functions or custom code for API integrations.
- **Threads:** The conversation "container" that maintains message history and stores generated assets (like files).

## 3. Key Operational Primitives

- **Instructions:** System prompts that define the agent's persona, goals, and constraints.
- **Runs:** The execution of an agent on a specific thread. During a run, the agent identifies intent, selects tools, and generates responses.
- **Foundry IQ:** A specialized retrieval system (RAG) that allows multiple agents to share a single, scaleable knowledge base.

## 4. Why Use Foundry Agent Service?

- **Security:** Native integration with Microsoft Entra ID (RBAC) and VNet isolation.
- **Observability:** Built-in tracing and logging to monitor agent decisions and performance.
- **Model Router:** Automatically routes tasks to the most efficient/cost-effective model.

**Tip**

1- For more information about Foundry Agent Service, see Microsoft Foundry Agent Service documentation.
https://learn.microsoft.com/en-us/azure/ai-services/agents/

# Exercise: Explore AI Agent development

https://learn.microsoft.com/en-us/training/modules/ai-agent-fundamentals/5-exercise

# Summary: What is an AI Agent

## 1. Definition

A **fully managed service** for building, deploying, and scaling secure, extensible AI agents.

## 2. Key Technical Advantages

- **Serverless Architecture:** Developers do not need to manage underlying compute or storage.
- **Low-Code Efficiency:** Reduces the total volume of code required to deploy agents.
- **Customization:** Supports custom instructions and integration with advanced tools/APIs.
- **Security & Compliance:** Designed for high-stakes industries (like Healthcare) requiring robust data protection.

## 3. Practical Application (Healthcare Example)

- **Use Cases:** Automating patient inquiries, scheduling, and real-time medical data retrieval.
- **Primary Benefit:** Offloads infrastructure and security overhead so teams can focus on the agent's logic and quality rather than the backend.

## 1. What is an AI Agent?

An **AI Agent** is a software service that uses generative AI to understand context and perform tasks **autonomously** on behalf of a user.

- **Core Difference:** Unlike standard chat models, agents don't just "talk"—they **take action** (execute workflows, use tools, and access grounding data).
- **Key Traits:** Goal-oriented, context-aware, and capable of operating without constant human intervention.

## 2. Why Use AI Agents?

- **Automation:** Handles repetitive tasks to free up human creativity.
- **Decision-Making:** Processes massive data sets for real-time, autonomous insights.
- **Scalability:** Grows business operations without needing more staff.
- **24/7 Availability:** Continuous operation for customer service or monitoring.

## 3. Common Use Cases

| Type                      | Example Task                                             |
| :------------------------ | :------------------------------------------------------- |
| **Personal Productivity** | Scheduling, drafting emails (e.g., M365 Copilot).        |
| **Research**              | Monitoring market trends and stock performance.          |
| **Sales**                 | Automating lead qualification and follow-ups.            |
| **Customer Service**      | Handling refunds and routine inquiries (e.g., Cineplex). |
| **Developer**             | Bug fixing and code review (e.g., GitHub Copilot).       |

## 4. Critical Security Risks & Mitigations

| Risk                     | Description                                                 |
| :----------------------- | :---------------------------------------------------------- |
| **Prompt Injection**     | Malicious inputs trick the agent into unauthorized actions. |
| **Data Leakage**         | Unintentional exposure of sensitive/private data.           |
| **Privilege Escalation** | Agent accesses systems/data beyond its intended scope.      |
| **Over-Reliance**        | Executing unintended actions without human oversight.       |

### Mitigation Strategies (Security-by-Design):

- **RBAC:** Enforce Role-Based Access Control and Least Privilege.
- **Human-in-the-Loop:** Gating sensitive actions (e.g., payments) for human approval.
- **Sandboxing:** Isolating operations to prevent system-wide breaches.
- **Prompt Filtering:** Validation layers to block injection attacks.

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

- **Answer questions** using real-time or proprietary data.
- **Make decisions** and perform actions based on user input.
- **Automate workflows** by combining generative AI with tools that interact with real-world data.
- **Common Applications:** Customer support, data analysis, automated reporting, and report generation.

## Key Features

- **Automatic Tool Calling:** Manages the full lifecycle of running models, invoking tools, and returning results.
- **Managed Conversation State:** Uses "threads" to securely manage conversation states automatically.
- **Out-of-the-box Tools:** Includes support for file retrieval, code interpretation, Bing search, Azure AI Search, and Azure Functions.
- **Flexibility:** Allows for specific model selection (Azure OpenAI) and customizable storage (platform-managed or bring-your-own Azure Blob storage).
- **Security:** Provides enterprise-grade security, including keyless authentication and data privacy compliance.

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

> **Note on Selection:** > \* Use **Copilot Studio** for Microsoft 365 integrations.
>
> - Use **Semantic Kernel Agents Framework** for multi-agent orchestration.

## The Development Lifecycle (Implementation Steps)

To integrate an agent into an app via SDK (Python, etc.) or REST API, follow these high-level steps:

1.  **Connect:** Link to the AI Foundry project using the project endpoint and Entra ID authentication.
2.  **Reference/Create Agent:** Define the agent by specifying:
    - **Model Deployment:** The specific model (e.g., GPT-4o) used for reasoning.
    - **Instructions:** Defining behavior and functionality.
    - **Tools:** Resources the agent can invoke.
3.  **Create a Thread:** Establish a stateful chat session that retains history and data.
4.  **Message & Invoke:** Add user messages to the thread and trigger the agent.
5.  **Status Check:** Monitor the thread status; once ready, retrieve messages and artifacts.
6.  **Chat Loop:** Repeat the message and retrieval steps until the session ends.
7.  **Cleanup:** Delete the agent and thread to manage resources and data privacy.

## Agent Tools

Tools allow agents to perform tasks beyond simple text generation. They are categorized into two types:

### 1. Knowledge Tools (Grounding)

Used to enhance the agent's context with real-world or proprietary data:

- **Bing Search:** Real-time web data.
- **File Search:** Data from files in a vector store.
- **Azure AI Search:** Results from custom search indexes.
- **Microsoft Fabric:** Data from Fabric data stores.

### 2. Action Tools (Execution)

Used to perform tasks or run computations:

- **Code Interpreter:** A secure sandbox for running model-generated Python (e.g., for math or data visualization).
- **Custom Functions:** Implementation of your own local code.
- **Azure Functions:** Serverless cloud code execution.
- **OpenAPI Spec:** Calling external APIs via standard specifications.

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

- **Agent Discovery and Management:** Browse, create, and manage agents within existing projects.
- **Visual Agent Designer:** An intuitive graphical interface to configure instructions, models, and capabilities.
- **Integrated Testing (Playground):** Real-time interaction with agents to refine behavior before deployment.
- **Code Generation:** Automatically generates sample integration code to connect agents to applications.
- **Deployment Pipeline:** Direct deployment from VS Code to Microsoft Foundry for production.

## Key Features

- **Tool Integration:** Seamless support for **RAG** (Retrieval-Augmented Generation), Search capabilities, custom actions, and **Model Context Protocol (MCP)** servers.
- **Project Integration:** Direct connection to Microsoft Foundry projects and infrastructure resources.

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

- **Model Choice:** Support for multiple AI models beyond OpenAI.
- **Enterprise Security:** Built-in features for production-grade security.
- **Data Integration:** Connections to Azure data services.
- **Tooling:** Access to built-in and custom tools.

## 2. Agent Creation & Prerequisites

### Prerequisites:

1.  Complete extension setup and sign in to Azure.
2.  Create or select a **Microsoft Foundry project**.
3.  Deploy an AI model to be used by the agent.

### Creation Steps:

1.  Open the **Microsoft Foundry Extension** view in VS Code.
2.  Navigate to **Resources** and click the **+ (plus)** icon next to **Agents**.
3.  The extension opens two views simultaneously:
    - **Agent Designer:** A visual interface for configuration.
    - **YAML File:** The direct configuration file (`.yaml`) containing metadata, model options, and instructions.

## 3. Configuration & Instruction Design

### Basic Properties:

- **Name & Description:** Identity and purpose of the agent.
- **Model Selection:** The specific deployment the agent will use.
- **System Instructions:** Definitions of behavior, personality, and response style.
- **Agent ID:** Automatically generated upon creation.

### Best Practices for Instructions:

- **Specificity:** Define clear actions and behaviors.
- **Context & Boundaries:** Explain the environment and what the agent _cannot_ do.
- **Examples & Personality:** Provide sample interactions and establish a specific tone.

## 4. Deployment & Management

### Deployment Process:

1.  Click **"Create on Microsoft Foundry"** in the Designer view.
2.  Once complete, refresh the **Azure Resources** view to verify the agent appears in the list.

### Management Options:

- **Edit/Redeploy:** Modify configurations and use **"Update on Microsoft Foundry"**.
- **Integration:** Use **"Open Code File"** to generate sample code for application integration.
- **Playground:** Use **"Open Playground"** for real-time testing and validation.

## 5. Testing and Conversation Concepts

The extension uses a specific structure to manage agent interactions:

- **Threads:** Conversation sessions that store messages and context.
- **Messages:** Individual units of interaction (text, images, files).
- **Runs:** Single executions where the agent processes the thread based on its configuration.

# Summary: Extend AI agent capabilities with tools

![alt text](images-cert/image12.png)

## 1. Overview of Agent Tools

Tools are programmatic functions that allow agents to automate actions and access data beyond their initial training. When an agent identifies a need, it can:

- **Invoke** the specific tool automatically.
- **Process** the data/results.
- **Incorporate** the findings into a final user response.

## 2. Built-in Tools in Microsoft Foundry

These tools are production-ready and require no additional setup:

| Tool                    | Capability                            | Use Case                                             |
| :---------------------- | :------------------------------------ | :--------------------------------------------------- |
| **Code Interpreter**    | Writes/executes Python code.          | Math, data analysis, charts, file processing.        |
| **File Search**         | Retrieval-augmented generation (RAG). | Indexing PDFs/Word docs; searching knowledge bases.  |
| **Grounding with Bing** | Real-time internet search.            | Current events, trending topics, and citations.      |
| **OpenAPI Tools**       | Connects to external APIs.            | Integrating with 3rd-party services via OpenAPI 3.0. |
| **MCP**                 | Standardized tool interfaces.         | Using community-driven tools.                        |

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

- **Standardization:** A consistent protocol for all tool communications.
- **Reusability:** Components work across different agents/implementations.
- **Community Support:** Access to tools via MCP registries.
- **Simplified Integration:** Consistent interfaces reduce custom coding.

## 5. Management & Best Practices

To ensure reliable performance in production:

- **Identify Requirements:** Only add tools that match the agent's specific role.
- **Start Simple:** Prioritize built-in tools before developing custom ones.
- **Rigorous Testing:** Validate tool behavior in the playground under various scenarios.
- **Monitor Usage:** Track tool effectiveness and performance to optimize response times.

# Exercise - Build an AI agent using the Microsoft Foundry extension

https://learn.microsoft.com/en-us/training/modules/develop-ai-agents-vs-code/5-exercise

# Unit: Integrate custom tools into your agent

# Summary: Why use custom tools

![alt text](images-cert/image13.png)

## 1. Core Value Proposition

Custom tools move agents from general assistants to specialized workers by providing:

- **Enhanced Productivity:** Automates repetitive, high-volume tasks unique to a specific company.
- **Improved Accuracy:** Reduces human error by providing consistent, logic-based outputs from trusted data sources.
- **Tailored Solutions:** Addresses niche business needs that off-the-shelf software cannot solve.

## 2. The Decision Process (How Agents Use Tools)

When an agent receives a prompt, it follows a specific reasoning path to decide if a custom tool is required:

1.  **Intent Recognition:** The user asks a specific question (e.g., "What is my order status?").
2.  **Tool Selection:** The agent checks its available "tool belt" and identifies a custom API/tool that can answer the query.
3.  **Execution:** The agent calls the tool (e.g., queries a CRM API).
4.  **Response Synthesis:** The tool returns raw data; the agent formats it into a natural language response for the user.

## 3. Industry-Specific Scenarios

Custom tools enable deep integration across various business functions:

| Industry/Dept        | Tool Integration    | Key Functionality                                                 |
| :------------------- | :------------------ | :---------------------------------------------------------------- |
| **Customer Support** | CRM Systems         | Retrieve order history, process refunds, and update shipping.     |
| **Inventory**        | Management Systems  | Check stock levels, predict restocking needs, and auto-order.     |
| **Healthcare**       | Patient Records/EMR | Access records, suggest slots, and send automated reminders.      |
| **IT Helpdesk**      | Ticketing Systems   | Troubleshoot technical issues and track ticket resolution status. |
| **E-Learning**       | LMS Systems         | Recommend courses and track student progress based on data.       |

## 4. Operational Impacts

- **Speed:** Faster resolution of queries (Customer Support/IT).
- **Optimization:** Better resource utilization and supply chain efficiency (Inventory/Healthcare).
- **Engagement:** Increased user interaction and personalized learning (E-Learning).
- **Scalability:** Allows departments to handle higher volumes without increasing headcount.

# Summary: Options for implementing custom tools

![alt text](images-cert/image14.png)

The Microsoft Foundry Agent Service provides several options for implementing custom tools, allowing AI agents to integrate with existing infrastructure, web services, and external applications.

### 1. Function Calling (Custom Functions)

- **Description:** Allows you to describe the structure of custom functions to an agent.
- **How it works:** The agent dynamically identifies the correct function and arguments based on definitions.
- **Key Benefit:** Useful for integrating custom logic and workflows using various programming languages.

### 2. Azure Functions

- **Description:** Supports the creation of intelligent, event-driven applications with minimal overhead.
- **Key Features:**
  - **Triggers:** Determine when a function executes.
  - **Bindings:** Streamline connections to input or output data sources.
- **Key Benefit:** Simplifies interaction between AI agents and external systems.

### 3. OpenAPI Specification Tools

- **Description:** Connects agents to external APIs using the OpenAPI 3.0 standard.
- **How it works:** Uses standard descriptions of HTTP APIs to help the agent understand API functionality.
- **Key Benefit:** Provides standardized, automated, and scalable API integrations, including the ability to generate client code and tests.

### 4. Azure Logic Apps

- **Description:** A low-code/no-code action.
- **Key Benefit:** Facilitates workflows and connects apps, data, and services without extensive manual coding.

# Summary: How to integrate custom tools

![alt text](images-cert/image15.png)

Custom tools allow agents to interact with external systems and process real-time data. The agent determines when to call these tools based on the prompt's context, rather than through explicit code execution by the developer.

## 1. Function Calling

Ideal for executing predefined functions dynamically within the agent's code to retrieve data or process queries.

**Workflow:**

- Define a standard Python function.
- Register it with the `FunctionTool` and `ToolSet`.
- Enable auto-function calls in the agent client.

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

- Deploy an Azure Function.
- Define an `AzureFunctionTool` in the agent configuration, specifying parameters and storage queues for input/output.

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

- Create an OpenAPI JSON specification file.
- Load the spec and define the `OpenApiTool`.

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
                    "location": { "type": "string" },
                    "snow": { "type": "string" }
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

- **Main Agent:** Acts as the central hub; interprets user input and delegates tasks.
- **Sub-agents:** Specialized units designed for specific functions (e.g., document summarization, data retrieval, or policy validation).
- **Mechanism:** Tasks are routed using **natural language** rather than complex manual coding.

## Key Benefits

The division of labor through connected agents provides several advantages:

- **Simplified Workflows:** Breaks down complex processes into manageable parts.
- **Modular Development:** Solutions are easier to build, debug, and reuse across different projects.
- **Reliability and Traceability:** Clear separation of duties makes it easier to test individual agents and identify issues.
- **Extensibility:** New agents can be added or swapped without reworking the entire system.
- **Scalability:** Aligns AI operations with real-world business logic.

## Strategic Value

Using connected agents is particularly effective for:

- Handling sensitive tasks independently (e.g., private data processing).
- Generating personalized content.
- Building flexible systems that do not require custom orchestration logic.

# Summary: Design a multi-agent solution with connected agents

![alt text](images-cert/image17.png)

## Core Design Philosophy

Success in a multi-agent system relies on **clear role definition**. The system operates on a hub-and-spoke model where a central agent manages collaboration between specialized sub-agents.

## Roles and Responsibilities

### 1. The Main Agent (Orchestrator)

The main agent is the "brain" of the operation. Its duties include:

- **Interpreting:** Understanding the user's intent.
- **Selecting:** Choosing the right connected agent for the job.
- **Forwarding:** Providing necessary context and instructions to the sub-agent.
- **Aggregating:** Summarizing results from various agents into a final response.

### 2. Connected Agents (Domain Specialists)

These agents are built with a **single responsibility** (e.g., retrieving stock prices or validating compliance). Their duties include:

- Executing specific actions based on clear prompts.
- Using specialized tools to complete tasks.
- Returning results exclusively to the main agent.

## Implementation Steps

To build this solution in Microsoft Foundry Agent Service, follow these steps:

1. **Initialize the Client:** Connect to the Microsoft Foundry project.
2. **Create the Connected Agent:** Define the sub-agent using `create_agent` and provide specific instructions for its role.
3. **Initialize the ConnectedAgentTool:** Wrap the sub-agent definition into a tool. Assign a **name and description** so the main agent understands its purpose.
4. **Create the Main Agent:** Use `create_agent` and include the sub-agents in the `tools` property.
5. **Manage the Conversation:** \* Create a **Thread** to maintain context.
   - Create a **Message** containing the user's request.
6. **Run the Workflow:** Execute a "run." The main agent will delegate tasks and compile the response.
7. **Handle Results:** Review the final output. Note that only the main agent’s response is visible to the user; the sub-agent interactions happen in the background.

## Benefits of the Design

- **Debuggable:** Easier to isolate issues within a single-purpose agent.
- **Reusable:** Connected agents can be used across different solutions.
- **Flexible:** Provides a foundation that scales as business needs grow.

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

| Feature                   | Benefit                                                                               |
| :------------------------ | :------------------------------------------------------------------------------------ |
| **Dynamic Discovery**     | Agents receive a "live list" of tools and descriptions; no manual coding for updates. |
| **LLM Interoperability**  | Works across different models; switch LLMs without rewriting integrations.            |
| **Standardized Security** | Single authentication method for multiple servers; no need for separate API keys.     |

## What is Dynamic Tool Discovery?

It is a mechanism where an agent queries a centralized **MCP Server**—acting as a live catalog—to see what tools are available.

- **No Hardcoding:** The agent doesn't need to "know" the tools beforehand.
- **Always Current:** Agents always use the latest version of a tool without code changes.
- **Offloaded Logic:** Managing tool details moves from the agent code to a dedicated service.

## How it Works: The MCP Pipeline

1. **The MCP Server:** Hosts tools defined with the `@mcp.tool` decorator (executable functions).
2. **The MCP Client:** Connects to the server and fetches these tools dynamically.
3. **The AI Agent:** Generates "function wrappers" and adds them to its definitions to respond to user requests.

## Why Use It?

- **Scalability:** Add/update tools without redeploying the agent.
- **Modularity:** Keeps the agent focused on delegation, not tool management.
- **Maintainability:** Centralized management reduces errors and duplication.
- **Flexibility:** Supports rapid evolution of tools and complex, multi-team environments.

## Summary: Integrate agent tools using an MCP server and client

![alt text](images-cert/image19.png)

The Model Context Protocol (MCP) allows Azure AI Agents to dynamically connect to tools by separating tool management from agent logic.

## 1. The MCP Server

The **MCP Server** serves as a tool registry.

- **Initialization:** Created using `FastMCP("server-name")`.
- **Automation:** Uses Python type hints and docstrings to auto-generate tool definitions.
- **Hosting:** Definitions are served over HTTP.
- **Benefit:** Tools can be updated or added on the server without modifying or redeploying the agent.

## 2. The MCP Client

The **MCP Client** acts as the bridge between the server and the Azure AI Agent Service. It performs three main tasks:

1.  **Discovery:** Finds tools using `session.list_tools()`.
2.  **Generation:** Creates Python function stubs to wrap the tools.
3.  **Registration:** Connects these functions to the agent so they can be called like native functions.

## 3. Tool Registration & Execution

- **Invocation:** Tools are called via `session.call_tool(tool_name, tool_args)`.
- **Structure:** Each tool must be wrapped in an **async function**.
- **Bundling:** Functions are bundled into a `FunctionTool` to be included in the agent's toolset at runtime.

## 4. Workflow Overview

1.  **Host:** Define tools on the server using the `@mcp.tool` decorator.
2.  **Connect:** Client initializes a connection to the server.
3.  **Fetch:** Client retrieves definitions via `list_tools()`.
4.  **Wrap:** Each tool is placed in an async function calling `call_tool`.
5.  **Bundle:** Tools are converted into `FunctionTool` objects.
6.  **Register:** `FunctionTool` is added to the agent's toolset.

**Result:** The agent can access and invoke tools through natural language, creating a clean separation between tool maintenance and agent logic.

## Summary: Use Azure AI agents with MCP servers

![alt text](images-cert/image20.png)

## Overview

Model Context Protocol (MCP) servers allow Microsoft Foundry agents to access external tools and contextual data. This extends agent capabilities beyond built-in functions without the need to manually manage client sessions or individual function tools.

## Key Benefits

- **Simplified Integration:** No manual MCP client sessions or function tool creation required.
- **Scalability:** Connect to multiple servers simultaneously by defining separate tool objects.
- **Dynamic Access:** Use different tools from various servers based on specific prompt needs.

## Integration Requirements

To integrate a remote MCP server, you must have:

1.  **Remote Endpoint:** A valid URL (e.g., `https://api.githubcopilot.com/mcp/`).
2.  **Configured Agent:** An agent set up to utilize the MCP tool.

### Server Configuration Parameters

- **server_label:** A unique identifier for the server.
- **server_url:** The specific endpoint for the MCP server.
- **allowed_tools (Optional):** A whitelist of specific tools the agent can access.
- **Custom Headers:** Used for authentication (API keys, OAuth tokens) or other requirements. These are passed in `tool_resources` per run and are not stored.

## Tool Invocation Process

Tools are automatically invoked during an agent run. The workflow includes:

1.  **Object Creation:** Create the `McpTool` with the label and URL.
2.  **Header Application:** Use `update_headers` for required server credentials.
3.  **Approval Mode:** Set using `set_approval_mode`:
    - `always` (Default): Requires developer approval for every call.
    - `never`: Automatic execution without approval.
4.  **Toolset Management:** Add the `McpTool` to a `ToolSet` object and specify it in the agent run.

## Handling Approvals (Requires Action)

If approval is required, the run status will change to `requires_action`.

- **Review:** Check the `requires_action` field for the specific tool being called and its arguments.
- **Submission:** Submit approval by providing the `call_id` and setting `approve` to `true`.

# Exercise - Connect MCP tools to Azure AI Agents

https://learn.microsoft.com/en-us/training/modules/connect-agent-to-mcp-tools/5-exercise

# Unit: Develop an AI agent with Microsoft Agent Framework

## Summary: Introduction

## 1. Overview of AI Agents

- **Definition:** Unlike traditional programs, AI agents use generative AI to interpret data, make decisions, and complete tasks with minimal human intervention.
- **Core Function:** They leverage large language models (LLMs) to streamline complex workflows and automate business processes.

## 2. The Microsoft Agent Framework

- **Purpose:** An open-source SDK that simplifies integrating AI models into applications.
- **Compatibility:** Supports multiple providers, including:
  - Microsoft Foundry
  - Azure OpenAI & OpenAI
  - Microsoft Copilot Studio
  - Anthropic
- **Key Advantage:** Provides flexibility, scalability, and **workflow orchestration** for multi-agent solutions where collaborative agents must be coordinated.

## 3. Microsoft Foundry Agent Service

- **Service Model:** A fully managed, enterprise-grade service for building, deploying, and scaling extensible AI agents.
- **Infrastructure:** Developers do not need to manage underlying compute or storage resources.
- **Integration:** Using the Microsoft Agent Framework with this service allows for natural language processing and access to built-in tools with minimal code.

## 4. Practical Application (Tools & Functions)

- **Capabilities:** Agents can interact with APIs, retrieve data, and execute tasks.
- **Example Use Case:** An agent that extracts data from expense reports, formats them, and emails them to recipients automatically.

## 5. Learning Objectives

- Connect to a Microsoft Foundry project using the Framework.
- Create Foundry agents within the Framework.
- Extend agent capabilities by integrating **tool functions**.

## Summary: Understand Microsoft Agent Framework AI agents

![alt text](images-cert/image21.png)

## 1. Core Definition: AI Agents

- **Nature:** Autonomous programs using Generative AI (LLMs) to interpret data and make decisions.
- **Advantage:** Unlike traditional software, they handle complex workflows without continuous human oversight.
- **Framework Role:** The Microsoft Agent Framework is an open-source SDK that integrates these models into applications.

## 2. Microsoft Agent Framework: Core Components

The framework is modular, allowing components to be used individually or together:

- **Agents:** The interface for multi-agent orchestration, supporting function calling, chat history, and streaming.
- **Chat Providers:** Abstractions that allow a common interface for different services (Azure OpenAI, OpenAI, Anthropic, etc.).
- **Function Tools:** Custom containers that allow agents to trigger external APIs.
- **Built-in Tools:** Prebuilt capabilities like **Code Interpreter (Python)**, **File Search**, and **Web Search**.
- **Conversation Management:** Uses `AgentSession` for persistent context and a role-based message system (`USER`, `ASSISTANT`, `SYSTEM`, `TOOL`).
- **Workflow Orchestration:** Manages sequential tasks, concurrent execution, and "handoff" patterns between agents.

## 3. Microsoft Foundry Agents

These are "Enterprise-Level" versions of agents designed specifically for Azure environments.

- **Key Benefits:**
  - **Enterprise Integration:** Secure, compliant, and supports RBAC (Role-Based Access Control).
  - **Automatic Invocation:** Seamlessly calls tools like Azure AI Search or Azure Functions.
  - **Thread Management:** Built-in mechanisms to maintain conversation states across long sessions.
  - **Advanced Tools:** Support for Model Context Protocol (MCP) and Python execution environments.

## 4. Fundamental Framework Concepts (Quick Reference)

| Concept                 | Description                                                       |
| :---------------------- | :---------------------------------------------------------------- |
| **BaseAgent**           | The unified foundation/interface for all agent types.             |
| **Agent Session**       | Persistent storage for conversation history.                      |
| **Multi-modal Support** | Ability to process text, images, and structured outputs.          |
| **Schema Generation**   | Automatic creation of schemas from Python functions for tool use. |
| **Authentication**      | Supports Azure CLI, API keys, MSAL, and RBAC.                     |

## 5. Key Takeaway for Implementation

The architecture is provider-agnostic. You can switch between LLM providers (OpenAI to Anthropic) without changing your core code, enabling everything from simple bots to complex, multi-agent business solutions.

## Summary: Create an Azure AI agent with Microsoft Agent Framework

![alt text](images-cert/image22.png)

## 1. The Microsoft Foundry Agent Advantage

- **Specialization:** Designed for enterprise-level conversational capabilities.
- **Automation:** Automatically handles tool calling (no manual parsing/invocation needed).
- **State Management:** Securely manages conversation history using **threads**, reducing the manual overhead of maintaining state.
- **Native Tool Support:** Includes Code Interpreter, File Search, Web Search, Azure AI Search, and Azure Functions.

## 2. Implementation Workflow (5 Steps)

To deploy a Microsoft Foundry Agent, follow this sequence:

1. **Project Setup:** Create a Microsoft Foundry project.
2. **Configuration:** Add the project connection string to your application code.
3. **Authentication:** Set up credentials (typically using `AzureCliCredential`).
4. **Client Connection:** Connect to the project using the `AzureOpenAIResponsesClient` class.
5. **Agent Initialization:** Create an `Agent` instance by defining:
   - The Client
   - Instructions (System prompts)
   - Tools (Capabilities)

## 3. Key Technical Components

| Component                      | Function                                                                                  |
| :----------------------------- | :---------------------------------------------------------------------------------------- |
| **AzureOpenAIResponsesClient** | Manages the project connection; provides enterprise authentication and model access.      |
| **Agent Class**                | The "runtime" that combines the client, instructions, and tools into a working unit.      |
| **AgentSession**               | Tracks history and manages conversation state; allows for reusing existing threads.       |
| **Tools Integration**          | Automatically registers custom functions to connect with external APIs.                   |
| **Azure Identity**             | Supports `AzureCliCredential`, service principals, and MSAL for secure access.            |
| **Thread Management**          | Offers "Automatic" (simple scenarios) vs. "Explicit" (complex/long-term) thread handling. |

## 4. Operational Summary

The Foundry Agent acts as a **self-contained runtime**. By combining instructions with the `AgentSession`, it maintains security and scalability while ensuring the AI model has the context necessary to complete business-specific tasks.

## Summary: Add tools to Azure AI agent

![alt text](images-cert/image23.png)

## 1. How Tools Work

- **Mechanism:** Tools use **Function Calling**. The AI identifies a need, requests a specific function, the framework executes it in your codebase, and the results are fed back to the LLM.
- **Requirement:** For automatic calling to work, the function’s **input, output, and purpose** must be clearly described so the AI understands "how" and "when" to use it.

## 2. Built-in Tools (Ready-to-Use)

Microsoft Foundry Agents include three "out-of-the-box" tools that require no extra setup:

- **Code Interpreter:** Executes Python for math, data analysis, or logic.
- **File Search:** Analyzes and searches through uploaded documents.
- **Web Search:** Accesses real-time internet information.

## 3. Creating Custom Function Tools

To build your own tools, follow these core Python concepts:

- **The `@tool` Decorator:** Applied to a function to register it. It includes parameters for `name`, `description`, and `approval_mode`.
- **Type Annotations:** Use Pydantic’s `Annotated` and `Field` to give the AI precise instructions on what each parameter does.
- **Registration:** Pass the functions into the `ChatAgent` using the `tools` parameter (as a single function or a list).

## 4. Orchestration & Invocation

- **Automatic Execution:** You do not manually call the tool. The AI decides to trigger it based on the user's natural language request and the tool's description.
- **Multi-Tool Handling:** One agent can hold multiple tools; the framework automatically selects the right one or combines results from several.

## 5. Tool Development Best Practices

| Practice               | Importance                                                                     |
| :--------------------- | :----------------------------------------------------------------------------- |
| **Clear Descriptions** | Helps the AI understand the _intent_ of the tool.                              |
| **Type Hints**         | Ensures the AI passes the correct data formats (str, int, etc.).               |
| **Error Handling**     | Prevents the agent from crashing on unexpected inputs.                         |
| **Focused Design**     | Each tool should handle **one specific task** rather than being a "catch-all." |
| **Meaningful Returns** | Return data in a format the LLM can easily summarize for the user.             |

# Exercise - Develop an Azure AI agent with the Microsoft Agent Framework SDK

https://learn.microsoft.com/en-us/training/modules/develop-ai-agent-with-semantic-kernel/5-exercise

# Unit: Orchestrate a multi-agent solution using the Microsoft Agent Framework

## Summary: Introduction

## 1. The Multi-Agent Concept

- **When to use:** When a task is too large or complex for a single, realistic AI agent.
- **Core Benefit:** Allows specialized agents to collaborate within the **same conversation**, breaking down complex problems into modular, manageable parts.

## 2. Practical Example: DevOps Multi-Agent System

A specialized system can be built using four distinct agents working in a sequence:

- **Monitoring Agent:** Uses NLP to ingest logs/metrics and detect anomalies or trigger alerts.
- **Root Cause Analysis (RCA) Agent:** Correlates anomalies with system changes to pinpoint the source of the problem.
- **Automated Deployment Agent:** Interacts with CI/CD pipelines to implement fixes or roll back changes.
- **Reporting Agent:** Summarizes the entire process (anomaly, cause, resolution) and notifies stakeholders via email/comms.

## 3. Key Advantages

- **Modularity:** Each agent focuses on a specific domain (Separation of Concerns).
- **Efficiency:** Reduces manual human intervention in technical workflows.
- **Scalability:** Intelligent orchestration ensures that issues are resolved and communicated in a timely manner.

## 4. Orchestration Patterns

The Microsoft Agent Framework provides the "glue" for these agents to work together. Key capabilities include:

- **Collaborative Design:** Orchestrating agents so they can hand off tasks to one another.
- **Task Distribution:** Using tools and plugins across different agents in the same environment.

## 5. Learning Objectives for this Module

- Build agents using the **Microsoft Agent Framework SDK**.
- Implement **tools and plugins** for specialized functionality.
- Identify and apply different **orchestration patterns** (e.g., sequential, parallel).
- Develop end-to-end **multi-agent solutions**.

## Summary: Understand the Microsoft Agent Framework

![alt text](images-cert/image24.png)

## 1. Overview

The **Microsoft Agent Framework** is an open-source SDK designed for integrating AI models into applications. It allows developers to build agents that work independently or collaboratively to complete complex tasks by combining large language models (LLMs) with traditional programming logic.

## 2. Core Components

- **Agents:** AI-driven entities that use reasoning, tools, and conversation history to execute tasks and respond to user needs.
- **Agent Orchestration:** A unified interface that allows multiple agents to collaborate. It supports various patterns, enabling developers to switch orchestration styles without rewriting core logic.
- **Chat Clients:** Provides a common interface (`BaseChatClient`) to connect to various AI providers, including Azure OpenAI, OpenAI, and Anthropic.
- **Tools and Function Integration:** Extends agent capabilities via:
  - **Custom Functions:** Integration with external APIs and code execution.
  - **Built-in Tools:** Code Interpreter, File Search, and Web Search.
- **Conversation Management:** Uses `AgentSession` to maintain history. It utilizes a structured message system with specific roles: `USER`, `ASSISTANT`, `SYSTEM`, and `TOOL`.

## 3. Key Benefits & Use Cases

- **Provider-Agnostic:** The design allows switching between AI providers without changing the underlying code.
- **Collaboration:** Supports multi-agent workflows where specialized agents handle specific tasks (e.g., data collection or analysis).
- **Human-Agent Interaction:** Facilitates "human-in-the-loop" processes, where agents augment human decision-making and automate repetitive tasks.
- **Versatility:** Suitable for a range of applications, from basic chatbots to complex enterprise-level systems.

## Summary: Understand agent orchestration

![alt text](images-cert/image25.png)

## 1. Why Orchestration Matters

Orchestration moves beyond single-agent limitations by combining specialized agents. This allows for:

- **Specialization:** Assigning distinct roles, skills, or perspectives to different agents.
- **Improved Accuracy:** Combining outputs from multiple agents for better decision-making.
- **Complex Pipelines:** Coordinating multi-stage tasks that a single agent couldn't handle alone.
- **Dynamic Routing:** Moving control between agents based on the specific context of the task.

## 2. Core Components of a Workflow

Workflows are structured sequences that provide control, multi-agent coordination, and **checkpointing** (saving/resuming state).

### **Executors**

The "workers" of the workflow. They can be **AI agents** or **custom logic** components. They take input, perform an action, and produce output.

### **Edges (The Logic Flow)**

Edges determine how messages move between executors:

- **Direct:** Sequential flow (A → B).
- **Conditional:** Triggers only if specific criteria are met.
- **Switch-Case:** Routes to different executors based on predefined rules (e.g., VIP vs. Standard).
- **Fan-Out:** Sends one message to multiple executors at once (parallel processing).
- **Fan-In:** Combines results from multiple executors into one final output.

### **Events (Observability)**

- More info:
  https://learn.microsoft.com/en-us/training/modules/orchestrate-semantic-kernel-multi-agent-solution/3-understand-agent-orchestration

Built-in tracking for debugging and monitoring, including:

- `WorkflowStartedEvent` / `WorkflowOutputEvent`
- `ExecutorInvokeEvent` / `ExecutorCompleteEvent`
- `WorkflowErrorEvent`

## 3. Supported Orchestration Patterns

The SDK offers several "technology-agnostic" patterns:
| Pattern | Description | Best For |
| :--- | :--- | :--- |
| **Concurrent** | Broadcasts tasks to multiple agents at once. | Parallel analysis, ensemble decisions. |
| **Sequential** | Pass output from one agent to the next in a fixed order. | Step-by-step pipelines. |
| **Handoff** | Dynamically transfers control between agents. | Escalations, expert routing. |
| **Group Chat** | Shared conversation managed by a "chat manager." | Brainstorming, building consensus. |
| **Magentic** | Manager-driven; plans and adapts the path. | Open-ended, evolving problems. |

## 4. Implementation Flow

The framework uses a unified interface, allowing developers to swap patterns without rewriting agent logic:

1. **Define** agents and their capabilities.
2. **Select** an orchestration pattern.
3. **Configure** optional callbacks or transforms.
4. **Start** the runtime.
5. **Invoke** the task and retrieve results asynchronously.
6. **Retrieve** results in an asynchronous, nonblocking way.

## Summary: Use concurrent orchestration

![alt text](images-cert/image26.png)

## 1. Concept Overview

**Concurrent Orchestration** allows multiple agents to work on the same task simultaneously and independently. Instead of working in a sequence, agents generate diverse approaches or solutions in parallel, which are later gathered and combined.

## 2. When to Use vs. When to Avoid

### **Use When:**

- **Speed is a priority:** Running agents in parallel reduces total wait time.
- **Diversity of thought is needed:** Ideal for brainstorming, ensemble reasoning (combining different methods), or reaching a consensus (voting/quorum).
- **Independent tasks:** Agents have specialized skills (technical, business, creative) that do not require input from one another to begin.

### **Avoid When:**

- **Dependencies exist:** If Agent B needs Agent A's output to start, use sequential orchestration instead.
- **Resource constraints:** High parallel usage may hit model rate limits or quotas.
- **Conflict resolution is difficult:** If agents produce contradictory data that cannot be easily reconciled.
- **Strict sequencing:** The task requires a specific, repeatable order of operations.

## 3. Key Characteristics

- **Independence:** Agents do not share results with each other during execution.
- **Dynamic or Fixed:** You can call a fixed group of agents or select them dynamically based on the task.
- **Flexible Outputs:** Results can be merged into a single final answer or kept as separate actions (e.g., updating different databases).

## 4. Implementation Steps

The Microsoft Agent Framework uses a specific workflow for concurrent execution:

1.  **Create Chat Client:** Initialize the connection (e.g., `AzureOpenAIChatClient`).
2.  **Define Agents:** Create instances using `create_agent`, assigning unique roles and instructions to each.
3.  **Build Workflow:** Use the `ConcurrentBuilder` class. Add agents via the `.participants()` method and finalize with `.build()`.
4.  **Run:** Execute the workflow's `.run()` method with the initial input.
5.  **Process Results:** Use `get_outputs()` to extract results. The output contains the aggregated conversation history, identified by each agent's name.

## 5. Summary Table: Concurrent Workflow

| Feature           | Description                                           |
| :---------------- | :---------------------------------------------------- |
| **Logic Type**    | Parallel / Non-linear                                 |
| **Primary Tool**  | `ConcurrentBuilder`                                   |
| **Communication** | Agents are unaware of each other's real-time progress |
| **Outcome**       | Aggregated responses from all participants            |

## Summary: Use sequential orchestration

![alt text](images-cert/image27.png)

## 1. Concept Overview

**Sequential Orchestration** arranges agents in a linear pipeline. Each agent processes a task one after another, where the **output of one agent becomes the direct input for the next**. This creates a "step-by-step" improvement process with a fixed, predefined order.

## 2. When to Use vs. When to Avoid

### **Use When:**

- **Ordered Dependencies:** Steps must happen in a specific sequence (e.g., Draft → Review → Polish).
- **Data Transformation:** Each stage adds critical information that the next stage requires to function.
- **Gradual Refinement:** Tasks that benefit from multiple layers of processing (e.g., content creation or multi-stage reasoning).
- **Predictable Workflows:** You know exactly which agent needs to perform which task in what order.

### **Avoid When:**

- **Independence:** If stages can run in parallel without affecting quality (use Concurrent instead).
- **Simplicity:** A single agent can handle the entire task effectively.
- **Dynamic Needs:** The workflow requires backtracking, looping, or changing direction based on intermediate results.
- **Risk of Early Failure:** If an early stage produces poor output and there is no mechanism to stop or correct the downstream agents.

## 3. Key Characteristics

- **Linear Flow:** Control moves strictly from Agent A to Agent B to Agent C.
- **Fixed Order:** The sequence is decided beforehand; agents do not decide who speaks next.
- **Cumulative Context:** The final output contains the complete history of how each agent contributed.

## 4. Implementation Steps

The Microsoft Agent Framework uses the following workflow for sequential execution:

1.  **Create Chat Client:** Establish the connection (e.g., `AzureOpenAIChatClient`).
2.  **Define Agents:** Create instances for each "station" in the pipeline (e.g., a "Researcher" agent and an "Editor" agent).
3.  **Build Workflow:** Use the `SequentialBuilder` class. Add agents in the desired order via `.participants()` and finalize with `.build()`.
4.  **Run with Streaming:** Use the `run_stream` method. This allows the task to flow through the sequence asynchronously.
5.  **Process Events:** Use an async loop to monitor `WorkflowOutputEvent` instances.
6.  **Extract Conversation:** Collect the final output, which represents the refined result of the entire chain.

## 5. Summary Table: Sequential Workflow

| Feature          | Description                                             |
| :--------------- | :------------------------------------------------------ |
| **Logic Type**   | Linear Pipeline                                         |
| **Primary Tool** | `SequentialBuilder`                                     |
| **Data Flow**    | Output(n) becomes Input(n+1)                            |
| **Best For**     | Document review, data pipelines, step-by-step reasoning |

## Summary: Use group chat orchestration

![alt text](images-cert/image28.png)

## 1. Overview

Group chat orchestration models collaborative conversations between multiple AI agents and optional human participants. A **central chat manager** controls the flow, deciding who speaks next and when the process ends.

- **Goal:** Simulate meetings, debates, or collaborative problem-solving.
- **Key Feature:** Agents contribute to a conversation rather than directly changing systems.
- **Support:** Works for both free-flowing ideation and formal workflows with approval steps.

## 2. When to Use vs. Avoid

### Use When:

- **Collaboration is needed:** Spontaneous or guided interaction between agents/humans.
- **Iterative loops:** "Maker-checker" setups where agents review each other's work.
- **Oversight:** Scenarios requiring real-time human intervention.
- **Transparency:** All outputs are collected in a single, auditable thread.
- **Complex tasks:** Brainstorming, consensus-building, or cross-disciplinary dialogue.

### Avoid When:

- Tasks are simple, linear, or involve basic delegation.
- Real-time speed requirements make discussion overhead too slow.
- Workflows are strictly hierarchical or deterministic.
- The chat manager cannot clearly define a completion point.
- **Agent Count:** Avoid having too many agents (limit to three or fewer for easier control).

## 3. The Maker-Checker Loop

A specific type of orchestration where:

1.  **The Maker:** Proposes content or solutions.
2.  **The Checker:** Reviews and critiques the proposal.
3.  **The Cycle:** Feedback is sent back to the maker; the loop repeats until the result is satisfactory.

## 4. Implementation (Microsoft Agent Framework SDK)

To implement this pattern, follow these steps:

1.  **Create Chat Client:** Set up a client (e.g., `AzureOpenAIChatClient`) with credentials.
2.  **Define Agents:** Use `create_agent` to create instances with specific names and roles.
3.  **Build Workflow:** Use the `GroupChatBuilder` class. Add agents via `.participants()` and finalize with `.build()`.
4.  **Run Workflow:** Use the `.run()` method with the initial task.
5.  **Process Results:** Use `get_outputs()` to extract aggregated messages from the workflow events.
6.  **Identify Responses:** Each message includes the author's name to identify the specific agent.

## 5. Customizing the Group Chat Manager

By extending the `GroupChatManager` class, you can control:

- Filtering/summarizing results.
- Selecting the next agent.
- Timing for human intervention.
- Termination conditions.

### Call Order (Logic Flow)

During each round, the manager calls methods in this specific order:

1.  `should_request_user_input`: Checks if a human needs to intervene.
2.  `should_terminate`: Checks if the conversation should end (e.g., max rounds).
3.  `filter_results`: Processes/summarizes the final thread if terminating.
4.  `select_next_agent`: Chooses the next participant if continuing.

## Summary: Use handoff orchestration

![alt text](images-cert/image29.png)

## 1. Overview

Handoff orchestration allows AI agents to transfer control to one another based on task context or specific user requests. Unlike parallel patterns, agents work **one at a time**, ensuring the most qualified expert handles the current stage of the task.

- **Core Concept:** Dynamic delegation where the "best" agent may not be known at the start.
- **Ideal for:** Customer support, expert systems, and multi-domain problem solving.

## 2. When to Use vs. Avoid

### Use When:

- **Emergent Expertise:** Requirements become clear only during processing.
- **Sequential Specialization:** Problems require different specialists to work in a specific order.
- **Dynamic Routing:** The number or order of agents cannot be determined in advance.
- **Clear Signals:** You can define specific rules or triggers for when a handoff should occur.

### Avoid When:

- **Fixed Workflows:** The order of agents is known and constant.
- **Simple Logic:** Routing is basic and doesn't require dynamic interpretation.
- **Parallel Needs:** Multiple operations must run simultaneously.
- **Risk of Loops:** There is a high risk of "bouncing" between agents or infinite loops.

## 3. Implementation (Microsoft Agent Framework SDK)

Implementation relies on **control workflows** using a switch-case structure to route tasks based on an agent's output.

### Step 1: Data Models and Chat Client

- Set up the chat client (AI service connection).
- Define **Pydantic models** for structured JSON responses from agents.
- Create data classes to pass information between workflow steps.
- Configure agents with a `response_format` for structured output.

### Step 2: Specialized Executor Functions

- **Input Storage Executor:** Saves data to a shared state and forwards it to the classifier.
- **Transformation Executor:** Converts the agent’s JSON response into a typed routing object.
- **Handler Executors:** Separate executors for each outcome with guard conditions to verify processing.

### Step 3: Routing Logic

- Create factory functions for **condition checkers**.
- Use `Case` objects within **switch-case edge groups**.
- **Crucial:** Always include a `Default` case as a fallback for unexpected scenarios.

### Step 4: Assemble the Workflow

- Use `WorkflowBuilder` to connect executors.
- Add the switch-case edge group for dynamic routing.
- Configure the workflow to follow the first matching case.
- Set a **terminal executor** to yield the final result.

## 4. Key Takeaways

Handoff orchestration provides flexibility for evolving tasks. By using the Microsoft Agent Framework SDK, you can create systems that seamlessly transfer control between specialized experts and include human input where necessary to ensure efficient task completion.

## Summary: Use Magentic orchestration

![alt text](images-cert/image30.png)

## 1. Overview

Magentic orchestration is a flexible, general-purpose pattern for **complex, open-ended tasks**. It utilizes a dedicated **Magentic manager** that coordinates a team of specialized agents by building a dynamic execution plan in real-time.

- **Core Philosophy:** Emphasizes planning and documentation as much as the final solution.
- **The Task Ledger:** A dynamic record of goals, subgoals, and plans that is refined as the workflow progresses.
- **Adaptive Nature:** The manager tracks progress and adapts the workflow based on evolving context and agent capabilities.

## 2. When to Use vs. Avoid

### Use When:

- **Open-Ended Problems:** The solution path is unknown and must be discovered.
- **Specialized Collaboration:** Multiple experts are needed to shape the final result.
- **Human Review Required:** You need a documented plan of approach for a human to audit.
- **Direct Interaction:** Agents use tools to interact with external systems.
- **Step-by-Step Planning:** There is high value in having an execution plan before tasks run.

### Avoid When:

- **Fixed Paths:** The process is deterministic or simple enough for lightweight patterns.
- **Speed Over Planning:** The overhead of creating a ledger/plan is impractical for fast execution requirements.
- **Low Complexity:** No need to produce a formal "plan of approach."
- **High Risk of Stalls:** Scenarios where frequent loops occur without a clear resolution path.

## 3. Implementation Steps (Microsoft Agent Framework)

1.  **Define Specialized Agents:** Create agents (e.g., `ChatAgent`) with specific roles and instructions.
2.  **Setup Event Callbacks:** Define an `async` function to handle orchestrator messages, streaming updates, and final results.
3.  **Build with MagenticBuilder:** \* Add participants (agents).
    - Configure the event callback.
    - Set parameters like `max_round_count` and `stall_limits`.
4.  **Configure the Standard Manager:** Use a chat client to enable the manager's planning and progress-tracking capabilities.
5.  **Run with `run_stream`:** Execute the complex task; the manager will dynamically plan and delegate.
6.  **Process and Extract:** Iterate through workflow events and collect the final `WorkflowOutputEvent`.

## 4. Key Parameters for Control

To prevent infinite loops or inefficient planning, the manager uses:

- **Max Round Count:** Limits how many total turns the orchestration can take.
- **Stall Count:** Detects when no progress is being made and triggers a reset or intervention.
- **Reset Count:** Controls how many times the manager can restart the planning process.

# Exercise: Develop a multi-agent solution

https://learn.microsoft.com/en-us/training/modules/orchestrate-semantic-kernel-multi-agent-solution/9-exercise

# Summary: Discover Azure AI Agents with A2A

# Study Guide: Introduction to A2A Protocol

## 1. The Challenge of Multi-Agent Systems

While individual AI agents are powerful, real-world tasks often require collaboration. Manually coordinating interactions between remote or distributed agents is complex and often leads to "siloed" systems that cannot talk to each other.

## 2. What is the A2A Protocol?

The **Agent-to-Agent (A2A)** protocol is a standardized framework designed to solve interoperability challenges. It provides a "universal language" for AI agents.

- **Discovery:** Allows agents to find each other and understand their respective skills.
- **Communication:** Provides a secure and standardized way for agents to exchange messages.
- **Task Delegation:** Enables one agent to hand off work to another seamlessly.
- **Security:** Ensures communication is standardized and secure across distributed environments.

## 3. Workflow Example: Technical Writing

To understand A2A in practice, consider a content creation workflow managed by a **Routing Agent**:

1.  **User Request:** A user asks for blog content.
2.  **Step A (Title Agent):** The routing agent sends the request to a specialized agent that generates catchy headlines.
3.  **Step B (Outline Agent):** The routing agent takes that title and passes it to an agent that builds detailed outlines.
4.  **Final Delivery:** The routing agent returns the completed outline to the user automatically.

## 4. Learning Objectives for Azure AI Agents

When implementing A2A with Azure, the focus is on three main areas:

- **Configuring Routing Agents:** Setting up the "central brain" that directs traffic.
- **Registering Remote Agents:** Adding external or distributed agents to the system.
- **Building Coordinated Workflows:** Creating end-to-end processes where multiple agents contribute to a single goal.

## Summary: Define an A2A agent

![alt text](images-cert/image31.png)

## 1. Core Definition

An **A2A Agent** is an AI entity that follows the Agent-to-Agent protocol to communicate, share context, and invoke capabilities across different vendors and platforms.

### Key Advantages:

- **Enhanced Collaboration:** Connects traditionally disconnected systems.
- **Flexible Model Selection:** Each agent can use a different, optimized LLM (unlike MCP, which often uses one).
- **Integrated Authentication:** Security and identity verification are built directly into the protocol.

## 2. Agent Skills

An **Agent Skill** is a discrete building block that describes a specific function an agent can perform. It is the "What" of the agent's capabilities.

| Element         | Description                                         |
| :-------------- | :-------------------------------------------------- |
| **ID & Name**   | Unique identifier and human-readable name.          |
| **Description** | Detailed explanation of the task the skill handles. |
| **Tags**        | Keywords used for categorization and discovery.     |
| **Examples**    | Sample prompts showing the skill in action.         |
| **I/O Modes**   | Supported formats (e.g., Text, JSON, Image).        |

## 3. The Agent Card

The **Agent Card** is a machine-readable JSON document (the agent's "digital business card") that allows others to discover its skills and how to connect to it.

- **Identity:** Name, description, and version.
- **Endpoint URL:** The digital address where the agent is reached.
- **Capabilities:** Features supported (e.g., streaming, push notifications).
- **Authentication:** Details on the credentials required to talk to the agent.
- **Skill List:** The comprehensive list of available skills defined above.

## 4. The Collaborative Process

1.  **Discovery:** A routing agent or client automatically finds an agent via its Agent Card.
2.  **Routing:** The request is directed to the specific **Skill** best suited for the task.
3.  **Collaboration:** The agent performs the task and returns the response in a supported format.

**Example:** In a writing workflow, a **Routing Agent** retrieves the cards for a "Title Agent" and an "Outline Agent." It passes the user's topic to the Title Agent's skill, then feeds that output into the Outline Agent's skill to generate the final document.

## Summary: Implement an agent executor

![alt text](images-cert/image32.png)

## Overview

The **Agent Executor** is a core A2A component that bridges protocol requirements with business logic. It defines how an agent processes requests and communicates within multi-agent workflows.

## Key Operations

- **Execute**: Accesses `RequestContext` to process tasks and pushes results to an `EventQueue`.
- **Cancel**: Manages the termination of ongoing tasks (optional for simple agents).

## Request Handling Flow

1. **Receive**: Executor gets a request.
2. **Process**: Calls internal logic/helper classes.
3. **Wrap**: Formats the result as an event.
4. **Respond**: Places the event on the `EventQueue` for the routing mechanism to deliver.

**Note:** The Agent Executor ensures standardized communication, enabling seamless integration between clients and other agents.

## Summary: Host an A2A server

## Overview

Hosting transforms an agent into a reachable service over HTTP. This enables the agent to expose its **Agent Card**, process incoming requests via the **Agent Executor**, and manage complex task lifecycles.

## The Three Essential Components

1. **Agent Card**: The "directory" of the agent.
   - Contains skills and metadata.
   - Endpoint: `/.well-known/agent-card.json`.
2. **Request Handler**: The "manager".
   - Routes actions (execute, cancel).
   - Requires a **Task Store** for reliability and state tracking.
3. **Server Application**: The "infrastructure".
   - Uses **Starlette** (Framework) and **Uvicorn** (ASGI Server).

## Implementation Workflow

1. Define Skills + Agent Card.
2. Initialize Request Handler (Executor + Task Store).
3. Set up the Server Application.
4. Run via Uvicorn.

**Note:** Even a "Hello World" agent requires this hosting structure to respond to network-based requests and participate in multi-agent workflows.

## Summary: Connect to your A2A agent

## Client Responsibilities

- **Discovery**: Locating the Agent Card for metadata and endpoints.
- **Communication**: Sending requests and managing transmission.
- **Interpretation**: Decoding direct messages vs. task-based objects.

## Interaction Mechanics

### 1. Establishing Connection

- Requires the **Base URL**.
- Starts by fetching the **Agent Card** to understand what the agent can do.

### 2. Request Patterns

- **Sync (Non-Streaming)**: Standard request-response loop.
- **Async (Streaming)**: Asynchronous partial results; useful for long-running logic.

### 3. Response Varieties

- **Direct**: Immediate output.
- **Task-based**: Represents a lifecycle; allows for tracking and cancellation.

## Key Technical Details

- **Identification**: Use unique IDs for every request.
- **Roles**: Define the sender (e.g., `role: "user"`) in the message payload.
- **Complexity**: Advanced agents may return multiple tasks simultaneously rather than a single message string.

# Exercise: Connect to remote Azure AI Agents with the A2A protocol

https://learn.microsoft.com/en-us/training/modules/discover-agents-with-a2a/6-exercise

# Summary: Build agent-driven workflows using Microsoft Foundry

## What is Microsoft Foundry?

A platform to orchestrate multiple AI agents through a visual interface. It balances automation with **runtime safeguards** to ensure reliability.

## Key Capabilities

- **Low-Code Design**: A visual canvas for defining agent interactions.
- **Decision Logic**: Using agent outputs to move data through specific steps.
- **Triage & Scaling**: Managing high volumes of tasks (like support tickets) by routing them based on complexity.

## Critical Concepts

| Concept               | Definition                                                                |
| :-------------------- | :------------------------------------------------------------------------ |
| **Workflow Nodes**    | The individual building blocks/steps of a process.                        |
| **Human-in-the-Loop** | A pattern where humans intervene when AI confidence is low.               |
| **Power Fx**          | The expression language used to manipulate data and evaluate conditions.  |
| **For-Each Loops**    | A method to process multiple inputs (e.g., a batch of tickets) in one go. |

## Why use Workflows?

Workflows solve the "Scale vs. Safety" dilemma. They allow for high-speed triage while maintaining human control over sensitive or complex decisions.

# Summary: Understand Workflows

## Core Definition

Workflows are **visual and declarative**. You arrange nodes to define an execution path, and the system manages the "state" (memory) and execution automatically.

## Node Functions

| Node Type               | Purpose                              |
| :---------------------- | :----------------------------------- |
| **Agent Nodes**         | Execute AI reasoning or skills.      |
| **Condition Nodes**     | Branch the path based on logic/data. |
| **Communication Nodes** | Interface with the end-user.         |

## Orchestration Patterns

- **Specialization**: Instead of one "do-it-all" agent, workflows link multiple agents with distinct responsibilities.
- **Human Oversight**: Workflows can be configured to stop and wait for a human if the agent is unsure, ensuring safety in real-world applications.
- **Flow Control**: You determine exactly how information moves from the output of one node to the input of the next.

**Key Takeaway**: Workflows turn individual AI "brains" into a functional, reliable business "process."

# Summary: Identify Workflow Patterns

## Pattern Comparison Table

| Pattern               | Logic Type              | Best Use Case                                  |
| :-------------------- | :---------------------- | :--------------------------------------------- |
| **Sequential**        | Linear / Fixed          | Data processing pipelines & validation.        |
| **Human-in-the-Loop** | Oversight-driven        | Approvals, confirmations, and safety checks.   |
| **Group Chat**        | Dynamic / Collaborative | Complex support and multi-agent brainstorming. |

## Key Insights

- **Sequential** is the easiest to reason about and serves as the best starting point for beginners.
- **Human-in-the-Loop** balances the speed of AI with the reliability of human judgment.
- **Group Chat** allows for flexibility; agents can adapt to changing inputs and "talk" to each other to refine a result.

**Decision Tip:** Choose your pattern based on how much "flexibility" vs. "predictability" your specific business process requires.

# Summary: Create workflows in Microsoft Foundry

## Workflow Design Principles

- **Nodes as Building Blocks**: Every action (AI call, logic check, data change) is a node.
- **Manual Persistence**: Remember to **Save Regularly**; there is no auto-save in the designer.
- **Interactivity**: Test workflows via the chat window to trace execution paths live.

## Essential Node Catalog

### 1. The "Brains" (Invoke)

- **Agent Node**: Used for classification, recommendations, and reasoning.
- _Key Tip_: Use structured outputs (JSON) to make data easier for "Flow" nodes to read.

### 2. The "Logic" (Flow)

- **If/Else**: Creating branches based on conditions.
- **For Each**: Processing arrays or lists (e.g., a list of emails).
- **Go To**: Non-linear navigation within the workflow.

### 3. The "Memory" (Data & Variables)

- **Set Variable**: Storing data for later steps.
- **Parse Value**: Cleaning up agent responses into usable formats.
- **Basic Chat**: The interface for human-agent interaction.

## Best Practices

- Start with a **predefined pattern** (like Sequential) to learn the ropes.
- Use **Variables** to pass information from the beginning of a workflow to the end.
- Use the **End node** to clearly define the final output or success state.

# Summary: Add Agents to a Workflow

## The Invoke Agent Node

The primary tool for adding AI reasoning to a workflow.

- **Sources**: Use existing project agents or create new "inline" agents.
- **Inputs**: Receives context from previous steps or user messages.
- **Outputs**: Returns data that can be used by subsequent nodes.

## Key Configuration Options

| Feature               | Purpose                                                        |
| :-------------------- | :------------------------------------------------------------- |
| **Response Format**   | Define a JSON schema for predictable, machine-readable data.   |
| **Action Settings**   | Where you map the agent's response to a specific **Variable**. |
| **Tools & Knowledge** | Grant the agent access to external data or specific functions. |

## Strategic Patterns

- **Modularity**: Build a library of specialized agents (Classifiers, Summarizers, Researchers) and reuse them across different workflows.
- **Structured Reasoning**: Always use structured outputs when the agent's answer needs to trigger an `If/Else` or `For-Each` loop.
- **State Persistence**: Store agent results in variables to ensure the "memory" of the conversation carries through the entire execution path.

**Pro Tip**: If an agent's output will be used to make a logic decision, define a JSON schema to prevent "hallucinated" formatting from breaking your workflow.

# Summary: Apply Power Fx in Workflows

# Study Guide: Apply Power Fx in Workflows

Power Fx is the low-code, Excel-like language that acts as the glue of a workflow. It allows for data manipulation, condition evaluation, and execution control without complex coding.

## Core Concepts

### 1. How Formulas Work

A Power Fx formula is an expression that evaluates to a value. These formulas can reference two types of variables:

- **System variables:** Provide contextual info (e.g., current activity, last message, user info).
- **Local variables:** Store data captured or created during workflow execution for use in later nodes.

### 2. Decision Points and loops

- **Conditions:** Expressions are used in **If/Else** nodes to branch execution (e.g., checking an agent's confidence score to decide if a human is needed).
- **Loops:** **For-each** nodes use Power Fx to iterate over collections, applying actions to each item in a list (e.g., processing multiple support tickets).

---

## Power Fx Formula Reference

| Purpose                  | Formula Example                                     | Notes                                       |
| :----------------------- | :-------------------------------------------------- | :------------------------------------------ |
| **Convert to uppercase** | `Upper(Local.Input)`                                | Transforms a string to all caps             |
| **Convert to lowercase** | `Lower(Local.Input)`                                | Transforms a string to all lowercase        |
| **Get string length**    | `Len(Local.Input)`                                  | Returns the number of characters            |
| **Conditional check**    | `Local.Confidence > 0.8`                            | Returns true/false; used in If/Else nodes   |
| **If/Else logic**        | `If(Local.Confidence > 0.8, "Proceed", "Escalate")` | Returns one of two values                   |
| **Sum simple list**      | `Sum([10, 20, 30])`                                 | Adds up numbers in a simple list            |
| **Sum table column**     | `Sum(Local.ItemList, Amount)`                       | Adds the **Amount** property of each record |
| **Count items**          | `Count(Local.ItemList)`                             | Returns the number of items in a table/list |
| **Check if blank**       | `IsBlank(Local.Input)`                              | Returns true if variable or input is empty  |
| **Check if empty table** | `IsEmpty(Local.ItemList)`                           | Returns true if a table has no records      |
| **Loop over items**      | `ForAll(Local.ItemList, Upper(Name))`               | Applies a formula to each item in a list    |
| **Concatenate text**     | `Concatenate(Local.FirstName, " ", Local.LastName)` | Joins multiple strings into one             |

## Logic Summary for Workflows

- **Dynamic Adaptation:** Using variables allows workflows to change behavior based on previous steps.
- **Maintainability:** This low-code approach keeps complex logic understandable and easy to manage.

```powerfx
// Example of a combined logic check
If(
    Count(Local.ItemList) > 0,
    ForAll(Local.ItemList, Upper(Name)),
    "No items to process"
)
```

**Tip**

1- For more information about the Power Fx language, visit the Power Fx documentation. https://learn.microsoft.com/en-us/power-platform/power-fx/overview

# Summary: Maintain Workflows in Microsoft Foundry

Maintaining and refining workflows ensures they remain reliable, understandable, and adaptable as business needs or AI models change.

## Core Maintenance Features

### 1. Dual Representations

Microsoft Foundry provides two synchronized ways to view and edit workflows:

- **Visual Canvas:** Best for conceptual understanding, tracing execution paths, and collaboration.
- **YAML:** A textual representation used for advanced configuration, version tracking, and source control integration.
- **Synchronization:** Changes made in one view are automatically reflected in the other to ensure consistency.

### 2. Versioning

Foundry automatically creates a new, **immutable version** every time a workflow is saved.

- **Safety Net:** Allows users to review prior versions, compare changes, or roll back if errors occur.
- **Collaboration:** Simplifies tracking who made specific changes and the reasoning behind them.

### 3. Documentation (Notes)

The workflow visualizer allows maintainers to attach notes directly to nodes or sections.

- **Context:** Explains design decisions and clarifies variable usage.
- **Efficiency:** Reduces errors and accelerates updates for future team members.

## Best Practices for Refinement

To improve clarity, reliability, and efficiency, follow these standards:

- **Clean Up:** Regularly review workflows to remove unused or redundant nodes.
- **Consistency:** Ensure structured agent outputs are handled uniformly.
- **Documentation:** Consistently use notes to document logic and decisions.
- **Validation:** Use version history to track changes and validate all updates.

## Summary Table: YAML vs. Visual Canvas

| Feature           | Visual Canvas                     | YAML Representation                |
| :---------------- | :-------------------------------- | :--------------------------------- |
| **Primary Use**   | Conceptual mapping & flow tracing | Advanced config & version tracking |
| **Collaboration** | Ideal for team walk-throughs      | Integration with source control    |
| **Editing**       | Drag-and-drop interface           | Text-based editing                 |

### YAML Configuration Example

```yaml
# Example snippet of a workflow's textual representation
name: "Customer Support Logic"
version: 1.2.0
nodes:
  - id: "check_confidence"
    type: "condition"
    inputs:
      formula: "Local.Confidence > 0.8"
```

# Summary: Use workflows in code

## Overview

Workflows designed in the **Microsoft Foundry visual designer** can be integrated into applications (web apps, APIs, backend services) using the **Azure AI Projects SDK**. Workflows are defined by YAML but are invoked programmatically by name.

## 1. Invoking a Workflow

To run a workflow, you must first establish a connection to the project using the `AIProjectClient`. This client manages authentication and provides access to an OpenAI-compatible API.

### Implementation Steps:

- Reference the workflow by its specific name.
- Create a conversation context.
- Invoke the workflow by passing the `conversation.id` and an `agent_reference`.

### Python Example:

```python
# Reference a workflow created in the Foundry portal
workflow_name = "triage-workflow"

# Create a conversation context for the workflow
conversation = openai_client.conversations.create()

# Execute the workflow, passing input to drive the workflow logic
stream = openai_client.responses.create(
    conversation=conversation.id,
    extra_body={"agent": {"name": workflow_name, "type": "agent_reference"}},
    input="Users can't reset their password from the mobile app.",
    stream=True,
)
```

## 2. Input Parameters

The `input` parameter drives the workflow logic. Depending on the design, this can be:

- A user question for analysis.
- A support ticket description for routing/classification.
- A data payload for processing.
- An empty string to simply trigger the start.

## 3. Processing Workflow Events

When **streaming** is enabled, the application receives real-time events. This allows for progress tracking and capturing agent outputs.

### Python Example:

```python
for event in stream:
    if event.type == "response.completed":
        print("Workflow completed:")
        for message in event.response.output:
            if message.content:
                for content_item in message.content:
                    if content_item.type == 'output_text':
                        print(content_item.text)
    if (event.type == "response.output_item.done") and event.item.type == ItemType.WORKFLOW_ACTION:
        print(f"Action '{event.item.action_id}' completed with status: {event.item.status}")

```

### Common Event Types:

| Event Type                  | Description                                              |
| --------------------------- | -------------------------------------------------------- |
| `response.completed`        | The workflow finished and returned a final response.     |
| `response.output_item.done` | An individual output (like a workflow action) completed. |

## 4. Interaction Patterns

- **Streaming:** Used for real-time monitoring and progress updates.
- **Non-Streaming:** Waiting for the entire workflow to finish before processing.
- **Human-in-the-loop:** Handling pauses where the workflow waits for user input; the application resumes execution by sending additional messages to the conversation.

## 5. Benefits of Integration

| Scenario               | Benefit                                       |
| ---------------------- | --------------------------------------------- |
| **Web applications**   | Embed AI workflows in user-facing apps.       |
| **APIs/Microservices** | Expose workflows via REST endpoints.          |
| **Batch processing**   | Programmatic invocation for bulk operations.  |
| **Testing/Validation** | Automated testing via CI/CD pipelines.        |
| **Custom interfaces**  | Build specialized UIs for specific workflows. |

# Exercise - Create an Agent-driven Workflow

https://learn.microsoft.com/en-us/training/modules/build-agent-workflows-microsoft-foundry/9-exercise

# Unit: Build knowledge-enhanced AI agents with Foundry IQ

# Summary: Understanding RAG for agents

## Overview

Enterprise environments require high accuracy and reliability. **Retrieval Augmented Generation (RAG)** is the architectural solution used to overcome the limitations of simple AI agents by connecting them to real-time organizational knowledge.

---

## 1. Limitations of Simple AI Agents

Simple agents often fail in business settings because they rely solely on static training data.

| Limitation               | Impact                    | Example                                          |
| :----------------------- | :------------------------ | :----------------------------------------------- |
| **Knowledge Cutoff**     | No access to recent info  | Inability to explain a policy updated yesterday. |
| **Private Data Access**  | Generic responses only    | Missing internal product specs or procedures.    |
| **Lack of Context**      | Irrelevant advice         | Ignoring specific company security protocols.    |
| **Fabricated Responses** | Compliance/Security risks | Providing confident but "hallucinated" info.     |
| **Scalability Issues**   | Engineering inefficiency  | Every team rebuilding RAG infra from scratch.    |

---

## 2. The RAG Process

RAG transforms agents by moving from static data to **dynamic knowledge retrieval** through three coordinated steps:

1.  **Retrieve**: The system searches internal knowledge bases for content relevant to the user's query.
2.  **Augment**: The system combines the retrieved factual content with the user's original question.
3.  **Generate**: The agent creates a final response using a blend of its core training and the specific retrieved information.

## 3. Critical Advantages for Enterprise

By implementing RAG, organizations gain three specific benefits:

- **Real-time Updates**: Agents stay current with new policies without needing to be retrained.
- **Source Transparency**: Users can see which documents informed the response, enabling verification and trust.
- **Factual Grounding**: Responses are anchored in actual organizational data, eliminating fabrications and ensuring compliance.

---

## 4. Transition to Microsoft Foundry IQ

Building custom RAG infrastructure requires significant technical expertise. **Microsoft Foundry IQ** serves as a "ready-made" knowledge platform designed to eliminate the complexity of manual RAG implementation.

# Summary: Explore Foundry IQ

## 1. What is Foundry IQ?

Foundry IQ is a **managed knowledge platform** built on Azure AI Search. It acts as a shared service that allows multiple AI agents to access organizational data without rebuilding RAG (Retrieval Augmented Generation) infrastructure for every new agent.

## 2. Knowledge Bases (KB)

Foundry IQ organizes information by **business domain** (e.g., "HR Policies") rather than technical storage location (e.g., "SharePoint").

- **Unified Source:** A single KB can aggregate data from SharePoint, Azure Blob Storage, and OneLake.
- **Agent Perspective:** To the agent, the KB appears as one unified, searchable source.
- **Management:** You connect data sources to KBs once, and any agent can be linked to them.

## 3. Data Source Integration & Processing

Foundry IQ automates the technical "heavy lifting" of RAG:

1.  **Discovery:** Scans storage for documents.
2.  **Processing:** Automatically handles **chunking** and **embedding** for semantic search.
3.  **Indexing:** Makes content searchable.
4.  **Monitoring:** Triggers automatic reindexing when documents change.

## 4. Built-in Retrieval Intelligence

The platform includes automated logic to optimize how information is found:

- **Query Analysis:** Understands the intent of the agent's question.
- **Strategy Selection:** Switches between keyword search (factual) and semantic search (complex) as needed.
- **Relevance Ranking:** Scores results so the most important info surfaces first.
- **Citations:** Provides source documents so agents can verify information.

## 5. Connecting Agents to Knowledge (Code)

Foundry IQ uses the **Model Context Protocol (MCP)** to connect agents to knowledge bases as a "tool."

### Python Example:

```python
from azure.ai.projects import AIProjectClient
from azure.ai.projects.models import PromptAgentDefinition, MCPTool

project_client = AIProjectClient(endpoint=search_endpoint, credential=credential)

# Connect to the product documentation knowledge base via MCP
knowledge_tool = MCPTool(
    server_label="product-docs",
    server_url=f"{search_endpoint}/knowledgebases/product-documentation/mcp"
)

# Create an agent and grant it access to the KB via the 'tools' parameter
agent = project_client.agents.create_version(
    agent_name="product-support-agent",
    definition=PromptAgentDefinition(
        model="gpt-4o-mini",
        instructions="Answer product questions using the knowledge base. Always cite your sources.",
        tools=[knowledge_tool]
    )
)
```

## 6. The Shared Knowledge Advantage

Scalability: One KB can serve multiple agents (e.g., a "Product Docs" KB can serve both a Support Agent and a Developer Agent).
Consistency: Updating a single data source improves the responses of every connected agent simultaneously.
Standardization: Uses MCP for secure, standardized access to external tools and data.

# Summary: Configure data sources for knowledge bases

## 1. Selecting the Right Data Source

Foundry IQ supports six primary data source types. The choice depends on **data location** and **access requirements** (Real-time vs. Indexed).

| Data Source               | Access Type | Best For...                                           |
| :------------------------ | :---------- | :---------------------------------------------------- |
| **Azure AI Search Index** | Indexed     | Pre-existing enterprise search with custom pipelines. |
| **Azure Blob Storage**    | Direct      | Standard document files (PDF, Word, MD) in Azure.     |
| **Web (Bing)**            | Real-time   | Grounding responses in current, public news/events.   |
| **SharePoint (Remote)**   | Real-time   | Live content with M365 governance; no maintenance.    |
| **SharePoint (Indexed)**  | Indexed     | Faster responses and advanced/custom search needs.    |
| **OneLake**               | Direct      | Unstructured data from Microsoft Fabric lakehouses.   |

## 2. Deep Dive: Key Source Characteristics

### A. Azure AI Search Index

- **Best Use:** When you have already invested in custom search indexes.
- **Key Benefits:**
  - **Semantic Ranking:** Finds context based on meaning, not just keywords.
  - **Custom Scoring:** Prioritizes results based on business logic.
  - **Faceted Navigation:** Allows filtering by categories.

### B. Azure Blob Storage

- **Supported Formats:** PDF, .docx, .txt, .md, .html.
- **Advantage:** Direct path from files to KB without manually building an index.
- **Organization:** Blobs can be organized into containers by topic/access level.

### C. Web (Grounding via Bing)

- **Purpose:** Provides news, pricing, and external info not found in internal KBs.
- **Risk:** Less control over specific sources; accuracy is dependent on Bing.
- **Hybrid Tip:** Can be combined with internal sources as a fallback.

### D. SharePoint: Remote vs. Indexed

| Feature            | SharePoint **Remote**         | SharePoint **Indexed**                   |
| :----------------- | :---------------------------- | :--------------------------------------- |
| **Access Method**  | Real-time queries             | Preprocessed index                       |
| **Data Freshness** | Always current                | Depends on indexing schedule             |
| **Maintenance**    | None                          | Requires index updates                   |
| **Search Power**   | Standard                      | Advanced (Custom analyzers/AI pipelines) |
| **Speed**          | Variable (SharePoint latency) | Fast (Pre-indexed)                       |

### E. Microsoft OneLake

- **Integration:** Connects to Microsoft Fabric data lakehouses.
- **Common Use Cases:** Referencing business intelligence reports, analytical findings, and research outputs.

---

## 3. Decision Guide Summary

To choose a source, follow this logic:

- **Need real-time web info?** Choose **Web**.
- **Data in SharePoint but need it "always current"?** Choose **SharePoint Remote**.
- **Data in SharePoint but need "fast/advanced search"?** Choose **SharePoint Indexed**.
- **Already have an Azure Search setup?** Choose **Azure AI Search Index**.
- **Using Microsoft Fabric?** Choose **OneLake**.

**Note:** You can **combine** multiple sources (e.g., SharePoint + Web) in a single knowledge base to ensure the agent is both an internal expert and aware of current events.

# Summary: Configure retrieval with Foundry IQ

This section moves from "how to store data" to "how to make an agent actually use it." The core challenge is ensuring that the agent doesn't just "know" the information, but consistently **retrieves** it, **cites** it, and **stays grounded** in it.

---

### **Summary: Configure Retrieval with Foundry IQ**

# Study Guide: Configuring Retrieval & Instructions

## 1. The Retrieval Behavior Problem

Without specific configuration, agents may produce inconsistent results. The goal is to move from generic AI knowledge to **grounded, verifiable enterprise knowledge**.

| Behavior                          | Result                                 | Verdict            |
| :-------------------------------- | :------------------------------------- | :----------------- |
| **Training Data Only**            | Generic, likely outdated answers.      | ❌ Unacceptable    |
| **Search without Citation**       | Correct info but no accountability.    | ❌ Unacceptable    |
| **Search + Citation + Grounding** | Verifiable info anchored in your data. | ✅ Target Behavior |

---

## 2. Writing Effective Retrieval Instructions

Instructions act as the "contract" for how an agent uses the knowledge base. Vague instructions (e.g., "Use the KB") lead to inconsistent tool calls.

### Critical Components of Instructions:

1.  **When to retrieve:** Explicitly command the agent to search _every time_.
2.  **How to cite:** Define the exact citation format (e.g., `【doc_id:search_id†source_name】`).
3.  **Fallback logic:** Tell the agent exactly what to say if the answer is missing (avoid "guessing").

### Python Example: Implementing Advanced Instructions

```python
# Define strict retrieval rules
retrieval_instructions = """You are a helpful HR assistant.
CRITICAL RULES:
- ALWAYS search the knowledge base before answering.
- NEVER answer from your own training data.
- FORMAT citations exactly as: 【doc_id:search_id†source_name】
- IF NOT FOUND: Say "I don't have that information. Contact hr@company.com"
"""

agent = project_client.agents.create_version(
    agent_name="hr-assistant",
    definition=PromptAgentDefinition(
        model="gpt-4o-mini",
        instructions=retrieval_instructions,
        tools=[knowledge_tool]
    )
)

```

## 3. Systematic Testing

Testing ensures the instructions are actually being followed across different query types.

| Query Type       | Goal                      | Expected Result                         |
| ---------------- | ------------------------- | --------------------------------------- |
| **Factual**      | Test direct retrieval.    | Precise answer + citation.              |
| **Synthesis**    | Test multi-doc retrieval. | Combined answer + multiple citations.   |
| **Out-of-Scope** | Test fallback logic.      | Triggered the "I don't know" phrase.    |
| **Ambiguous**    | Test reasoning.           | Clarifying question or relevant search. |

---

## 4. Specialized Retrieval Strategies

Tailor your instructions based on the agent's specific role:

- **Customer Support:** High accuracy, zero guessing, "connect to human" fallback.
- **Research Assistant:** Broad synthesis, suggests related topics, indicates confidence levels.
- **Compliance Expert:** Strictly factual, references specific policy sections and effective dates.

---

## 5. Production Monitoring

Retrieval quality is an iterative process. Once in production, track:

- **Citation Frequency:** Are citations appearing as requested?
- **Fallback Rate:** Are there gaps in your documentation?
- **Retrieval Accuracy:** Is the system surfacing the _right_ documents for the user's intent?

# Exercise: Integrate an AI agent with Foundry IQ

https://learn.microsoft.com/en-us/training/modules/introduction-foundry-iq/7-exercise
