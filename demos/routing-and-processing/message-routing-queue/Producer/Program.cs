using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string highPriorityQueue = "codemash.routing.high-priority";
const string lowPriorityQueue = "codemash.routing.low-priority";

// Prompt the user to start sending messages
Console.WriteLine("Press any key to start sending messages...");
Console.ReadKey(true);


// Create a new instance of the ServiceBusClient
await using ServiceBusClient client = new(connectionString);

// Create a new instance of the ServiceBusSender for the high-priority queue
await using ServiceBusSender highPrioritySender = client.CreateSender(highPriorityQueue);

// Create a new instance of the ServiceBusSender for the low-priority queue
await using ServiceBusSender lowPrioritySender = client.CreateSender(lowPriorityQueue);

// Send messages to the queues
Console.WriteLine();
Console.WriteLine("Sending messages to the queues...");
for (int i = 0; i < 10; i++)
{

	// Determine the priority of the message
	string priority = (i % 3 == 0) ? "high" : "low";

	// Create a new message
	ServiceBusMessage message = new($"Message {i + 1}");
	message.ApplicationProperties.Add("priority", priority);

	// Send the message to the appropriate queue based on its priority
	if (priority == "high")
		await highPrioritySender.SendMessageAsync(message);
	else
		await lowPrioritySender.SendMessageAsync(message);

	Console.WriteLine($"\tSent message {i + 1} with priority {priority}");

}

// Wait for the user to press a key before exiting
Console.WriteLine();
Console.WriteLine("Finished sending messages. Press any key to exit...");
Console.ReadKey(true);