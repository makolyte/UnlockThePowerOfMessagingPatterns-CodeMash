using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.deadletter";

Console.WriteLine("Consumer");

// Create a Service Bus client
await using ServiceBusClient client = new(connectionString);

// Create a Service Bus proceessor for the queue
await using ServiceBusProcessor processor = client.CreateProcessor(queueName);

// Add handler to process messages
processor.ProcessMessageAsync += async args =>
{
	try
	{
		Console.WriteLine($"Processing message: {args.Message.Body}");
		if (args.Message.Body.ToString().Contains("fail"))
			throw new Exception("Simulated exception");
		await args.CompleteMessageAsync(args.Message);
		Console.WriteLine("\tMessage processed successfully.");
	}
	catch (Exception ex)
	{

		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine($"\tError processing message: {ex.Message}");
		Console.ResetColor();

		Dictionary<string, object> deadLetterProperties = new()
		{
			{"DeadLetterReason", "Processing error"},
			{"DeadLetterErrorMessage", ex.Message }
		};
		await args.DeadLetterMessageAsync(args.Message, deadLetterProperties);
	}
};

// Add handler to process any errors
processor.ProcessErrorAsync += args =>
{
	Console.WriteLine($"Error: {args.Exception.Message}");
	return Task.CompletedTask;
};

// Start processing
await processor.StartProcessingAsync();
Console.WriteLine("Processing messages from the queue. Press any key to exit...");
Console.WriteLine();
Console.ReadKey(true);

// Stop processing
await processor.StopProcessingAsync();
Console.WriteLine("Stopped processing messages.");