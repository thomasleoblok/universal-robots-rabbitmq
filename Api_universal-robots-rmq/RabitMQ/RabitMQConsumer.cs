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
using Message = FirebaseAdmin.Messaging.Message;

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
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    //Credential = GoogleCredential.FromFile(@"Firebase/private_key.json")
                    Credential = GoogleCredential.FromFile("private_key.json")
                });
            }
            // This registration token comes from the client FCM SDKs.
            var registrationToken = "fzf1QRMsSa-KEEywrgDytb:APA91bFEuOLfdp8PoNgDVik3a-eQGrUc-luqrMMd_mo9uIwS-T7qi90O3ui9ZkNnYZs5k6doqF09T0eFZCWEBJcuB7x24xCR0yZc4TzTEN9UDe3L5q9yrIJx4OqfJjhgs68zuRZsVyOJ";

            // See documentation on defining a message payload.
            var msg = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "myData", "1337" },
                },
                //Topic = "all",
                Token = registrationToken,
                Notification = new Notification()
                {
                    Title = "hej",
                    Body = "Here is your test!"
                }
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            string response = FirebaseMessaging.DefaultInstance.SendAsync(msg).Result;
            // Response is a message ID string.
            Console.WriteLine("Successfully sent message: " + response);
        }
    }
}
