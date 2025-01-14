using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.deadletter";

// Wait for user input to start sending messages
Console.WriteLine("Press any key to send messages to the queue...");
Console.ReadKey(true);

// Create a Service Bus client
await using ServiceBusClient client = new(connectionString);

// Create a sender for the queue
await using ServiceBusSender sender = client.CreateSender(queueName);

// Create messages to send to the queue
Console.WriteLine();
Console.WriteLine("Sending messages to the queue...");
for (int i = 0; i < 10; i++)
{
	string messageContent = i % 2 == 0 ? $"Task {i}" : $"Task {i} fail";
	ServiceBusMessage message = new(messageContent);
	await sender.SendMessageAsync(message);
	Console.WriteLine($"\tSent message: {messageContent}");
}
Console.WriteLine("All messages sent to the queue.");


