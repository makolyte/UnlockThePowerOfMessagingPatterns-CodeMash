using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string topicName = "codemash.fundamentals.publish-subscribe";

// Ask the user to press a key to send a message to the topic
Console.WriteLine("Press any key to send a message to the topic: codemash.fundamentals.publish-subscribe");
Console.ReadKey(true);

// Create a ServiceBusClient object that you can use to create a ServiceBusSender
await using ServiceBusClient client = new(connectionString);

// Create a ServiceBusSender object that you can use to send messages to the topic
ServiceBusSender sender = client.CreateSender(topicName);

// Create a message that we can send
ServiceBusMessage message = new("Hello, CodeMash Subscribers!");

// Send the message to the topic
await sender.SendMessageAsync(message);
Console.WriteLine($"Sending message: {message.Body}");

// Dispose of the client and sender
await sender.DisposeAsync();
await client.DisposeAsync();


