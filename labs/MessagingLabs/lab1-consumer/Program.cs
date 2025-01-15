using Azure.Messaging.ServiceBus;

//One will process
var sub1 = await AddSubscription("subscription1");
var sub2 = await AddSubscription("subscription2");
var sub3 = await AddSubscription("subscription3");
var sub4 = await AddSubscription("subscription4");

Console.ReadLine();

foreach(var sub in new List<ServiceBusProcessor> { sub1, sub2, sub3, sub4 })
{
    await sub.StopProcessingAsync();
}

static async Task<ServiceBusProcessor> AddSubscription(string subscriptionName)
{
    string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
    string queueName = "codemash.fundamentals.point-to-point";
    ServiceBusClient client = new(connectionString);
    ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions()
    {
        AutoCompleteMessages = true
    });

    processor.ProcessMessageAsync += async (args) => Console.WriteLine($"Subscription {subscriptionName} Received: {args.Message.Body}");
    processor.ProcessErrorAsync += async (args) => Console.WriteLine(args.Exception.Message);

    await processor.StartProcessingAsync();

    return processor;
}