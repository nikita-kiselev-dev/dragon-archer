using System;

namespace Core.Initialization.Scripts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ControlEntityOrder : Attribute
    {
        public string ScopeName { get; private set; }
        public int InitOrder { get; private set; }
        
        public ControlEntityOrder(string scopeName, int initOrder = int.MaxValue)
        {
            ScopeName = scopeName;
            InitOrder = initOrder;
        }
    }
}