using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string highPriorityQueue = "codemash.routing.high-priority";
const string lowPriorityQueue = "codemash.routing.low-priority";

// Process messages from the high-priority queue
await ProcessMessagesAsync(highPriorityQueue, "high");

// Process messages from the low-priority queue
await ProcessMessagesAsync(lowPriorityQueue, "low");

async Task ProcessMessagesAsync(string queueName, string priority)
{

	// Create a new instance of the ServiceBusClient
	await using ServiceBusClient client = new(connectionString);

	// Create a new instance of the ServiceBusProcessor for the specified queue
	await using ServiceBusProcessor serviceBusProcessor = client.CreateProcessor(queueName);

	// Register a message handler for the  queue
	serviceBusProcessor.ProcessMessageAsync += async args =>
	{
		// Process the message
		Console.WriteLine($"Received {priority}-priority message: {args.Message.Body}");
		await args.CompleteMessageAsync(args.Message);
	};

	// Register an error handler for the queue
	serviceBusProcessor.ProcessErrorAsync += args =>
	{
		Console.WriteLine($"Error processing {priority}-priority message: {args.Exception.Message}");
		return Task.CompletedTask;
	};

	// Start processing messages from the queue
	await serviceBusProcessor.StartProcessingAsync();
	Console.WriteLine();
	Console.WriteLine($"Press any key to stop processing {priority} priority messages...");
	Console.ReadKey(true);

	// Stop processing messages from the queue
	await serviceBusProcessor.StopProcessingAsync();

}