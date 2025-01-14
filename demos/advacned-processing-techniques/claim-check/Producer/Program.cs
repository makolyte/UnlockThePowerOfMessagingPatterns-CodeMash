using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using System.Text;

const string serviceBusConnectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string storageConnectionString = "UseDevelopmentStorage=true";
const string queueName = "claim-check";
const string blobContainerName = "claim-check";

// Prompt the user to start sending messages
Console.WriteLine("Press any key to start sending messages.");
Console.ReadKey(true);

// Create the payload
string payload = "This is a large payload that needs to be stored in external storage.";

// Create a unique identifier for the claim check
string claimCheckId = Guid.NewGuid().ToString();

// Create a blob client and upload the payload
BlobServiceClient blobServiceClient = new(storageConnectionString);
BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
await blobContainerClient.CreateIfNotExistsAsync();
BlobClient blobClient = blobContainerClient.GetBlobClient(claimCheckId);
using MemoryStream stream = new(Encoding.UTF8.GetBytes(payload));
await blobClient.UploadAsync(stream);

// Create and send the messaage
await using ServiceBusClient serviceBusClient = new(serviceBusConnectionString);
await using ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);
ServiceBusMessage message = new("Claim check for payload");
message.ApplicationProperties.Add("ClaimCheckId", claimCheckId);
await serviceBusSender.SendMessageAsync(message);
Console.WriteLine($"Message sent with claim check ID: {claimCheckId}");