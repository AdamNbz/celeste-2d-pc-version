using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Assets.Script.SaveData
{
    [System.Serializable]
    public class ChapterData
    {
        float timetofinish = -1;
        //Strawberry Go Here
        int Death=0;

        public void IncreasePlayTime(float deltaTime)
        {
            if(timetofinish ==-1)
            {
                timetofinish = deltaTime;
            }
            else
            {
                timetofinish += deltaTime;
            }
        }

        public float GetPlayTime()
        {
            return timetofinish;
        }

        public void ResetSession()
        {
            timetofinish = -1;
        }

        public void IncreaseDeath()
        {
            Death++;
        }

        public int DeathCount => Death;

    }

    [CreateAssetMenu(fileName = "StaticChaptersDataManager", menuName = "StaticChaptersDataManager", order = 0)]
    public class StaticChaptersData:ScriptableObject
    {
        [SerializeField] string name;
        [SerializeField] int builtIndex;
        [SerializeField] int maxStrawberry;
    }

    [CreateAssetMenu(fileName ="StaticChaptersDataManager",menuName ="StaticChaptersDataManager",order=0)]
    public class StaticChaptersDataManager : ScriptableObject
    {
        static private StaticChaptersDataManager __instance;
        public static StaticChaptersDataManager Instance
        {
            get
            {
                if(__instance==null)
                {
                    __instance = Resources.Load<StaticChaptersDataManager>("SaveData/StaticChapterData");
                    if (__instance==null)
                    {
                        Debug.Log("File is not exits or not in correct direction");
                    }
                }
                return __instance;
            }
        }
        [SerializeField] Dictionary<string, StaticChaptersData> datas;
        public StaticChaptersData GetStaticChaptersData(string name)
        {
            return datas[name];
        }
    }
}
