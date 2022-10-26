using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace Api_universal_robots_rmq.RabitMQ
{
    public class RabitMQConsumer : IRabitMQConsumer
    {
        public void ConsumeMessages()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                const string ROUTINGKEY = "universalrobots.messages";

                channel.ExchangeDeclare(exchange: "robot_messages", type: "topic");

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: "robot_messages",
                                  routingKey: ROUTINGKEY);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    System.Diagnostics.Debug.WriteLine(message);
                    File.WriteAllText("C:\\Users\\thoma\\Desktop\\WriteLines.txt", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }
    }
}
