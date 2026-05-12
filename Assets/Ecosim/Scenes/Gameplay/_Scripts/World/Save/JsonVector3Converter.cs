using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Ecosim
{
    public class JsonVector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WriteEndObject();
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = Newtonsoft.Json.Linq.JObject.Load(reader);
    
            var x = (float)(obj["x"] ?? 0);
            var y = (float)(obj["y"] ?? 0);
            var z = (float)(obj["z"] ?? 0);
    
            return new Vector3(x, y, z);
        }
    }
}
