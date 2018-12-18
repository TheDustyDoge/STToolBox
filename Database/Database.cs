using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace STToolBox.Database
{
    public class Database : MonoBehaviour
    {
        public static Database instance;
        List<DatabaseEntry> databaseEntries;

        [Tooltip("Show the full path within the database rather than just the name.")]
        public bool showFullPath;
        [Tooltip("A readonly list of the names of all the loaded database entries.")]
        public List<string> loadedEntries;

        [Tooltip("The database path in the file system. Defaults to UnityEngine.Application.persistentDataPath.")]
        public string systemPath;
        public string defaultPath { get { return Application.persistentDataPath; } }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitilizeDatabase();
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        void InitilizeDatabase()
        {

        }

        void Start()
        {
            // load?
        }

        // ============== \\
        // = Validation = \\
        // ============== \\

        private void OnValidate()
        {
            if (!File.Exists(systemPath))
            {
                Debug.LogError("Secified path (" + systemPath + ") does not exist!");
                systemPath = defaultPath;
            }
            // TODO: Check permissions

            if (!VerifyFileSystem())
            {
                Debug.Log("Some directorys are still missing from your file system.");
            }

            UpdateLodadedEntries();
        }

        bool VerifyFileSystem(bool createMissing = false)
        {
            return true;
        }

        void UpdateLodadedEntries()
        {
            loadedEntries.Clear();
            for (int i = 0; i < databaseEntries.Count; i++)
            {
                if (showFullPath)
                {
                    loadedEntries.Add(databaseEntries[i].fullPathWithinDatabase);
                }
                else
                {
                    loadedEntries.Add(databaseEntries[i].fullFileName);
                }
            }
        }

        // ================== \\
        // = Global Methods = \\
        // ================== \\

        public static void SaveDatabase()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();


        }

        public static void LoadDatabase()
        {

        }

        public static bool DatabaseContainsEntry(DatabaseEntry entry)
        {
            for (int i = 0; i < instance.databaseEntries.Count; i++)
            {
                if (instance.databaseEntries[i].IsSameEntry(entry))
                {
                    return true;
                }
            }
            return false;
        }

        // ================== \\
        // = Helper Methods = \\
        // ================== \\



    }
}
