using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Ecosim
{
    public class JsonQuaternionConverter : JsonConverter<Quaternion>
    {
        public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WritePropertyName("w");
            writer.WriteValue(value.w);
            writer.WriteEndObject();
        }

        public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = Newtonsoft.Json.Linq.JObject.Load(reader);
    
            var x = (float)(obj["x"] ?? 0);
            var y = (float)(obj["y"] ?? 0);
            var z = (float)(obj["z"] ?? 0);
            var w = (float)(obj["w"] ?? 0);
    
            return new Quaternion(x, y, z, w);
        }
    }
}
