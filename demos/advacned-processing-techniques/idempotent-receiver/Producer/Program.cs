using Azure.Messaging.ServiceBus;
using Entities;
using System.Text.Json;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "idempotent-receiver";

// Prompt the user to start sending messages
Console.WriteLine("Press any key to start sending messages...");
Console.ReadKey(true);


await using ServiceBusClient serviceBusClient = new(connectionString);
await using ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);

// Send messages to the queues
Console.WriteLine();
Console.WriteLine("Sending messages to the queues...");

MyMessage message1 = new() { Content = "First attempt" };
MyMessage message2 = new() { MessageId = message1.MessageId, Content = "Duplicate attempt" };

await serviceBusSender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(message1)));
await serviceBusSender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(message2)));

// Wait for the user to press a key before exiting
Console.WriteLine();
Console.WriteLine("Finished sending messages. Press any key to exit...");
Console.ReadKey(true);