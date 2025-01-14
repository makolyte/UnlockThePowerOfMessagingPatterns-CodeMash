[Unlock the Power of Messaging Patterns](https://github.com/TaleLearnCode/UnlockThePowerOfMessagingPatterns) \ [CodeMash 2025](..\..\README.md) \ [Labs](..\README.md) \

# Publish/Subscribe Messaging

Publish/subscribe (pub/sub) messaging is a messaging pattern used in distributed systems to allow for flexible and scalable communication. Unlike point-to-point messaging, pub/sub messaging can involve multiple receivers (subscribers) who express interest in specific types of messages from a sender (publisher)

#### Key Components

- **Topics**: These act as channels or subjects that messages are published to. Subscribers subscribe to specific topics to receive relevant messages.
- **Publishers**: The components that create and send messages to topics.
- **Subscribers**: The components that receive and process messages from topics they are subscribed to.

#### How it Works

- **Messaging Publishing**: A publisher sends a message to a topic.
- **Messaging Broadcasting**: The messaging system delivers the message to all subscribers of the topic.
- **Message Receiving**: Subscribers receive the message from the topic and process it.

#### Benefits

- **Decoupling**: Publishers and subscribers are completely decoupled, meaning they do not need to know about each other's existence.
- **Scalability**: Multiple subscribers can independently process messages from a single topic, enabling scalable distribution of information.
- **Flexibility**: New subscribers can be added without affecting existing publishers or other subscribers.

#### Use Cases

- **Event Notification Systems**: Notifying multiple services of events such as change sin data or user actions.
- **Broadcasting Messaging**: Sending updates to multiple clients, such as stock price updates or live sports scores.
- **Microservices Communication**: Enabling communication between different microservices in an application.

#### Example: Azure Service Bus

In Azure Service Bus, pub/sub messaging is implemented using topics and subscriptions. A topic acts as a central hub where messages are sent, and subscriptions are used by subscribers to receive messages from that topic.

###### Steps:

1. **Create a Topic**: In Azure Service Bus, you create a topic where messages are published.
2. **Create Subscriptions**: Subscribers create subscriptions to the topic to receive messages.
3. **Send Messages**: Publishers send messages to the topic.
4. **Receive Messages**: Subscribers receive messages from their subscriptions.

#### Conclusion

Publish/subscribe messaging is an effective way to enable scalable and decoupled communication in distributed systems. By using topics and subscriptions, it allows for flexible message distribution, making it suitable for a wide range of use cases, from event notification systems to microservices communications. This pattern is vendor-agnostic and can be implemented using various messaging platforms, include Azure Service Bus, Apache Kafka, and RabbitMQ.

## Hands-On Exercise

> [!IMPORTANT]
>
> Be sure to have completed the [Prerequisites.md](..\prerequisites.md) before continuing on with this hands-on exercise.

### Step 1: Setup the Lab Folder

1. Open Visual Studio Code.
2. Create a new folder for your project named `publish-subscribe-messaging`.
3. Open this folder in Visual Studio Code.

### Step 2: Create the Publisher console app (producer)

1. Open the integrated terminal in Visual Studio Code and initialize a new .NET console project:

   ```sh
   dotnet new console -n PublisherApp
   cd PublisherApp
   ```

2. Install the necessary NuGet packages:

   ```sh
   dotnet add package Azure.Messaging.ServiceBus
   dotnet add package System.Text.Json
   ```

   > [!NOTE] 
   >
   > The `Azure.Messaging.ServiceBus` NuGet package (version 7.18.2) includes the `System.Text.Json` package version 6.0.6 which has known vulnerabilities. So we are specially installing the `System.Text.Json` package so we can get a non-vulnerable version.

3. Replace the content of `Program.cs` with the following code:

   ```c#
   using Azure.Messaging.ServiceBus;
   
   try
   {
   
   	string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
   	string topicName = "publish-subscribe";
   	string messageBody = "Hello, CodeMash!";
   
   	// Create a ServiceBusClient
   	ServiceBusClient client = new(connectionString);
   
   	// Create a ServiceBusSender for the topic
   	ServiceBusSender sender = client.CreateSender(topicName);
   
   	// Create a message to send
   	ServiceBusMessage message = new(messageBody);
   
   	// Send the message
   	await sender.SendMessageAsync(message);
   	Console.WriteLine("==== Message Sent ====");
   	Console.WriteLine(messageBody);
   
   	// Dispose of the client and sender
   	await client.DisposeAsync();
   	await sender.DisposeAsync();
   
   }
   catch (Exception ex)
   {
   	PrintException(ex);
   }
   
   static void PrintException(Exception ex)
   {
   	Console.ForegroundColor = ConsoleColor.Red;
   	Console.WriteLine("An exception occurred:");
   	Console.ResetColor();
   	Console.WriteLine(ex.Message);
   }
   ```

4. Run the application:

   ```sh
   dotnet run
   ```

   This application will perform the following actions:

   - Create a `ServiceBusClient` which is the top-level client that allows us to interact with the Service Bus.

   - Create a `ServiceBusSender` which allows us to send messages to the specific Service Bus entity (topic).

   - Create the `ServiceBusMessage` which represents the data we are sending to the Service Bus topic.

   - Send the message to the Service Bus topic.

   - Clean up.


   You should see a result similar to:

   ```sh
==== Message Sent ====
Hello, CodeMash!
   ```

### Step 4: Create the Subscriber console app (consumer)

1. Open the integrated terminal in Visual Studio Code and initialize a new .NET console project:

   ```sh
   cd ..
   dotnet new console -n SubscriberApp
   cd SubscriberApp
   ```

2. Install the necessary NuGet packages:

   ```sh
   dotnet add package Azure.Messaging.ServiceBus
   dotnet add package System.Text.Json
   ```

3. Replace the content of `Program.cs` with the following code:

   ```c#
   using Azure.Messaging.ServiceBus;
   
   string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
   string topicName = "publish-subscribe";
   string subscriptionName = "subscription.1";
   
   // Create a ServiceBusClient that owns the connection and can be used to create the receiver
   ServiceBusClient client = new(connectionString);
   
   // Create a ServiceBusProcessor client for processing messages
   ServiceBusProcessor processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());
   
   try
   {
   	// Add handler to process messages
   	processor.ProcessMessageAsync += MessageHandler;
   
   	// Add handler to process any errors
   	processor.ProcessErrorAsync += ErrorHandler;
   
   	// Start processing 
   	await processor.StartProcessingAsync();
   
   	Console.WriteLine("Press any key to end processing incoming messages...");
   	Console.ReadKey();
   
   	// stop processing 
   	Console.WriteLine("\nStopping the receiver...");
   	await processor.StopProcessingAsync();
   	Console.WriteLine("Stopped receiving messages");
   }
   finally
   {
   	// Calling DisposeAsync on client types is required to ensure that network
   	// resources and other unmanaged objects are properly cleaned up.
   	await processor.DisposeAsync();
   	await client.DisposeAsync();
   }
   
   // Handle received messages
   async Task MessageHandler(ProcessMessageEventArgs args)
   {
   
   	// Display the message details
   	Console.WriteLine();
   	Console.WriteLine("==== Message Received ====");
   	Console.WriteLine($"Id: {args.Message.MessageId}");
   	Console.WriteLine($"Enqueued Time: {args.Message.EnqueuedTime}");
   	Console.WriteLine($"Expiration Time: {args.Message.ExpiresAt}");
   	Console.WriteLine($"Body: {args.Message.Body}");
   
   	// Complete the message. The messages will be deleted from the subscription. 
   	await args.CompleteMessageAsync(args.Message);
   
   	Console.WriteLine("Press any key to end processing incoming messages...");
   }
   
   // Handle any errors when receiving messages
   Task ErrorHandler(ProcessErrorEventArgs args)
   {
   	Console.WriteLine(args.Exception.ToString());
   	return Task.CompletedTask;
   }
   ```

4. Run the application:

   ```sh
   dotnet run
   ```

   This application will perform the following actions:

   - Creates a `ServiceBusClient` that owns the connection and can be used to create the receiver.

   - Creates a `ServiceBusProcessor` client that allows the application to process messaging coming into the specified subscription.

     > [!TIP] 
     >
     > The Service Bus client types are safe to cache and use as a singleton for the lifetime of the application, which is the best practice when messages are being published or read regularly.

   - Adding a handler to process messages.

     - When messages come into the subscription, the `MessageHandler` method processes the message and displays its details.
     - Near the end of the `MessageHandler` method, the application "completes" the message. This indicates to Service Bus that the consumer has finished processing the message and it will be deleted from the subscription. Other subscription on the same topic will still have the message until their consumer "completes" them.

   - Adds a handler to process any errors.

     - When there is an error with the received message, the `ErrorHandler` will print the details of the exception.

   - Starts processing messages as they are added to the subscription. This will continue until you press a key to stop the application.

   - Once message processing is stopped, the application will dispose the `ServiceBusClient` and `ServiceBusProcessor` to ensure that network resources an other unmanaged objects are properly cleaned up.

   You should see a result similar to:

   ```sh
   Press any key to end processing incoming messages...
   
   ==== Message Received ====
   Id: 6d2518ee1cdf471aa5533fa2a4f5e373
   Enqueued Time: 12/28/2024 6:03:58 PM +00:00
   Expiration Time: 12/28/2024 7:03:58 PM +00:00
   Body: Hello, CodeMash!
   
   Press any key to end processing incoming messages...
   ```

   ### Conclusion

   This exercise demonstrates how to use Azure Service Bus for publish/subscribe messaging. By following these steps, you will gain hands-on experience with the pub/sub messaging pattern in order to reinforce your understanding of how to build scalable and decoupled systems.