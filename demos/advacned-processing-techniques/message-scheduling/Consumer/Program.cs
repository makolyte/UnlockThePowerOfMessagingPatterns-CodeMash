

using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "message-scheduling";

await using ServiceBusClient serviceBusClient = new(connectionString);
await using ServiceBusProcessor serviceBusProcessor = serviceBusClient.CreateProcessor(queueName);

serviceBusProcessor.ProcessMessageAsync += async processMessageEventArgs =>
{
	string body = processMessageEventArgs.Message.Body.ToString();
	Console.WriteLine($"Processing message: {body}");
	Console.WriteLine(processMessageEventArgs.Message.EnqueuedTime);
	await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
};

serviceBusProcessor.ProcessErrorAsync += processErrorEventArgs =>
{
	Console.WriteLine($"Error when receiving messages: {processErrorEventArgs.Exception}");
	return Task.CompletedTask;
};

await serviceBusProcessor.StartProcessingAsync();
Console.WriteLine("Press any key to stop receiving messages...");
Console.ReadKey(true);

await serviceBusProcessor.StopProcessingAsync();
Console.WriteLine("Stopped receiving messages. Press any key to exit...");
Console.ReadKey(true);

