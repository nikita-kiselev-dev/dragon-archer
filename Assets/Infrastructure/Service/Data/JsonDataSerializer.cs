using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Infrastructure.Service.Data
{
    public class JsonDataSerializer : IDataSerializer
    {
        public Dictionary<string, SaveLoad.Data> Deserialize(string data, Dictionary<string, Type> dataNameToType)
        {
            var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(data);
            var result = new Dictionary<string, SaveLoad.Data>();

            foreach (var (key, value) in deserializedData)
            {
                if (dataNameToType.TryGetValue(key, out var type))
                {
                    var deserializedValue = (SaveLoad.Data)JsonConvert.DeserializeObject(value.ToString(), type);
                    result[key] = deserializedValue;
                }
                else
                {
                    Debug.LogError($"{GetType().Name}: unknown type for key: {key} can't deserialize save file!");
                }
            }

            return result;
        }

        public string Serialize(Dictionary<string, SaveLoad.Data> data)
        {
            var serializedData = data.ToDictionary(kvp => kvp.Key, kvp => JObject.FromObject(kvp.Value));
            return JsonConvert.SerializeObject(serializedData);
        }
    }
}