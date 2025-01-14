using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.fundamentals.point-to-point";

// Create a ServiceBusClient that will be used to create a receiver
ServiceBusClient client = new(connectionString);

// Create a ServiceBusProcessor that we can use to process messages
ServiceBusProcessor processor = client.CreateProcessor(queueName);

// Add event handlers to the processor
processor.ProcessMessageAsync += MessageHandler;
processor.ProcessErrorAsync += ErrorHandler;

// Start processing
await processor.StartProcessingAsync();

// Wait for the user to press a key to end the processing
Console.WriteLine("Press any key to end the processing...");
Console.ReadKey(true);

// Stop processing
await processor.StopProcessingAsync();

// Close the processor and client
await processor.DisposeAsync();
await client.DisposeAsync();

async Task MessageHandler(ProcessMessageEventArgs args)
{
	Console.WriteLine();
	Console.WriteLine("==== Message Received ====");
	Console.WriteLine("Sequence Number: " + args.Message.SequenceNumber);
	Console.WriteLine("Enqueued Time: " + args.Message.EnqueuedTime);
	Console.WriteLine("Message Body: " + args.Message.Body.ToString());
	await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs args)
{
	Console.WriteLine();
	Console.WriteLine("==== An error occurred ====");
	Console.WriteLine("Error Source: " + args.ErrorSource);
	Console.WriteLine("Exception: " + args.Exception.ToString());
	return Task.CompletedTask;
}

