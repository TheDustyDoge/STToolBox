using System.Runtime.Serialization;

namespace STToolBox.Database
{
    public class DatabaseEntry
    {
        public string fileName = "DefaultName";
        public string fileExt = ".dat";
        public string pathWithinDatabase;
        public string fullFileName { get { return fileName + fileExt; } }
        public string fullPathWithinDatabase { get { return pathWithinDatabase + fullFileName; } }

        public SerializableDictionary dictionary;

        public DatabaseEntry(string fileName, string fileExt, string pathWithinDatabase)
        {
            this.fileName = fileName;
            this.fileExt = fileExt;
            this.pathWithinDatabase = pathWithinDatabase;

            Initialize();
        }        

        protected void Initialize()
        {
            dictionary = new SerializableDictionary();
        }

        public void AddDataToEntry(string key, ISerializedObject data)
        {
            dictionary.AddValueWithKey(key, data);
        }

        public bool RemoveDataFromEntry(string key)
        {
            return dictionary.RemoveValueWithKey(key);
        }

        public bool IsSameEntry(DatabaseEntry other)
        {
            return (fileName == other.fileName && fileExt == other.fileExt && pathWithinDatabase == other.pathWithinDatabase);
        }
    }
}
