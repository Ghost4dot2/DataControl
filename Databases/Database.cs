using System;

namespace Databases
{
    interface Database
    {
        // If ID already exists it will replace it
        public void add(DB_Object newItem, bool saveAfterAdd = true);
        public void remove(String ID);
        public T getObject<T>(String ID);
    }
}
