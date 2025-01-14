using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.deadletter/$DeadLetterQueue";

Console.WriteLine("Dead Letter Consumer");

// Create a Service Bus client
await using ServiceBusClient client = new(connectionString);

// Create a Service Bus proceessor for the queue
await using ServiceBusProcessor processor = client.CreateProcessor(queueName);

// Add handler to process messages
processor.ProcessMessageAsync += async args =>
{
	Console.WriteLine($"Processing DLQ message: {args.Message.Body}");
	if (args.Message.ApplicationProperties.TryGetValue("DeadLetterReason", out object? reason))
	{
		Console.WriteLine($"\tDead Letter Reason: {reason}");
	}
	if (args.Message.ApplicationProperties.TryGetValue("DeadLetterErrorMessage", out object? errorMessage))
	{
		Console.WriteLine($"\tDead Letter Error Message: {errorMessage}");
	}
	await args.CompleteMessageAsync(args.Message);
};

// Add handler to process any errors
processor.ProcessErrorAsync += args =>
{
	Console.WriteLine($"Error: {args.Exception.Message}");
	return Task.CompletedTask;
};

// Start processing
await processor.StartProcessingAsync();
Console.WriteLine("Processing messages from the dead letter queue. Press any key to exit...");
Console.ReadKey(true);
Console.WriteLine();

// Stop processing
await processor.StopProcessingAsync();
Console.WriteLine("Stopped processing messages.");


