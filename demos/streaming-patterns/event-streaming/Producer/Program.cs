using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

const string hostName = "codemash-messaging-patterns.servicebus.windows.net";
const string eventHubName = "eventstreamingpattern";

// Wait for the user to press a key before sending events
Console.WriteLine("Press any key to send events to the Event Hub...");
Console.ReadKey(true);

// Create a producer client that we can use to send messages to the event hub
await using EventHubProducerClient producer = new(hostName, eventHubName, new DefaultAzureCredential());

// Create a batch of events
Console.WriteLine();
Console.WriteLine("Creating a batch of events to send to the Event Hub...");
using EventDataBatch eventBatch = await producer.CreateBatchAsync();
for (int i = 0; i < 10; i++)
{
	string eventData = $"Event {i}: {DateTime.UtcNow}";
	eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(eventData)));
	Console.WriteLine($"\tAdded: {eventData}");
}

// Send the batch of events to the Event Hub
Console.WriteLine();
Console.WriteLine("Sending the batch of events to the Event Hub...");
await producer.SendAsync(eventBatch);
Console.WriteLine("The batch of events has been sent to the Event Hub.");


