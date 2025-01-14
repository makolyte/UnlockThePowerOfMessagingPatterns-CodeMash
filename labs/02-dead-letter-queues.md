[Unlock the Power of Messaging Patterns](https://github.com/TaleLearnCode/UnlockThePowerOfMessagingPatterns) \ [CodeMash 2025](..\..\README.md) \ [Labs](..\README.md) \

# Dead Letter Queues (DLQs)

A dead letter queue is a specialized queue used in messaging systems to store messages that cannot be processed successfully by the primary queue. These messages are often referred to as "dead letters." DLQs help ensure that problematic messages do not block the processing of other messages and provide a way to analyze and handle these unprocessed messages.

#### Key Components

- **Primary Queue**: The main queue where messages are initially sent and processed
- **Dead Letter Queue**: A separate queue that stores messages that cannot be processed by the primary queue.

#### How it Works

1. **Message Sending**: Producers send messages to the primary queue.
2. **Message Processing**: Consumers attempt to process messages from the primary queue.
3. **Failed Processing**: If a message cannot be processed after a predefined number of attempts or due to specific error conditions, it is moved to the dead letter queue.
4. **Dead Letter Handling**: Administrators or automatic processes review and handle messages in the DLQ to diagnose and resolve issues.

### Benefits

- **Error Isolation**: Ensures that problematic messages do not block the processing of other messages in the primary queue.
- **Troubleshooting**: Provides a way to diagnose and analyze why certain messages could not be processed.
- **Retry Logic**: Allows for the implementation of custom retry logic or manual intervention for messages in the DLQ.

### Use Cases

- **Message Validation Failures**: Messages that fail validation checks or contain invalid data.
- **Processing Errors**: Messages that cause errors during processing, such as database constraints or application exceptions.
- **Expired Messages**: Messages that exceed their time-to-live (TTL) without being processed.

### Example: Implementing Dead Letter Queues with Azure Service Bus

Azure Service Bus supports dead letter queues natively. Each queue or subscription in Azure Service Bus has an associated dead letter queue that stores messages that cannot be processed.

###### Steps:

1. **Enable DLQ**: No additional configuration is needed to enable DLQs, as they are supported by default in Azure Service Bus.
2. **Send Messages**: Producers send messages to the primary queue.
3. **Process Messages**: Consumers attempt to process messages. If a message cannot be processed, it is moved to the DLQ.
4. **Retrieve Dead Letters**: Administrators or automated processes can retrieve and handle messages from the DLQ.

### Conclusion

Dead letter queues are an essential feature in messaging systems to ensure reliable message processing and error handling. By isolating unprocessable messages, DLQs help maintain the overall health and efficiency of the system, allowing for effective troubleshooting and resolution of issues.

## Hands-On Exercise

### Step 1: Setup the Lab Folder

1. Open Visual Studio Code.
2. Create a new folder for your project named `dead-letter-queues`.
3. Open this folder in Visual Studio Code.

### Step 2: Create the Producer application in Visual Studio Code

1. Open the integrated terminal in Visual Studio Code and initialize a new .NET console project:

   ```sh
   dotnet new console -n Producer
   cd Producer
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
   
   string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
   string queueName = "deadletter";
   
   ServiceBusClient client = new(connectionString);
   ServiceBusSender sender = client.CreateSender(queueName);
   
   for (int i = 0; i < 10; i++)
   {
   	string messageContent = i % 2 == 0 ? $"Task {i}" : $"Task {i} fail";
   	ServiceBusMessage message = new(messageContent);
   	await sender.SendMessageAsync(message);
   	Console.WriteLine($"Sent message: {messageContent}");
   }
   
   await client.DisposeAsync();
   ```

4. Run the application:

   ```sh
   dotnet run
   ```

   This application will perform the following actions:

   - Create a `ServiceBusClient` which is the top-level client that allows us to interact with the Service Bus.
   - Create a `ServiceBusSender` which allows us to send messages to the specific Service Bus entity (queue).
   - Create a `ServiceBusMessage` which represents the data we are sending to the Service Bus topic and send that message. This is done ten times.
   - Clean up.

   You should see a result similar to:

   ```sh
   Sent message: Task 0
   Sent message: Task 1 fail
   Sent message: Task 2
   Sent message: Task 3 fail
   Sent message: Task 4
   Sent message: Task 5 fail
   Sent message: Task 6
   Sent message: Task 7 fail
   Sent message: Task 8
   Sent message: Task 9 fail
   ```

### Step 3: Set up the Consumer and Dead Letter Handling in Visual Studio Code

1. Open the integrated terminal in Visual Studio Code and initialize a new .NET console project:

   ```sh
   cd ..
   dotnet new console -n Consumer
   cd Consumer
   ```

2. Install the necessary NuGet packages:

   ```sh
   dotnet add package Azure.Messaging.ServiceBus
   dotnet add package System.Text.Json
   ```

   > [!NOTE]
   >
   > The `Azure.Messaging.ServiceBus` NuGet package (version 7.18.2) includes the `System.Text.Json` package version 6.0.6 which has known vulnerabilities. So we are specially installing the `System.Text.Json` package so we can get a non-vulnerable version.

3. Replace the content of `Program.cs` with the following code to initialize the variables and Service Bus Client and Processor:

   ```c#
   using Azure.Messaging.ServiceBus;
   
   string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
   string queueName = "deadletter";
   string deadLetterQueueName = "deadletter/$DeadLetterQueue";
   
   ServiceBusClient client = new(connectionString);
   ServiceBusProcessor processor = client.CreateProcessor(queueName);
   ```

   - **Connection String**: Stores the connection string for accessing the Azure Service Bus. Here we have hard coded this to the connection for the Service Bus Emulator. In a real-world application, this value should not be hardcoded.
   - **Queue Names**: Defines the names of the primary queue and the dead letter queue.
   - **ServiceBusClient**: Initializes a new instance of the `ServiceBusClient` to intact with the Azure Service Bus.
   - **ServiceBusProcessor**: Create a processor for the primary queue to process incoming messages.

4. Add the following code to `Program.cs` for the message processing logic

   ```c#
   processor.ProcessMessageAsync += async args =>
   {
   	ServiceBusReceivedMessage message = args.Message;
   	Console.WriteLine($"Received message: {message.Body}");
   
   	try
   	{
   		// Simulate message processing
   		if (message.Body.ToString().Contains("fail"))
   		{
   			throw new Exception("Simulated processing error");
   		}
   		await args.CompleteMessageAsync(message);
   		Console.WriteLine($"Completed message: {message.Body}");
   	}
   	catch (Exception ex)
   	{
   		Console.WriteLine($"Error processing message: {ex.Message}");
   		await args.DeadLetterMessageAsync(message);
   	}
   };
   ```

   - **ProcessMesageAsync**: Defines an asynchronous event handler for processing incoming messages.
   - **Message Handling**: Retrieves the message body and stimulates processing. If the message contains the keyword "fail", it throws an exception to simulate a processing error.
   - **CompleteMessageAsync**: Marks the message as successfully processed.
   - **DeadLetterMessageAsync**: If an exception occurs, the message is moved to the dead letter queue.

5. Add the following to the `Program.cs` for error handling logic:

   ```C#
   processor.ProcessErrorAsync += args =>
   {
   	Console.WriteLine($"Error processing message: {args.Exception.Message}");
   	return Task.CompletedTask;
   };
   ```

   - **ProcessErrorAsync**: Defines an asynchronous event handler for processing errors. Logs any errors that occur during message processing.

6. Add the following to `Program.cs` in order to start processing the messages:

   ```C#
   await processor.StartProcessingAsync();
   Console.WriteLine("Press any key to stop processing...");
   Console.ReadKey();
   await processor.StopProcessingAsync();
   ```

   - **StartProcessingAsync**: Starts the message processor to begin processing messages from the queue.
   - **Console.ReadKey**: Waits for a key press to stop the processor.
   - **StopProcessingAsync**: Stops the processing.

7. Add the following to `Program.cs` to handle dead letter queue processing:

   ```c#
   // Retreive messages from the Dead Letter Queue
   ServiceBusProcessor deadLeterProcessor = client.CreateProcessor(deadLetterQueueName);
   
   deadLeterProcessor.ProcessMessageAsync += async args =>
   {
   	ServiceBusReceivedMessage deadLetterMessage = args.Message;
   	Console.WriteLine($"Received message from Dead Letter Queue: {deadLetterMessage.Body}");
   	await args.CompleteMessageAsync(deadLetterMessage);
   	Console.WriteLine($"Completed message from Dead Letter Queue: {deadLetterMessage.Body}");
   };
   
   deadLeterProcessor.ProcessErrorAsync += args =>
   {
   	Console.WriteLine($"Error processing message from Dead Letter Queue: {args.Exception.Message}");
   	return Task.CompletedTask;
   };
   
   await deadLeterProcessor.StartProcessingAsync();
   Console.WriteLine();
   Console.WriteLine("Press any key to stop the dead letter processor...");
   Console.ReadKey();
   await deadLeterProcessor.StopProcessingAsync();
   await client.DisposeAsync();
   ```

   - **Dead Letter Processor**: Creates a separate processing for the dead letter queue.
   - **Dead Letter Message Handling**: Defines an event handler for processing messages from the dead letter queue, marking them complete after logging.
   - **Error Handling for DLQ**: Handles any errors that occur during processing of dead letter messages.
   - **Starting and Stopping the DLQ Processor**: Starts and stops the dead letter message processor similarly to the primary processor.

8. Start the consumer application:

   ```sh
   dotnet run
   ```

   Initially, you should see a result similar to:

   ```sh
   Press any key to stop processing...
   Received message: Task 0
   Completed message: Task 0
   Received message: Task 1 fail
   Error processing message: Simulated processing error
   Received message: Task 2
   Completed message: Task 2
   Received message: Task 3 fail
   Error processing message: Simulated processing error
   Received message: Task 4
   Completed message: Task 4
   Received message: Task 5 fail
   Error processing message: Simulated processing error
   Received message: Task 6
   Completed message: Task 6
   Received message: Task 7 fail
   Error processing message: Simulated processing error
   Received message: Task 8
   Completed message: Task 8
   Received message: Task 9 fail
   Error processing message: Simulated processing error
   ```

   In this result, we see that the consumer attempted to process all of the messages, but some of them resulted in an error. In those cases, the consumer sent the messages to the dead letter queue.

9. Click any key to stop process incoming messages.

10. You should now see a result similar to:

    ```sh
    Press any key to stop the dead letter processor...
    Received message from Dead Letter Queue: Task 1 fail
    Completed message from Dead Letter Queue: Task 1 fail
    Received message from Dead Letter Queue: Task 3 fail
    Completed message from Dead Letter Queue: Task 3 fail
    Received message from Dead Letter Queue: Task 5 fail
    Completed message from Dead Letter Queue: Task 5 fail
    Received message from Dead Letter Queue: Task 7 fail
    Completed message from Dead Letter Queue: Task 7 fail
    Received message from Dead Letter Queue: Task 9 fail
    Completed message from Dead Letter Queue: Task 9 fail
    ```

    Now we see that the consumer has processed the dead letter queue messages.

### Conclusion

This hands-on exercise demonstrates the use of dead letter queues (DLQs) with Azure Service Bus. By following these steps, you have gained practical experience in handling unprocessable messages, ensuring reliable message processing and effective error management.