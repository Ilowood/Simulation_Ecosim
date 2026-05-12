using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Ecosim
{
    public class JsonSaveService : ISaveService
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto, 
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = { new JsonVector3Converter(), new JsonQuaternionConverter() }
        };

        private string GetFullPath(string fileName) => Path.Combine(Application.persistentDataPath, fileName + ".json");

        public void SaveWorld(WorldSnapshot world, string fileName)
        {
            var path = GetFullPath(fileName);
            var json = JsonConvert.SerializeObject(world, _settings);
            
            File.WriteAllText(path, json);
            Debug.Log($"<b>JsonSaveService:</b> World saved to <color=green>{path}</color>");
        }

        public WorldSnapshot LoadWorld(string fileName)
        {
            var path = GetFullPath(fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"<b>JsonSaveService:</b> Save file '{fileName}' not found at {path}");
                return null;
            }

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<WorldSnapshot>(json, _settings);
        }
    }
}
