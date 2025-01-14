using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string topicName = "codemash.filtering";

Console.WriteLine("Press any key to send messages to the topic...");
Console.ReadKey();

await using ServiceBusClient serviceBusClient = new(connectionString);

await SendMessage(serviceBusClient, topicName, "Hello from the Producer!", "High");
await SendMessage(serviceBusClient, topicName, "Hello from the Producer!", "Low");

static async Task SendMessage(ServiceBusClient serviceBusClient, string topicName, string messageBody, string priority)
{
	await using ServiceBusSender sender = serviceBusClient.CreateSender(topicName);
	ServiceBusMessage message = new(messageBody);
	message.Subject = priority;
	await sender.SendMessageAsync(message);
}