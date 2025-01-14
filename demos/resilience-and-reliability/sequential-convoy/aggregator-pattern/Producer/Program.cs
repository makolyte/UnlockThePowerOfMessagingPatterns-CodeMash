using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.aggregator.input";

Console.WriteLine("Producer Online...");
Console.WriteLine();

await using ServiceBusClient client = new(connectionString);
await using ServiceBusSender sender = client.CreateSender(queueName);

Console.WriteLine("Press any key to send messages...");
Console.ReadKey();

for (int i = 0; i < 5; i++)
{
	ServiceBusMessage message = new($"Message {i}");
	message.ApplicationProperties["ValueToAggregate"] = i;
	await sender.SendMessageAsync(message);
	Console.WriteLine($"Sent message {i}");
}

Console.WriteLine("Done!");