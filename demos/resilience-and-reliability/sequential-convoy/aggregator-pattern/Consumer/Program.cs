using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.aggregator.output";

Console.WriteLine("Consumer Online...");
Console.WriteLine();

await using ServiceBusClient client = new(connectionString);
await using ServiceBusProcessor processor = client.CreateProcessor(queueName);

processor.ProcessMessageAsync += args =>
{
	Console.WriteLine();
	Console.WriteLine("Received message: " + args.Message.Body);
	return args.CompleteMessageAsync(args.Message);
};

processor.ProcessErrorAsync += args =>
{
	Console.WriteLine(args.Exception);
	return Task.CompletedTask;
};

await processor.StartProcessingAsync();
Console.WriteLine("Ready to receive aggregation outputs...");
Console.WriteLine("Press any key to stop...");
Console.ReadKey();
await processor.StopProcessingAsync();
