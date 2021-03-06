﻿using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;
using Databases;
using Newtonsoft.Json;

namespace DataControl
{
    class RabbitMQServer
    {
        public static void runServer()
        {

            SQLDatabase myDatabase = new SQLDatabase("localhost", "Northwind", "SA", "AStupidPassword1@");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue", durable: false,
                    exclusive: false, autoDelete: false, arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                    autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = "";

                    var body = ea.Body.ToArray();
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [.] " + message);
                        //determineAction(message, myDatabase);
                        response = determineAction(message, myDatabase);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "Data Control Error";
                    }
                    finally
                    {
                        Console.WriteLine(" [*] " + response);
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                            basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                            multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static string determineAction(string inMessage, SQLDatabase database)
        {
            string[] words = inMessage.Split("#");

            string action = words[0];
            string message = words[1];
            string response = "";

            switch (action)
            {
                case "Add":
                    addToDatabase(message, database);
                    response = "Received#Add";
                    break;
                case "Remove":
                    removeFromDatabase(message, database);
                    response = "Received#Remove";
                    break;
                case "Update":
                    updateDatabase(message, database);
                    response = "Received#Update";
                    break;
                case "List":
                    response = "List#" + getListFromDatabase(database);
                    break;
                case "Get":
                    response = "Get#" + getFromDatabase(message, database);
                    break;
            }


            return response;
        }

        private static void addToDatabase(string employeeJson, SQLDatabase database)
        {
            Employee newEmployee = JsonConvert.DeserializeObject<Employee>(employeeJson);
            database.addEmployee(newEmployee);
        }

        private static void updateDatabase(string employeeJson, SQLDatabase database)
        {
            Employee newEmployee = JsonConvert.DeserializeObject<Employee>(employeeJson);
            database.updateEmployee(newEmployee);
        }

        private static void removeFromDatabase(string id, SQLDatabase database)
        {
            database.removeEmployeeByID(id);
        }

        private static string getFromDatabase(string id, SQLDatabase database)
        {
            Employee temp = database.getEmployeeByID(id);
            string response = JsonConvert.SerializeObject(temp);
            return response;
        }

        private static string getListFromDatabase(SQLDatabase database)
        {
            string response = "";
            List<Employee> employees = database.getEmployees();
            response = JsonConvert.SerializeObject(employees);
            return response;
        }
    }


}