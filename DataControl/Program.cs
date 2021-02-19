using Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DataControl
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SQLDatabase myDatabase = new SQLDatabase("localhost", "Northwind", "SA", "AStupidPassword1@");
            Dictionary<String, int> employeeNameID = await myDatabase.getNames("Employees");

            foreach(KeyValuePair<String, int> entry in employeeNameID)
            {
                Console.WriteLine($"The Emplyee name is {entry.Value} {entry.Key}");
            }

            /*
            await using var connection = new SqlConnection("data source=localhost; Database=Northwind; User ID=SA;Password=AStupidPassword1@");

            await connection.OpenAsync();

            //read from database
            var query = connection.CreateCommand();
            query.CommandText = "SELECT * FROM Employees";
            query.CommandType = CommandType.Text;

            await using (var reader = await query.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine($"The Emplyee name is {reader["FirstName"]} {reader["LastName"]}");
                }
            }
            */

        }
        

    }
}
