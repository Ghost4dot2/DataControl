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
            */

            Dictionary<int, string> employeeNameID = myDatabase.getNames("Employees");
            foreach (KeyValuePair<int, string> entry in employeeNameID)
            {
                Console.WriteLine($"The Employee name is {entry.Value} {entry.Key}");
            }
            

            RabbitMQServer.runServer();


        }

    }
}
