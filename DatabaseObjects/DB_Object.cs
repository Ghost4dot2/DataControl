using System;
using System.Collections.Generic;
using System.Text;

namespace DataControl
{
    public class DB_Object
    {
        public String ID { get; set; }

        public virtual void debug()
        {
            Console.WriteLine(ID);
        }
    }
}
