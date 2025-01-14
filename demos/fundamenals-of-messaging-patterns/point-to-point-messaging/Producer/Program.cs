using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "codemash.fundamentals.point-to-point";

// Prompt the user to send a message
Console.WriteLine("Press any key to send a message to the queue...");
Console.ReadKey(true);

// Create a ServiceBusClient that will be used to create a sender
ServiceBusClient client = new(connectionString);

// Create a ServiceBusSender that we can use to send messages to the queue
ServiceBusSender sender = client.CreateSender(queueName);

// Create a message that we can send
ServiceBusMessage message = new("Hello, CodeMash!");

// Send the message
await sender.SendMessageAsync(message);
Console.WriteLine("==== Message Sent ====");
Console.WriteLine(message.Body.ToString());

// Close the sender and client
await sender.DisposeAsync();
await client.DisposeAsync();




