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