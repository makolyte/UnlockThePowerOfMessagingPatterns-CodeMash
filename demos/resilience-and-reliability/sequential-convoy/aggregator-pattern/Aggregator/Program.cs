using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string inputQueueName = "codemash.aggregator.input";
const string outputQueueName = "codemash.aggregator.output";

Console.WriteLine("Aggregator Online...");
Console.WriteLine();

List<ServiceBusReceivedMessage> receivedMessages = await ReceivedMessagesAsync();
int aggregatedValue = AggregateMessages(receivedMessages);
await SendOutputMessageAsync(aggregatedValue);

Console.WriteLine("Done!");
Console.ReadKey();

async Task<List<ServiceBusReceivedMessage>> ReceivedMessagesAsync()
{
	await using ServiceBusClient serviceBusClient = new(connectionString);
	await using ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(inputQueueName);
	List<ServiceBusReceivedMessage> receivedMessages = [];
	Console.WriteLine("Receieving messages...");
	while (receivedMessages.Count < 5)
	{
		ServiceBusReceivedMessage receivedMessage = await serviceBusReceiver.ReceiveMessageAsync();
		receivedMessages.Add(receivedMessage);
		Console.WriteLine("\tReceived message: " + receivedMessage.Body);
		await serviceBusReceiver.CompleteMessageAsync(receivedMessage);
	}
	return receivedMessages;
}

int AggregateMessages(List<ServiceBusReceivedMessage> messages)
{
	Console.WriteLine("Aggregating messages...");
	return messages.ToArray().Select(x => (int)x.ApplicationProperties["ValueToAggregate"]).Sum();
}

async Task SendOutputMessageAsync(int aggregatedValue)
{
	Console.WriteLine("Sending aggregated value...");
	await using ServiceBusClient serviceBusClient = new(connectionString);
	await using ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(outputQueueName);
	Console.WriteLine("Sending aggregated value...");
	ServiceBusMessage ouptutMessage = new($"Aggregated value: {aggregatedValue}");
	await serviceBusSender.SendMessageAsync(ouptutMessage);
}