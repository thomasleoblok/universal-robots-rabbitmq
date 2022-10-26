using MessageModel;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Headers;
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
                channel.ExchangeDeclare(exchange: "universalrobots", type: "topic");
                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queue: queueName,
                                  exchange: "universalrobots",
                                  routingKey: "universalrobots.default");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;

                    SendPushNotification(message);

                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);



                while (true) { }
            }
        }


        private async void SendPushNotification(string message)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.logsnag.com/v1/log"),
                //Headers = 
                //{
                //    { "Authorization", "Bearer <API_TOKEN>" },
                //},
                Content = new StringContent(message)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
        }
    }
}
