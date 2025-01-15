using Azure.Messaging.ServiceBus;

var sub1 = await AddSubscription("subscription1");
var sub2 = await AddSubscription("subscription2");

Console.ReadLine();

await Task.WhenAll(sub1.StopProcessingAsync(), sub2.StopProcessingAsync());

static async Task<ServiceBusProcessor> AddSubscription(string subscriptionName)
{
    string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
    string topicName = "codemash.fundamentals.publish-subscribe";
    ServiceBusClient client = new(connectionString);
    ServiceBusProcessor processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions()
    {
        AutoCompleteMessages = true
    });

    processor.ProcessMessageAsync += async (args) => Console.WriteLine($"Subscription {subscriptionName} Received: {args.Message.Body}");
    processor.ProcessErrorAsync += async (args) => Console.WriteLine(args.Exception.Message);

    await processor.StartProcessingAsync();

    return processor;
}