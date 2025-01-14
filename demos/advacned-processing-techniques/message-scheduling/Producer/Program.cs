using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "message-scheduling";

// Prompt the user to start sending messages
Console.WriteLine("Press any key to start sending messages.");
Console.ReadKey(true);

await using ServiceBusClient serviceBusClient = new(connectionString);
ServiceBusSender sender = serviceBusClient.CreateSender(queueName);

// Send a message that will be processed at a later time
ServiceBusMessage message = new($"Hello, Scheduling! Current Time: {DateTime.UtcNow}")
{
	ScheduledEnqueueTime = DateTimeOffset.UtcNow.AddSeconds(5)
};
await sender.SendMessageAsync(message);
Console.WriteLine($"Message '{message.Body}' scheduled to be sent at: {message.ScheduledEnqueueTime}");

// Send a message that will be processed right away
message = new ServiceBusMessage($"Hello, World! Current Time: {DateTime.UtcNow}");
await sender.SendMessageAsync(message);
Console.WriteLine($"Message '{message.Body}' sent immediately.");

Console.WriteLine();
Console.WriteLine("Finished sending messages. Press any key to exit...");
Console.ReadKey(true);