using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Consumer;

public class SequentialConsumer(ILogger<SequentialConsumer> logger)
{
	private readonly ILogger<SequentialConsumer> _logger = logger;

	[Function(nameof(SequentialConsumer))]
	public async Task Run(
		[ServiceBusTrigger("%QueueName%", Connection = "ServiceBusConnectionString", IsSessionsEnabled = true)]
			ServiceBusReceivedMessage message,
		ServiceBusMessageActions messageActions)
	{
		_logger.LogInformation("Message ID: {id}", message.MessageId);
		_logger.LogInformation("Message Body: {body}", message.Body);
		_logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

		// Complete the message
		await messageActions.CompleteMessageAsync(message);
	}
}