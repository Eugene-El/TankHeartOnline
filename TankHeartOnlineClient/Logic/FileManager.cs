using FastLogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TankHeartOnlineClient.Logic
{
    public class FileManager
    {
        private static FileManager _fileManager;
        public static FileManager Instance
        {
            get
            {
                if (_fileManager == null)
                    _fileManager = new FileManager();
                return _fileManager;
            }
        }
        private FileManager() { }

        public T ReadFile<T>(string filePath)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    if (fileStream.Length <= 0)
                        return default(T);
                    return (T)formatter.Deserialize(fileStream);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Log(exception.ToString());
                return default(T);
            }
        }

        public void WriteFile<T>(string filePath, T data)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, data);
            }
        }
    }
}
