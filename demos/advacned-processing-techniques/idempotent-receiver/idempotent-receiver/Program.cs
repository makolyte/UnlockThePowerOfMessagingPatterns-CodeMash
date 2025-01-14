using Azure.Messaging.ServiceBus;
using Entities;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string queueName = "idempotent-receiver";

MemoryCache _cache = new(new MemoryCacheOptions());

// Create a Service Bus client and processor to process messages
await using ServiceBusClient serviceBusClient = new(connectionString);
await using ServiceBusProcessor processor = serviceBusClient.CreateProcessor(queueName);

// Add an event handler to the processor to process messages
processor.ProcessMessageAsync += async args =>
{

	MyMessage? message = JsonSerializer.Deserialize<MyMessage>(args.Message.Body.ToString());

	if (message is not null)

	{
		if (!_cache.TryGetValue(message.MessageId, out _))
		{
			// Process the message
			Console.WriteLine($"Processing message: {message.Content}");

			// Mark the message as processed
			_cache.Set(message.MessageId, true, TimeSpan.FromMinutes(10));
		}
		else
		{
			Console.WriteLine($"Message with ID {message.MessageId} has already been processed.");
		}

		// Complete the message
		await args.CompleteMessageAsync(args.Message);

	}
	else
	{
		await args.DeadLetterMessageAsync(args.Message);
	}

};

// Add an error handler to the processor to handle any errors when receiving messages
processor.ProcessErrorAsync += args =>
{
	Console.WriteLine(args.Exception.ToString());
	return Task.CompletedTask;
};

// Start processing messages from the queue
await processor.StartProcessingAsync();
Console.WriteLine();
Console.WriteLine("Press any key to stop receiving messages...");
Console.ReadKey(true);

// Stop processing messages from the queue
await processor.StopProcessingAsync();