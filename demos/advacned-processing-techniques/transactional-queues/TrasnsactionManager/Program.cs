using Azure.Messaging.ServiceBus;
using System.Transactions;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string initiatorQueueName = "transactional-queues.initiator";
const string firstTransactionParticipantQueueName = "transactional-queues.participant-1";
const string secondTransactionParticipantQueueName = "transactional-queues.participant-2";

// Create a Service Bus client
ServiceBusClientOptions serviceBusClientOptions = new() { EnableCrossEntityTransactions = true };
await using ServiceBusClient client = new(connectionString, serviceBusClientOptions);

// Create a Service Bus receiver and senders
ServiceBusReceiver initiatorReceiver = client.CreateReceiver(initiatorQueueName);
ServiceBusSender participant1Sender = client.CreateSender(firstTransactionParticipantQueueName);
ServiceBusSender participant2Sender = client.CreateSender(secondTransactionParticipantQueueName);

// Receive a message from the initiator queue
Console.WriteLine("Ready to receive messages from the initiator queue.");
ServiceBusReceivedMessage receivedMessage = await initiatorReceiver.ReceiveMessageAsync();
Console.WriteLine($"Received message: {receivedMessage.Body}");

// Create a TransactionScope object
using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);

// Complete the initiator message
await initiatorReceiver.CompleteMessageAsync(receivedMessage);

// Send a message to the first participant queue
await participant1Sender.SendMessageAsync(new ServiceBusMessage("Sent to first participant"));
Console.WriteLine("Message sent to the first participant queue.");

// Send a message to the second participant queue
await participant2Sender.SendMessageAsync(new ServiceBusMessage("Sent to second particpant"));
Console.WriteLine("Message sent to the second participant queue.");

// Complete the transaction
transactionScope.Complete();
Console.WriteLine("Transaction completed successfully.");

// Prompt the user to press a key to exit
Console.WriteLine("Press any key to exit...");
Console.ReadKey(true);