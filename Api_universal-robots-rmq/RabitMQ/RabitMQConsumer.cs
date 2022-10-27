using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MessageModel;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using System.Net;

namespace Api_universal_robots_rmq.RabitMQ
{
    public class RabitMQConsumer : IRabitMQConsumer
    {
        IFirebaseClient client;


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
            // This registration token comes from the client FCM SDKs.
            var registrationToken = "BCXv3Slw4-aTkAb7lZQ9bzuX2JFZI3m4zJUpzDob3vrw4lKVmVyxY3HJV-9IcshQCbt7WWR94xBLR-xYum8iG6s";

            // See documentation on defining a message payload.
            var msg = new FirebaseAdmin.Messaging.Message()
            {
                Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(message),
                Token = registrationToken,
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(msg);
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }
    }
}
