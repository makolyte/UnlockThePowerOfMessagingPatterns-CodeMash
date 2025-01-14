using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "sequential-convoy";

Console.WriteLine("Waiting for messages...");

await using ServiceBusClient serviceBusClient = new(connectionString);

while (true)
{
	await using ServiceBusSessionReceiver sessionReceiver = await serviceBusClient.AcceptNextSessionAsync(queueName);
	if (sessionReceiver == null)
	{
		break; // No more sessions available
	}

	Console.WriteLine();
	Console.WriteLine($"Accepted session: {sessionReceiver.SessionId}");

	ServiceBusReceivedMessage message = await sessionReceiver.ReceiveMessageAsync();
	while (message != null)
	{
		string body = message.Body.ToString();
		string sessionId = message.SessionId;
		Console.WriteLine($"Received message: {body}");
		await sessionReceiver.CompleteMessageAsync(message);
		message = await sessionReceiver.ReceiveMessageAsync(TimeSpan.FromSeconds(2));
	}
}
