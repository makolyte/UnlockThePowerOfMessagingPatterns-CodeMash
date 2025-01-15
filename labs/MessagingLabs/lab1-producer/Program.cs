using Azure.Messaging.ServiceBus;

var connectionString = "Endpoint=sb://127.0.0.1:5672;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
var queueName = "codemash.fundamentals.point-to-point"; //change this to something in Config.json

//Note: 'await using' because it's IAsyncDisposable instead of IDisposable
await using var client = new ServiceBusClient(connectionString);
await using var sender = client.CreateSender(queueName);

while (true)
{
    Console.Write("Message:");
    var message = Console.ReadLine();
    ArgumentException.ThrowIfNullOrEmpty(message);

    await sender.SendMessageAsync(new ServiceBusMessage(message));

    Console.WriteLine("Sent message");
    Console.WriteLine();
}
