using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string initiatorQueueName = "transactional-queues.initiator";

// Prompt the user to start sending messages
Console.WriteLine("Press any key to start sending messages...");
Console.ReadKey(true);

// Create a Service Bus client and sender
await using ServiceBusClient sericeBusClient = new(connectionString);
await using ServiceBusSender serviceBusSender = sericeBusClient.CreateSender(initiatorQueueName);

// Send a message to the initiator queue
await serviceBusSender.SendMessageAsync(new ServiceBusMessage("Initiate Transaction"));
Console.WriteLine("Message sent to the initiator queue.");

// Prompt the user to end the application
Console.WriteLine("Press any key to exit...");
Console.ReadKey(true);