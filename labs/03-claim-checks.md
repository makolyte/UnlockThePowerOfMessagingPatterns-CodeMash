[Unlock the Power of Messaging Patterns](https://github.com/TaleLearnCode/UnlockThePowerOfMessagingPatterns) \ [CodeMash 2025](..\..\README.md) \ [Labs](..\README.md) \

# Claim Check Pattern
The claim check patter is a design approach used in messaging systems to handle the transfer of large payloads efficieitnly. Instead of sending large files or data directly through the message queue, which can lead to performance issues, the payload is stored in a seperate storage system. The messaging system sends a small message containing a reference or "claim check" to the storage location of the payload. The recipient can then use this claim check to retrieve the actual data from the storage system. This pattern helps optimize the messaging system's performance, reduce network congestion, and ensure reliable delivery of large data set.

#### Key Components

- **External Storage System**: A reliable storage system (e.g., blob storage, database) to store large message payloads.
- **Claim Check**: A unique reference or identifier that is included in the message in order to retrieve the payload from the storage system.
- **Message Queue**: The messaging system that passes lightweight messages containing the claim check.
- **Payload Retrieval Logic**: Logic to retrieve the payload from the external storage system using the claim check when needed.

#### How it Works

1. **Payload Storage**: When a sender needs to send a large payload, it first stores the payload in an external storage system (e.g., a database or a cloud storage service).
1. **Generate Claim Check**: The send then generates a unique identifier or reference, known as a "claim check," that points to the location of the stored payload.
1. **Send Claim Check**: Instead of sending the entire payload, the sender sends a small message containing the claim check through the messaging system.
1. **Retrieve Claim Check**: The recipient receives the message with the claim check and uses it to retrieve the actual payload from the external storage system.
1. **Process Payload**: Once the recipient retrieves the payload, it can process it as needed.

### Benefits

- **Improved Performance**: Reduces the size of messages in the queue, allowing for faster transmissions and processing.
- **Scalability**: Offloads large payloads to an external storage system, enabling the message queue to handle a higher volume of messages.
- **Resource Efficiency**: Optimize the use of queue resources by keeping messages lightweight and manageable.

### Use Cases

- **Large File Transfers**: When transferring large files or multimedia content, storing the files externally and passing references ensures efficient message processing.
- **Complex Data Structures**: For messages containing complex or large data structures, using a claim check prevents overloading the message queue.
- **Resource Optimization**: Optimizing the use of message queue resources by reducing the payload size and ensuring faster message transmission.

### Example: High-Resolution Image Processing for Marketing Campaigns

Imagine you are working for a company the processes high-resolution images for marketing campaigns. Sending these large image files directly through the messaging system can slow it down. Instead, your company uses the claim check pattern.

Here is how it play out:

1. **Store the Image**: When an employee uploads a high-resolution image, the image gets stored in a cloud storage service like Azur Blob Storage or AWS S3.
2. **Generate Claim Check**: The system generates a unique identifier (claim check) for the image and stores it in the database.
3. **Send Claim Check**: Instead of sending the entire image, the system sends a message containing the claim check through the messaging queue to the marketing team.
4. **Retrieve the Image**: The marketing team receives the message with the claim check, uses the identifier to retrieve the high-resolution image from the cloud storage.

This allows the messaging system to handle large files efficiently without performance issues, making the entire process smoother and faster.

### Conclusion

The claim check pattern is an effective solution for managing large payloads in messaging systems. By offloading large data to external storage an using lightweight references, this pattern enhances the performance, scalability, and resource efficiency of the messaging system. It is particularly beneficial for scenarios involving large file transfer, complex data structure, and optimization of queue resources. Implementing the claim check pattern can lead to smoother and more efficient data processing workflows, ensuring that both the messaging system and its users benefit from improved performance and reliability.

## Hands-On Exercise

### Step 1: Setup the Lab Folder

1. Open Visual Studio Code.
2. Create a new folder for your project named `claim-check`.
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
   using Azure.Storage.Blobs;
   using System.Text;
   
   const string serviceBusConnectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
   const string storageConnectionString = "UseDevelopmentStorage=true";
   const string queueName = "claim-check";
   const string blobContainerName = "claim-check";
   
   // Prompt the user to start sending messages
   Console.WriteLine("Press any key to start sending messages.");
   Console.ReadKey(true);
   
   // Create the payload
   string payload = "This is a large payload that needs to be stored in external storage.";
   
   // Create a unique identifier for the claim check
   string claimCheckId = Guid.NewGuid().ToString();
   
   // Create a blob client and upload the payload
   BlobServiceClient blobServiceClient = new(storageConnectionString);
   BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
   await blobContainerClient.CreateIfNotExistsAsync();
   BlobClient blobClient = blobContainerClient.GetBlobClient(claimCheckId);
   using MemoryStream stream = new(Encoding.UTF8.GetBytes(payload));
   await blobClient.UploadAsync(stream);
   
   // Create and send the messaage
   await using ServiceBusClient serviceBusClient = new(serviceBusConnectionString);
   await using ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);
   ServiceBusMessage message = new("Claim check for payload");
   message.ApplicationProperties.Add("ClaimCheckId", claimCheckId);
   await serviceBusSender.SendMessageAsync(message);
   Console.WriteLine($"Message sent with claim check ID: {claimCheckId}");
   ```

   This code stores a "large" payload in Azure Blob Storage and sends a message to an Azure Service Bus queue containing a claim check identifier that references the stored payload. This allows for the payload to be externally stored while keeping the message size small.

4. Run the application:

   ```sh
   dotnet run
   ```

   You should see a result similar to:

   ```sh
   Press any key to start sending messages.
   Message sent with claim check ID: 141afb21-eb7e-43f9-a406-4bd8a9bfaf51
   ```

### Step 3: Set up the Consumer in Visual Studio Code

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

3. Replace the content of `Program.cs` with the following code:

   ```C#
   using Azure.Messaging.ServiceBus;
   using Azure.Storage.Blobs;
   using System.Text;
   
   const string serviceBusConnectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
   const string storageConnectionString = "UseDevelopmentStorage=true";
   const string queueName = "claim-check";
   const string blobContainerName = "claim-check";
   
   // Create a Service Bus client and processor
   await using ServiceBusClient serviceBusClient = new(serviceBusConnectionString);
   await using ServiceBusProcessor serviceBusProcessor = serviceBusClient.CreateProcessor(queueName);
   
   // Register the message handler
   serviceBusProcessor.ProcessMessageAsync += async processMessageEventArgs =>
   {
   
   	Console.WriteLine();
   
   	// Get the claim check ID from the message
   	string? claimCheckId = processMessageEventArgs.Message.ApplicationProperties["ClaimCheckId"].ToString();
   	if (claimCheckId == null)
   	{
   		Console.WriteLine("Claim check ID not found in message.");
   		await processMessageEventArgs.DeadLetterMessageAsync(processMessageEventArgs.Message);
   		return;
   	}
   	Console.WriteLine($"Received message with claim check ID: {claimCheckId}");
   
   	// Download the payload from the blob storage
   	BlobServiceClient blobServiceClient = new(storageConnectionString);
   	BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
   	BlobClient blobClient = blobContainerClient.GetBlobClient(claimCheckId);
   	using MemoryStream stream = new();
   	await blobClient.DownloadToAsync(stream);
   	string payload = Encoding.UTF8.GetString(stream.ToArray());
   
   	// Process the payload
   	Console.WriteLine($"Payload: {payload}");
   
   	// Complete the message
   	await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
   
   };
   
   serviceBusProcessor.ProcessErrorAsync += processErrorEventArgs =>
   {
   	Console.WriteLine($"Exception: {processErrorEventArgs.Exception}");
   	return Task.CompletedTask;
   };
   
   await serviceBusProcessor.StartProcessingAsync();
   Console.WriteLine("Press any key to stop processing messages.");
   Console.ReadKey(true);
   
   await serviceBusProcessor.StopProcessingAsync();
   ```

   The code sets up a Service BUs processor to retrieve messages from an Azure Service Bus queue. It retrieves a claim check identifier from each message, uses that identifier to download from Azure Blob Storage, processes the payload, and then completes the message. It also includes error handling to manage any issues during message processing.

4. Start the consumer application:

   ```sh
   dotnet run
   ```

   Initially, you should see a result similar to:

   ```sh
   Press any key to stop processing messages.
   
   Received message with claim check ID: 141afb21-eb7e-43f9-a406-4bd8a9bfaf51
   Payload: This is a large payload that needs to be stored in external storage.
   ```


### Conclusion

This exercise demonstrates how to implement the claim check pattern using Azure Service Bus and Azure Blob Storage. By offloading large files to Blob Storage and using Service BUs to send lightweight references, you can optimize the performance and scalability of your messaging system.