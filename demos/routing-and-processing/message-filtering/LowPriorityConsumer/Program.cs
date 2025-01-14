using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string topicName = "codemash.filtering";
const string subscriptionName = "LowPriority";

// Create a Service Bus client that will authenticate through the connection string
await using ServiceBusClient serviceBusClient = new(connectionString);

// Create a processor that we can use to process the messages
await using ServiceBusProcessor processor = serviceBusClient.CreateProcessor(topicName, subscriptionName);

// Add handler to process messages
processor.ProcessMessageAsync += async args =>
{
	Console.WriteLine($"Received low-priority message: {args.Message.Body}");
	await args.CompleteMessageAsync(args.Message);
};

// Add handler to process any errors
processor.ProcessErrorAsync += args =>
{
	Console.WriteLine(args.Exception.ToString());
	return Task.CompletedTask;
};

// Start processing
await processor.StartProcessingAsync();
Console.WriteLine("Waiting for low-priority messages, press any key to end processing...");
Console.ReadKey();

// Stop processing
await processor.StopProcessingAsync();
Console.WriteLine("Stopped receiving messages");