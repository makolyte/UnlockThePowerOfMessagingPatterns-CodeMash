using ConsumerLibrary;

const string connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
const string topicName = "codemash.fundamentals.publish-subscribe";
const string subscriptionName = "subscription2";

Console.WriteLine("Consumer 2 is listening for messages...");
await ServiceBusHelper.ProcessTopicSubscription(connectionString, topicName, subscriptionName);




