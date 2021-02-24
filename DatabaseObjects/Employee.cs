using System;

namespace DataControl
{
    public class Employee : DB_Object
    {
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String title { get; set; }
        public String titleOfCourtesy { get; set; }
        public String address { get; set; }
        public String city { get; set; }
        public String region { get; set; }
        public String postalCode { get; set; }
        public String country { get; set; }
        public String phone { get; set; }
        public String extension { get; set; }
        public String notes { get; set; }

        public override String sql_Update(String table)
        {
            return "UPDATE " + table + "\n" +
                "SET LastName='" + lastName + "', FirstName='" + firstName + "', Title= '" + title + "', TitleOfCourtesy = '" + titleOfCourtesy + "', Address = '" + address + "', City = '" + city + "', Region = '" + region + "', PostalCode = '" + postalCode + "', Country = '" + country + "', HomePhone = '" + phone + "', Extension = '" + extension + "', Notes = '" + notes + "'\n" +
                "WHERE EmployeeID = " + ID + ";";
        }

        public override String sql_Insert(String table)
        {
            return "INSERT INTO " + table + "(LastName, FirstName, Title, TitleOfCourtesy, Address, City, Region, PostalCode, Country, HomePhone, Extension, Notes)\n" +
                "VALUES ('" + lastName + "', '" + firstName + "', '" + title + "', '" + titleOfCourtesy + "', '" + address + "', '" + city + "', '" + region + "', " + postalCode + ", '" + country + "', '" + phone + "', " + extension + ", '" +  notes + "');";
        }

        public override void debug()
        {
            string tempAddress = address.Replace("\r\n", " ");

            Console.WriteLine("ID: " + ID + "\n" +
                "Name: " + title +" " + firstName + " " + " " + lastName + " (" + titleOfCourtesy + ")" + "\n" +
                "Address: " + tempAddress + ", " + city + " " + country + ", " + postalCode + ", " + region + "\n" +
                "Phone: " + phone + "(" + extension + ")" + "\n"
                );
        }
    }
}
