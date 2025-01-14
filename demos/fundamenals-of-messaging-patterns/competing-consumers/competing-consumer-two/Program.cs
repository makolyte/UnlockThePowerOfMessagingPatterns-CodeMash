using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.competingconsumers.taskQueue";

// Create a new Service Bus client
await using var client = new ServiceBusClient(connectionString);

// Create a Service Bus processor for the queue
await using var processor = client.CreateProcessor(queueName);

// Add a handler to process messages
processor.ProcessMessageAsync += async args =>
{
	Console.WriteLine();
	Console.WriteLine($"Consumer 2 processing message: {args.Message.Body}");
	await Task.Delay(new Random().Next(200, 1500)); // Simulate variable processing time
	await args.CompleteMessageAsync(args.Message);
	Console.WriteLine($"Consumer 2 completed message: {args.Message.Body}");
};

// Add a handler to process any errors
processor.ProcessErrorAsync += args =>
{
	Console.WriteLine($"Error: {args.Exception}");
	return Task.CompletedTask;
};

// Start processing messages
await processor.StartProcessingAsync();
Console.WriteLine("Console 2 is ready to process messages; press any key to stop processing...");
Console.ReadKey(true);

// Stop processing messages
await processor.StopProcessingAsync();
Console.WriteLine("Stopped processing messages.");


