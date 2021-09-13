using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Saving
{
    public static class SaveSystem
    {
        private static string path = Application.persistentDataPath + "/save.dat";

        public static SaveData SaveData
        {
            get
            {
                return Load();
            }
            set
            {
                Save(value);
            }
        }

        private static void Save(SaveData data)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                GetBinaryFormatter().Serialize(stream, data);

                stream.Close();
            }

            Debug.Log("Data saved.");
        }

        private static SaveData Load()
        {
            if (File.Exists(path))
            {
                SaveData data;

                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    data = GetBinaryFormatter().Deserialize(stream) as SaveData;
                    
                    stream.Close();
                }

                return data;
            }
            else
            {
                Debug.LogWarning($"Save file not found in {path}. Creating default one.");

                var defaultSaveData = new SaveData();
                Save(defaultSaveData);

                return defaultSaveData;
            }
        }

        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter;
        }
    }
}
