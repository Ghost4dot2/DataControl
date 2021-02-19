using System;

namespace DataControl
{
    public class Customer : DB_Object
    {
        public String CompanyName { get; set; }
        public String Name { get; set; }
        public String Title { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Region { get; set; }
        public String PostalCode { get; set; }
        public String Country { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }

        public override void debug()
        {
            Console.WriteLine("ID: " + ID + "\n" +
                "Name: " + Title + Name + "\n" +
                "Company: " + CompanyName + "\n" +
                "Address: " + Address + "\n" +
                "\t" + City + ", " + PostalCode + "\n" +
                "\t" + Country + ", " + Region + "\n" +
                "Phone: " + Phone + "\n" +
                "Fax: " + Fax + "\n"
                );
        }
    }
}
