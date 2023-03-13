using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Sender";

IConnection cnn = factory.CreateConnection();
IModel channel = cnn.CreateModel();

const string exchangeName = "PoCExchange";
const string routingKey = "poc-routing-key";
const string queueName = "PoCQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

for (int i = 0; i < 60; i++)
{
    Console.WriteLine($"Sending Message {i}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Message #{i}");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    Thread.Sleep(1000);
}



channel.Close();
cnn.Close();
