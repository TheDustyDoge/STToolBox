namespace STToolBox.Database
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    [Serializable]
    public class SerializableDictionary : ISerializationCallbackReceiver
    {
        string[] keys;
        ISerializedObject[] values;

        [NonSerialized]
        public Dictionary<string, ISerializedObject> dictionary;

        public SerializableDictionary()
        {
            dictionary = new Dictionary<string, ISerializedObject>();
        }

        public void AddValueWithKey(string key, ISerializedObject value)
        {
            dictionary.Add(key, value);
        }

        public bool RemoveValueWithKey(string key)
        {
            return dictionary.Remove(key);
        }

        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool ContainsValue(ISerializedObject value)
        {
            return dictionary.ContainsValue(value);
        }

        // ==================== \\

        public void OnBeforeSerialize()
        {

            keys = new string[dictionary.Count];
            values = new ISerializedObject[dictionary.Count];

            int i = 0;
            foreach (KeyValuePair<string, ISerializedObject> pair in dictionary)
            {
                keys[i] = pair.Key;
                values[i] = pair.Value;
                i++;
            }
        }

        public void OnAfterDeserialize()
        {
            dictionary = new Dictionary<string, ISerializedObject>();

            for (int i = 0; i < keys.Length; i++)
            {
                dictionary.Add(keys[i], values[i]);
            }
        }
    }
}
