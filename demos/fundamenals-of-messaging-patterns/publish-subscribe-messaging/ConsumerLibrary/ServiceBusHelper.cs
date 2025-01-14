using Azure.Messaging.ServiceBus;

namespace ConsumerLibrary;

public static class ServiceBusHelper
{

	public static async Task ProcessTopicSubscription(string connectionString, string topicName, string subscriptionName)
	{

		// Create a ServiceBusClient object that you can use to create a ServiceBusProcessor
		await using ServiceBusClient client = new(connectionString);

		// Create a ServiceBusProcessor object that you can use to process messages from the subscription
		await using ServiceBusProcessor processor = client.CreateProcessor(topicName, subscriptionName);

		// Add an event handler to process messages
		processor.ProcessMessageAsync += async args =>
		{
			string body = args.Message.Body.ToString();
			Console.WriteLine($"Received message: {body}");
			await args.CompleteMessageAsync(args.Message);
		};

		// Add an event handler to process any errors
		processor.ProcessErrorAsync += args =>
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		};

		// Start processing
		await processor.StartProcessingAsync();
		Console.WriteLine();
		Console.WriteLine("Press any key to stop processing messages...");
		Console.ReadKey(true);

		// Stop processing
		await processor.StopProcessingAsync();

	}

}

