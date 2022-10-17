﻿using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using MessageModel;

class RobotMessaging
{
    public static void Main(string[] args)
    {
        // Opsætning af messaging miljøet
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "robot_messages",
                                type: "message");

        // Klargører channel samt message
        var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
        var message = (args.Length > 1)
                      ? string.Join(" ", args.Skip(1).ToArray())
                      : "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "robot_messages",
                             routingKey: routingKey,
                             basicProperties: null,
                             body: body);
        Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
    }
}