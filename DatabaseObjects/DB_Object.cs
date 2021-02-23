using System;
using System.Collections.Generic;
using System.Text;

namespace DataControl
{
    public class DB_Object
    {
        public String ID { get; set; }

        public virtual String sql_Overwrite(String table)
        {
            return "";
        }

        public virtual String sql_Insert(String table)
        {
            return "";
        }

        public virtual void debug()
        {
            Console.WriteLine(ID);
        }
    }
}
