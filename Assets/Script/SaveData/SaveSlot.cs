using Assets.Script.SaveData;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class SaveSlot
{
    [SerializeField] int slotID;
    [SerializeField] PlayerData playerData;
    [SerializeField] List<ChapterData> chapterDatas;

    public SaveSlot(int id)
    {
        slotID = id;
        playerData = new PlayerData();
        chapterDatas = new List<ChapterData>();
    }
    public int SlotID => slotID;
    public PlayerData PlayerData => playerData;
    public List<ChapterData> ChapterDatas => chapterDatas;
    public void SetSlotID(int id)
    {
        slotID = id;
    }

    public void SetPlayerData(PlayerData data)
    {
        playerData = data;
    }

    public void SetChapterDatas(List<ChapterData> datas)
    {
        chapterDatas = datas;
    }

    public ChapterData GetChapterDataByName(string name)
    {
        return chapterDatas.Find(chapter => chapter.Name == name);
    }

    public void LoadFromSaveFile()
    {
        string json = SaveSystem.getFileJson(slotID);
        if(json == "")
        {
            Debug.Log("Empty Save File");
            return;
        }
        JsonUtility.FromJsonOverwrite(json, this);
    }

    public void SaveToFile()
    {
        string json = JsonUtility.ToJson(this);
        SaveSystem.SaveToFile(json, slotID);
    }
}
