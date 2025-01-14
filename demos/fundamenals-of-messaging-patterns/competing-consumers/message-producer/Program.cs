using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.competingconsumers.taskQueue";

// Wait for user input before sending messages
Console.WriteLine("Press any key to send messages to the queue...");
Console.ReadKey(true);

// Create a new Service Bus client
await using ServiceBusClient client = new(connectionString);

// Create a sender for the queue
await using ServiceBusSender sender = client.CreateSender(queueName);

// Send messages to the queue
Console.WriteLine();
Console.WriteLine("Sending messages to the queue...");
for (int i = 0; i < 10; i++)
{
	ServiceBusMessage message = new($"Message {i}");
	await sender.SendMessageAsync(message);
	Console.WriteLine($"Sent message: {message.Body}");
}


