[Unlock the Power of Messaging Patterns](https://github.com/TaleLearnCode/UnlockThePowerOfMessagingPatterns) \ [Code Mash 2025](https://github.com/TaleLearnCode/UnlockThePowerOfMessagingPatterns-CodeMash) \

# Prerequisites

Welcome to the **Unlock the Power of Messaging Patterns** hands-on workshop. The primary purpose of this document is to ensure that all participants are prepared with the necessary resources before the workshop begins. By following these prerequisite instructions, you will be ready to dive straight into the hands-on activities without any setup delays.

## Observers Are Welcome!

It's understood that some participants might prefer to watch the hands-on labs and learn through observation. Whether due to technical constraints or personal preference, if you choose not to set up the prerequisites for the workshop, you are still very welcome to join us. The messaging patterns covered are not vendor-specific, meaning you can apply these concepts to other platforms and environments. By following along with the live demonstrations and discussions, you will gain valuable insights and knowledge that are applicable beyond Azure.

## Overview

This workshop will explore key messaging patterns and how they can be implemented using Azure services. Messaging patterns are essential for building scalable and resilient applications that can handle asynchronous communication between different components. Some of the patterns that will be covered include:

- **Message Queuing**: Decoupling send and receive components to enhance reliability and scalability.
- **Publish/Subscribe**: Distributing messages to multiple subscribers for real-time updates.
- **Event Streaming**: Processing and reacting to streams of events in real-time.

Implementation of these patterns will be demonstrated using the following Azure services:

- **Azure Service Bus**: Perfect for implementing messaging queueing and publish-subscribe patterns with advanced features like dead-lettering and message sessions.
- **Azure Event Hubs**: Ideal for event streaming scenarios, enabling high-throughput data ingestion and processing.
- **Azure Storage Queues**: A straightforward, cost-effective option for simple message queueing needs.

By the end of the workshop, you will have hands-on experience with each of these services and understand how to apply messaging patterns to build robust, distributed systems.

## Emulators and Local Development Tools

To make it easier for all participants, most of the lab exercises will be done using emulators and local development tools. This approach ensures that you do not necessarily need an Azure account to participate in the workshop. The primary focus will be on understanding how the messaging patterns work and how they can be applied to build powerful event-driven architectures. While participants will gain first-hand knowledge of using Azure services to implement these patterns, it's the concepts and patterns that are most important.

## Why Set Up Resources in Advance?

Setting up the required resources before the workshop allows us to maximize our time on the core content and hands-on activities. This approach ensures that:

- Everyone starts on even footing, regardless of their familiarity with the tools being used.
- Potential technical issues and delays are avoided during the workshop.
- Participants can focus on learning and experimentation rather than setup.

## Cloning the Workshop Repository

To access the workshop materials, you will need to clone the workshop repository to your local machine. Follow these steps:

1. Ensure you have [Git](https://git-scm.com/) installed on your machine.

2. Open a terminal or PowerShell window.

3. Navigate to the directory where you want to clone the repository.

4. Run the following command:

   ```bash
   git clone https://github.com/TaleLearnCode/UnlockThePowerOfMessagingPatterns-CodeMash.git
   ```

5. Navigate into cloned repository directory:

   ```bash
   cd your-repository-name
   ```

## Tools Installation

Before the workshop, please ensure you have the following tools installed and set up on your local environment:

- **Visual Studio Code**: [Download Visual Studio Code](https://code.visualstudio.com/)
  
  - **C# Extension**: [Install C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
  - **Azure Functions**: [Install Azure Functions Extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
  
- **PowerShell**: [Download PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell)

- **Azurite** (Azure Storage Emulator): [Download Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite)

- **Docker**: [Windows Install](https://docs.docker.com/desktop/setup/install/windows-install/) | [Mac Install](https://docs.docker.com/desktop/setup/install/mac-install/) | [Linux Install](https://docs.docker.com/desktop/setup/install/linux/)

- **WSL Enablement** (Only for Windows):

  - [Install Windows Subsystem for Linux (WSL)](https://learn.microsoft.com/en-us/windows/wsl/install)
  - [Configure Docker to use WSL](https://docs.docker.com/desktop/features/wsl/#:~:text=Turn%20on%20Docker%20Desktop%20WSL%202%201%20Download,engine%20..%20...%206%20Select%20Apply%20%26%20Restart.)

- **Azure Service Bus Emulator**: [Azure Service Bus Emulator Installer](https://github.com/Azure/azure-service-bus-emulator-installer)

> [!IMPORTANT]
> The emulator uses a specified JSON file to configure the Service Bus queues, topics, and subscriptions. Replace the `ServiceBus-Emulator\Config\Config.json` file with the [configuration file built for the workshop](config.json). This Config.json is built with all of the queues, topics, and subscriptions that will be used during the workshop.

> [!CAUTION]
> As of December 28, 2024, the labs are still in development and the Config.json file will change before the event.

- **Azure Functions Core Tools**: [Install Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)

## Conclusion

You are all set! With your local environment configured and the necessary tools installed, you are ready to fully engage in the **Unlock the Power of Messaging Patterns** workshop. Get ready for an informative and hands-on experience that will enhance your skills in building scalable, event-driven architectures.

See you at the workshop!