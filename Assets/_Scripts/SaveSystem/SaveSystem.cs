using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    const string SAVES_PATH = "/saves";
    public static readonly string PATH = "/saves";

    public static void Save<T>(T obj, string key)
    {
        string path = Application.persistentDataPath + SAVES_PATH;
        string file = path + "/" + key + ".json";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string json = JsonUtility.ToJson(obj, true);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(file, FileMode.Create);
        formatter.Serialize(stream, json);
        stream.Close();

        Debug.Log("Save completed at path : " + file);
    }

    public static void Load<T>(ref T obj, string key)
    {
        string path = Application.persistentDataPath + SAVES_PATH;
        string file = path + "/" + key + ".json";
        if (!File.Exists(file))
        {
            Debug.LogWarning("Save not found : " + file);
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = File.Open(file, FileMode.Open);
        JsonUtility.FromJsonOverwrite((string)bf.Deserialize(stream), obj);
        stream.Close();
    }

    public static bool SaveExists(string key)
    {
        string path = Application.persistentDataPath + SAVES_PATH;
        return File.Exists(path + "/" + key + ".json");
    }
}
