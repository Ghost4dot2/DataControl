using DataControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task<Dictionary<int, string>> getNames(String Table)
        {
            // String connString = "Data Source=" + dataSource + "; Initial Catalog=" + databaseName +
            //    ";User ID=" + username + ";Password=" + password;
            Dictionary<int, string> employeeNameID = new Dictionary<int, string>();

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
                    employeeNameID.Add((int) reader["EmployeeID"], name);

                    //Console.WriteLine($"The Emplyee name is {reader["FirstName"]} {reader["LastName"]}");
                }
            }

            await connection.CloseAsync();

            return employeeNameID;
        }

        public async Task<Employee> getEmployeeByID(string ID)
        {
            Employee identified = new Employee();

            await using var connection = new SqlConnection("Data Source=localhost;Initial Catalog=Northwind;User ID=SA;Password=AStupidPassword1@");

            await connection.OpenAsync();

            //read from database
            var query = connection.CreateCommand();
            query.CommandText = "SELECT * FROM " + "Employees WHERE EmployeeID=" + ID ;
            query.CommandType = CommandType.Text;

            try
            {
                await using (var reader = await query.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        

                        identified.ID = ((int)reader["EmployeeID"]).ToString();
                        identified.firstName = (reader["FirstName"] as string) ?? "";
                        identified.lastName = (reader["LastName"] as string) ?? "";
                        identified.title = (reader["Title"] as string) ?? "";
                        identified.titleOfCourtesy = (reader["TitleOfCourtesy"] as string) ?? "";
                        identified.address = (reader["Address"] as string) ?? "";
                        identified.city = (reader["City"] as string) ?? "";
                        identified.region = (reader["Region"] as string) ?? "";
                        identified.postalCode = (reader["PostalCode"] as string) ?? "";
                        identified.country = (reader["Country"] as string) ?? "";
                        identified.phone = (reader["HomePhone"] as string) ?? "";
                        identified.extension = (reader["Extension"] as string) ?? "";
                        identified.notes = (reader["Notes"] as string) ?? "";

                        identified.debug();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Failed to get employee with ID: {ID}");
            }
            finally
            {
                await connection.CloseAsync();
            }

            return identified;
        }

        public async Task<List<Employee>> getEmployees()
        {
            List<Employee> list = new List<Employee>();

            await using var connection = new SqlConnection("Data Source=localhost;Initial Catalog=Northwind;User ID=SA;Password=AStupidPassword1@");

            await connection.OpenAsync();

            //read from database
            var query = connection.CreateCommand();
            query.CommandText = "SELECT * FROM " + "Employees";
            query.CommandType = CommandType.Text;

            try
            {
                await using (var reader = await query.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Employee identified = new Employee();

                        identified.ID = ((int)reader["EmployeeID"]).ToString();
                        identified.firstName = (reader["FirstName"] as string) ?? "";
                        identified.lastName = (reader["LastName"] as string) ?? "";
                        identified.title = (reader["Title"] as string) ?? "";
                        identified.titleOfCourtesy = (reader["TitleOfCourtesy"] as string) ?? "";
                        identified.address = (reader["Address"] as string) ?? "";
                        identified.city = (reader["City"] as string) ?? "";
                        identified.region = (reader["Region"] as string) ?? "";
                        identified.postalCode = (reader["PostalCode"] as string) ?? "";
                        identified.country = (reader["Country"] as string) ?? "";
                        identified.phone = (reader["HomePhone"] as string) ?? "";
                        identified.extension = (reader["Extension"] as string) ?? "";
                        identified.notes = (reader["Notes"] as string) ?? "";

                        //identified.debug();
                        list.Append(identified);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Failed to get all employees");
            }
            finally
            {
                await connection.CloseAsync();
            }

            return list;
        }

        public async Task addEmployee(Employee employee)
        {

            await excuteSQLCommand(employee.sql_Insert("Employees"), $"Failed to insert new Employee ({employee.ID}) into table: Employees");
        }

        public async Task updateEmployee(Employee employee)
        {
            await excuteSQLCommand(employee.sql_Update("Employees"), $"Failed to update Employee: {employee.ID}");
        }

        public async Task removeEmployeeByID(string ID)
        {
            await excuteSQLCommand($"DELETE FROM Employees WHERE EmployeeID={ID}", $"Failed to delete Employee: {ID} from table");
        }

        private async Task excuteSQLCommand(string commandString, string failureLog = "")
        {
            await using var connection = new SqlConnection(connString);

            await connection.OpenAsync();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = commandString;
            command.CommandType = CommandType.Text;

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if(failureLog != "")
                {
                    Console.WriteLine(failureLog);
                }
                if(commandString != "")
                {
                    Console.WriteLine(commandString);
                }
                
            }
            finally
            {
                await connection.CloseAsync();
            }

        }
    }
}
