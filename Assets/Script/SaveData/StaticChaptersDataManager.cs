using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "StaticChaptersDataManager", menuName = "StaticChaptersDataManager")]
public class StaticChaptersDataManager : ScriptableObject
{
    static private StaticChaptersDataManager __instance;
    public static StaticChaptersDataManager Instance
    {
        get
        {
            if (__instance == null)
            {
                __instance = Resources.Load<StaticChaptersDataManager>("StaticChaptersDataManager");
                if (__instance == null)
                {
                    Debug.Log("File is not exits or not in correct direction");
                }
            }
            return __instance;
        }
    }
    [SerializeField] List<StaticChaptersData> datas;
    public StaticChaptersData GetStaticChaptersData(string name)
    {
        foreach(StaticChaptersData x in datas)
        {
            if (x.ChapterName==name)
            {
                return x;
            }
        }
        Debug.Log("Cannot Load Chapter Data");
        return null;
    }

    public List<StaticChaptersData> GetChaptersDatas()
    {
        return datas;
    }
}
