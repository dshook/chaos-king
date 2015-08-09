using System;

namespace Player
{
    [AttributeUsage(System.AttributeTargets.Class)]
    class Id : System.Attribute
    {
        public int id;

        public Id(int id)
        {
            this.id = id;
        }
    }
}
