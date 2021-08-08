using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Saving
{

    public static class SaveSystem
    {
        private static string path = Application.persistentDataPath + "/save.dat";

        public static void Save(SaveData data)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                GetBinaryFormatter().Serialize(stream, data);

                stream.Close();
            }

            Debug.Log("Data saved.");
        }

        public static SaveData Load()
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
                Debug.LogError($"Save file not found in {path}.");

                return null;
            }
        }

        public static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            return formatter;
        }
    }
}
