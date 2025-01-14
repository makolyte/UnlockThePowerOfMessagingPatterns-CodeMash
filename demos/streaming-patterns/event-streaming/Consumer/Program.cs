using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Storage.Blobs;

const string hostName = "codemash-messaging-patterns.servicebus.windows.net";
const string eventHubName = "eventstreamingpattern";
Uri blobContainerUri = new("https://stcodemash.blob.core.windows.net/event-streaming");

BlobContainerClient storageClient = new(
	blobContainerUri, new DefaultAzureCredential());

EventProcessorClient processor = new(
	storageClient,
	EventHubConsumerClient.DefaultConsumerGroupName,
	hostName,
	eventHubName,
	new DefaultAzureCredential());

// Register handlers for processing events and errors
processor.ProcessEventAsync += async (args) =>
{
	Console.WriteLine($"Event received: {args.Data.EventBody}");
	await args.UpdateCheckpointAsync();
};

processor.ProcessErrorAsync += async (args) =>
{
	Console.WriteLine($"Error: {args.Exception.Message}");
	await Task.CompletedTask;
};

await processor.StartProcessingAsync();
Console.WriteLine("Press any key to stop the processor...");
Console.ReadKey(true);

await processor.StopProcessingAsync();
Console.WriteLine("The processor has been stopped.");



