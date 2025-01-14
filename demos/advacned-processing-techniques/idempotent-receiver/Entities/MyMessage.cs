namespace Entities;

public class MyMessage
{
	public Guid MessageId { get; set; } = Guid.NewGuid();
	public required string Content { get; set; }
}