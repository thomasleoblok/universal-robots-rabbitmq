using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using MessageModel;
using System.Text.Json;

class RobotMessaging
{
    private static Random rnd = new Random();
    private static string[] descriptions = {
        "Overheating", "Reboot required", "Not responding",
        "Battery running low", "Robot gone rogue", "Task failed succesfully",
        "Program has encountered an error which should not have occurred", "Virus detected", "Goat gone wild"
    };

    public static void Main(string[] args)
    {
        // Opsætning af messaging miljøet
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        {
            using var channel = connection.CreateModel();
            {
                channel.ExchangeDeclare(exchange: "robot_messages",
                                        type: "topic");

                Console.WriteLine("Press ESC to stop");

                do
                {
                    while (!Console.KeyAvailable)
                    {
                        Thread.Sleep(rnd.Next(5000, 15000));
                        // Klargører channel samt message
                        var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
                        var message = (args.Length > 1)
                                      ? string.Join(" ", args.Skip(1).ToArray())
                                      : JsonSerializer.Serialize(new Message
                                      {
                                          Id = new Guid(),
                                          RobotId = rnd.Next(1, 4),
                                          Created = DateTime.Now,
                                          Description = descriptions[rnd.Next(0, descriptions.Length + 1)],
                                          State = (WarningState)rnd.Next(1, 4)
                                      });
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "robot_messages",
                                             routingKey: routingKey,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            }
        }
    }
}