using Azure.Messaging.ServiceBus;

string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
string queueName = "codemash.fundamentals.point-to-point";

// Create a ServiceBusClient that owns the connection and can be used to create the receiver
ServiceBusClient client = new(connectionString);

// Create a ServiceBusProcessor client for processing messages
ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

try
{
    // Add handler to process messages
    processor.ProcessMessageAsync += MessageHandler;

    // Add handler to process any errors
    processor.ProcessErrorAsync += ErrorHandler;

    // Start processing 
    await processor.StartProcessingAsync();

    Console.WriteLine("Press any key to end processing incoming messages...");
    Console.ReadKey();

    // stop processing 
    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
finally
{
    // Calling DisposeAsync on client types is required to ensure that network
    // resources and other unmanaged objects are properly cleaned up.
    await processor.DisposeAsync();
    await client.DisposeAsync();
}

// Handle received messages
async Task MessageHandler(ProcessMessageEventArgs args)
{

    // Display the message details
    Console.WriteLine();
    Console.WriteLine("==== Message Received ====");
    Console.WriteLine($"Id: {args.Message.MessageId}");
    Console.WriteLine($"Enqueued Time: {args.Message.EnqueuedTime}");
    Console.WriteLine($"Expiration Time: {args.Message.ExpiresAt}");
    Console.WriteLine($"Body: {args.Message.Body}");

    // Complete the message. The messages will be deleted from the subscription. 
    await args.CompleteMessageAsync(args.Message);

    Console.WriteLine("Press any key to end processing incoming messages...");
}

// Handle any errors when receiving messages
Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}