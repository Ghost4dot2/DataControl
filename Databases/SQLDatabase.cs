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

        public Dictionary<int, string> getNames(String Table)
        {
            // String connString = "Data Source=" + dataSource + "; Initial Catalog=" + databaseName +
            //    ";User ID=" + username + ";Password=" + password;
            Dictionary<int, string> employeeNameID = new Dictionary<int, string>();

            using var connection = new SqlConnection(connString);

            connection.Open();

            //read from database
            var query = connection.CreateCommand();
            query.CommandText = "SELECT * FROM " + "Employees";
            query.CommandType = CommandType.Text;

            using (var reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader["FirstName"] + " " + reader["LastName"];
                    employeeNameID.Add((int) reader["EmployeeID"], name);

                    //Console.WriteLine($"The Emplyee name is {reader["FirstName"]} {reader["LastName"]}");
                }
            }

            connection.Close();

            return employeeNameID;
        }

        public Employee getEmployeeByID(string ID)
        {
            Employee identified = new Employee();

            using var connection = new SqlConnection(connString);

            connection.Open();

            //read from database
            var query = connection.CreateCommand();
            query.CommandText = "SELECT * FROM " + "Employees WHERE EmployeeID=" + ID ;
            query.CommandType = CommandType.Text;

            try
            {
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
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
                connection.Close();
            }

            return identified;
        }

        public List<Employee> getEmployees()
        {
            List<Employee> list = new List<Employee>();

            using var connection = new SqlConnection(connString);

            connection.Open();

            //read from database
            var query = connection.CreateCommand();
            query.CommandText = "SELECT * FROM " + "Employees";
            query.CommandType = CommandType.Text;

            try
            {
                using (var reader = query.ExecuteReader())
                {
                    while (reader.Read())
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
                        list.Add(identified);
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
                connection.Close();
            }

            return list;
        }

        public void addEmployee(Employee employee)
        {

            excuteSQLCommand(employee.sql_Insert("Employees"), $"Failed to insert new Employee ({employee.ID}) into table: Employees");
        }

        public void updateEmployee(Employee employee)
        {
            excuteSQLCommand(employee.sql_Update("Employees"), $"Failed to update Employee: {employee.ID}");
        }

        public void removeEmployeeByID(string ID)
        {
            excuteSQLCommand($"DELETE FROM Employees WHERE EmployeeID={ID}", $"Failed to delete Employee: {ID} from table");
        }

        private void excuteSQLCommand(string commandString, string failureLog = "")
        {
            using var connection = new SqlConnection(connString);

            connection.Open();
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
                connection.Close();
            }

        }
    }
}
