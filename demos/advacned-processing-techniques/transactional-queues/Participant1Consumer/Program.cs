using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "transactional-queues.participant-1";

await using ServiceBusClient client = new(connectionString);
await using ServiceBusProcessor serviceBusProcessor = client.CreateProcessor(queueName);

serviceBusProcessor.ProcessMessageAsync += async processMessageEventArgs =>
{
	ServiceBusReceivedMessage message = processMessageEventArgs.Message;
	Console.WriteLine($"Received message: {message.Body}");
	await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
};

serviceBusProcessor.ProcessErrorAsync += args =>
{
	Console.WriteLine(args.Exception.ToString());
	return Task.CompletedTask;
};

await serviceBusProcessor.StartProcessingAsync();
Console.WriteLine("Transactional Participant 1 is listening for messages. Press any key to stop receiving messages...");
Console.ReadKey(true);

await serviceBusProcessor.StopProcessingAsync();