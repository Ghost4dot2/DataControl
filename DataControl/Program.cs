using Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;

namespace DataControl
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SQLDatabase myDatabase = new SQLDatabase("localhost", "Northwind", "SA", "AStupidPassword1@");
            /*
            //Callahan
            Employee temp = await myDatabase.getEmployeeByID("8");
            await myDatabase.removeEmployeeByID("10");


            Dictionary<int, string> employeeNameID = await myDatabase.getNames("Employees");
            foreach (KeyValuePair<int, string> entry in employeeNameID)
            {
                Console.WriteLine($"The Emplyee name is {entry.Value} {entry.Key}");
            }
            */

            /*
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
                    string response = null;

                    var body = ea.Body.ToArray();
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        int n = int.Parse(message);
                        Console.WriteLine(" [.] fib({0})", message);
                        response = fib(n).ToString();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
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
            */

        }
        

        public async Task<string> performAction(string action, SQLDatabase database )
        {
            string returnMessage = "";

            string[] args = action.Split("#");

            switch(args[0])
            {
                case "get":
                    if(args[1] == "ID")
                    {
                        if(args.Length == 3)
                        {
                            Employee temp = await database.getEmployeeByID(args[2]);
                            //translate to json for return to client
                            string json = JsonConvert.SerializeObject(temp);
                            returnMessage = "ID#" + json;
                        }
                        else
                        {
                            //error message for return to client
                            returnMessage = "ID#Error";
                        }

                    }
                    else if(args[1] == "Names")
                    {
                        Dictionary<int, string> temp = await database.getNames("Employees");
                        //translate to json for return to client
                        string json = JsonConvert.SerializeObject(temp);
                        returnMessage = "Names#" + json;

                    }
                    break;
                case "new":
                    //2nd arg is the json for employee
                    Employee newEmployee = JsonConvert.DeserializeObject<Employee>(args[1]);
                    //add employee to database
                    await database.addEmployee(newEmployee);
                    returnMessage = "Recieved#New";
                    break;
                case "update":
                    //2nd arg is the json for employee
                    Employee updateEmployee = JsonConvert.DeserializeObject<Employee>(args[1]);
                    //add employee to database
                    await database.addEmployee(updateEmployee);
                    returnMessage = "Recieved#Update";
                    break;
                default:
                    returnMessage = "Error#Command";
                    break;
                
            }

            return returnMessage;
        }

    }
}
