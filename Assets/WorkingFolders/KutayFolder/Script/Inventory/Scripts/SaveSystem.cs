using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[System.Serializable]
public class SaveSystem
{
    public static SaveSystem Instance;
    
    public (int, int)[] inventory;
    
    public int equippedItem = -1;

    public static void Save()
    {

        string path = Application.persistentDataPath + "/save.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.OpenOrCreate);
        bf.Serialize(file, Instance);
        file.Close();
        
    }

    public static void Load()
    {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            SaveSystem data = (SaveSystem)bf.Deserialize(file);

            file.Close();
            Instance = data;
        }
        else
        {
            Instance = new SaveSystem();
          
            Save();
        }
    }

#if UNITY_EDITOR

    [MenuItem("SaveSystem/Clear Save File")]
    private static void ClearSave()
    {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path))
        {
            File.Delete(path);

        }
    }
     
#endif   
    
}