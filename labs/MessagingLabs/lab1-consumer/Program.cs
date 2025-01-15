using Azure.Messaging.ServiceBus;

string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
string queueName = "codemash.fundamentals.point-to-point";

await using ServiceBusClient client = new(connectionString);
await using ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions()
{
    AutoCompleteMessages = true
});


processor.ProcessMessageAsync += async (args) => Console.WriteLine($"Received: {args.Message.Body}");
processor.ProcessErrorAsync += async (args) => Console.WriteLine(args.Exception.Message);

await processor.StartProcessingAsync();

Console.ReadLine();

await processor.StopProcessingAsync();