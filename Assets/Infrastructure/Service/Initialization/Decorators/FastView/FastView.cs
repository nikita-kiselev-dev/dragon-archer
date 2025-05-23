﻿using System;

namespace Infrastructure.Service.Initialization.Decorators.FastView
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FastView : Attribute
    {
        public string ViewKey { get; private set; }
        public string ViewType  {get; private set; }
        
        public FastView(string viewKey, string viewType)
        {
            ViewKey = viewKey;
            ViewType = viewType;
        }
    }
}