using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static void SaveData(string path, object data)
    {
        BinaryFormatter format = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + (path.StartsWith("/") ? path : "/" + path));
        format.Serialize(file, data);
        file.Close();
    }
    public static T LoadData<T>(string path)
    {
        string filename = path.StartsWith("/") ? path : "/" + path;
        if (File.Exists(Application.persistentDataPath + filename))
        {
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            try
            {
                BinaryFormatter format = new BinaryFormatter();
                
                T data = (T)format.Deserialize(file);
                file.Close();
                return data;
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
                File.Delete(filename);
            }
        }
        return System.Activator.CreateInstance<T>();
    }
}
