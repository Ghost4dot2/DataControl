using DataControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Databases
{
    public class SQLDatabase : Database
    {
        private String connString;

        public SQLDatabase(String dataSource, String databaseName, String username, String password)
        {
            //"data source=DESKTOP-KRAVNQC\\SQLEXPRESS; database=Northwind; integrated security=SSPI"
            connString = "Data Source=" + dataSource + "; Initial Catalog=" + databaseName +
                ";User ID=" + username + ";Password=" + password;

        }

        public void add(DB_Object newItem, bool saveAfterAdd = true)
        {
            throw new NotImplementedException();
        }

        public T getObject<T>(string ID)
        {
            throw new NotImplementedException();
        }

        public void remove(string ID)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<String, int>> getNames(String Table)
        {
            // String connString = "Data Source=" + dataSource + "; Initial Catalog=" + databaseName +
            //    ";User ID=" + username + ";Password=" + password;
            Dictionary<String, int> employeeNameID = new Dictionary<string, int>();

            await using var connection = new SqlConnection("Data Source=localhost;Initial Catalog=Northwind;User ID=SA;Password=AStupidPassword1@");

            await connection.OpenAsync();

            //read from database
            var query = connection.CreateCommand();
            query.CommandText = "SELECT * FROM " + "Employees";
            query.CommandType = CommandType.Text;

            await using (var reader = await query.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string name = reader["FirstName"] + " " + reader["LastName"];
                    employeeNameID.Add(name, (int) reader["EmployeeID"]);

                    //Console.WriteLine($"The Emplyee name is {reader["FirstName"]} {reader["LastName"]}");
                }
            }

            return employeeNameID;
        }

         

    }
}
