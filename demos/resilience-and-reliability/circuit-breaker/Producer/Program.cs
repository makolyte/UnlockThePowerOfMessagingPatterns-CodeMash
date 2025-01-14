using Azure.Messaging.ServiceBus;
using Polly;
using Polly.CircuitBreaker;
using System.Text;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string topicName = "circuit-breaker";

int delayInBetweenMessages = 100;

AsyncCircuitBreakerPolicy circuitBreakerPolicy = Policy
	.Handle<Exception>()
	.CircuitBreakerAsync(
		exceptionsAllowedBeforeBreaking: 3,
		durationOfBreak: TimeSpan.FromSeconds(5),
		onBreak: (exception, breakDelay) =>
		{
			Console.WriteLine($"Circuit breaker tripped! Breaking for {breakDelay.TotalSeconds} seconds due to: {exception.Message}");
			delayInBetweenMessages = 5000;
		},
		onReset: () =>
		{
			Console.WriteLine("Circuit breaker reset.");
			delayInBetweenMessages = 100;
		},
		onHalfOpen: () =>
		{
			Console.WriteLine("Circuit breaker is half-open. Testing the operation...");
		});

Console.WriteLine("Press any key to start sending messages...");
Console.ReadKey(true);
Console.WriteLine();

for (int i = 0; i < 30; i++)
{
	await SendMessageWithCircuitBreakerAsync();
	Thread.Sleep(delayInBetweenMessages);
}

Console.WriteLine();
Console.WriteLine("Done sending messages; press any key to exit...");
Console.ReadKey(true);

async Task SendMessageWithCircuitBreakerAsync()
{

	await using ServiceBusClient serviceBusClient = new(connectionString);
	await using ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(topicName);

	try
	{
		await circuitBreakerPolicy.ExecuteAsync(async () =>
		{
			string messageBody = "This is a test message for the Circuit Breaker pattern.";
			ServiceBusMessage message = new(Encoding.UTF8.GetBytes(messageBody));

			// Simulating a failure scenario
			if (DateTime.UtcNow.Second % 2 == 0)
			{
				throw new Exception("Simulated failure.");
			}

			await serviceBusSender.SendMessageAsync(message);
			Console.WriteLine($"Sent a message at: {DateTime.UtcNow}");

		});
	}
	catch
	{
		Console.WriteLine($"Attempted to send a message at: {DateTime.UtcNow}");
	}

}

