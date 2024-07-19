using System;
using System.Collections.Generic;

namespace Infrastructure.Service.Data
{
    public interface IDataSerializer
    {
        public Dictionary<string, SaveLoad.Data> Deserialize(string data, Dictionary<string, Type> dataNameToType);
        public string Serialize(Dictionary<string, SaveLoad.Data> data);
    }
}