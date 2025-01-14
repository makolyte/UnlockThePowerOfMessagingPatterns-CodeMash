using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "sequential-convoy";

// Prompt the user to start sending messages
Console.WriteLine("Press any key to start sending messages...");
Console.ReadKey(true);


// Build the list of messages to send
List<ServiceBusMessage> messages = [];
messages.Add(new ServiceBusMessage("Message 1 for order order-1") { SessionId = "order-1" });
messages.Add(new ServiceBusMessage("Message 1 for order order-2") { SessionId = "order-2" });
messages.Add(new ServiceBusMessage("Message 2 for order order-2") { SessionId = "order-2" });
messages.Add(new ServiceBusMessage("Message 3 for order order-2") { SessionId = "order-2" });
messages.Add(new ServiceBusMessage("Message 2 for order order-1") { SessionId = "order-1" });
messages.Add(new ServiceBusMessage("Message 3 for order order-1") { SessionId = "order-1" });

// Create a Service Bus client and sender
await using ServiceBusClient serviceBusClient = new(connectionString);
await using ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);

// Send the messages to the Service Bus queue
foreach (ServiceBusMessage message in messages)
{
	await serviceBusSender.SendMessageAsync(message);
	Console.WriteLine($"Sent message: {message.Body}");
}