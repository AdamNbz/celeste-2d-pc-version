using System.IO;
using UnityEngine;

public static class SaveSystem 
{
    static string persistancePath = "./";
    const string saveFile = "save";
    public static string GetBaseSaveFilePath()
    {
        Debug.Log(Application.persistentDataPath);
        if(!Directory.Exists(Path.Combine(persistancePath, saveFile)))
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(persistancePath, saveFile));
                Debug.Log("Create save file success");
            }
            catch
            {
                Debug.Log($"Failed to create directory at {Path.Combine(persistancePath, saveFile)}");
            }
        }
        return Path.Combine(persistancePath, saveFile);
    }

    public static void SaveToFile(string json,int saveslot)
    {
        string baseSaveFile = GetBaseSaveFilePath();
        string filename ="save" + "0" + saveslot.ToString();
        string filepath = Path.Combine(baseSaveFile, filename);
        File.WriteAllText(filepath,json);
    }

    public static string getFileJson(int saveslot)
    {
        string baseSaveFile = GetBaseSaveFilePath();
        string filename = "save" + "0" + saveslot.ToString();
        string filepath = Path.Combine(baseSaveFile, filename);
        return File.ReadAllText(filepath);
    }
}
