using NUnit.Framework;
using System;
using System.Collections;
using Unity;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
namespace Assets.Script.SaveData
{
    //Runtime Data
    [System.Serializable]
    public class ChapterData
    {
        [SerializeField] string name;
        [SerializeField] float timetofinish = -1;
        [SerializeField] List<StrawberryData> strawberries = new List<StrawberryData>();
        [SerializeField] int Death=0;
        [SerializeField] int strawberryCollected = 0;

        public void SetStrawberries(List<StrawberryData> data)
        {
            strawberries = data;
        }

        public List<StrawberryData> GetStrawberries()
        {
           return strawberries;
        }

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

        public void SetPlayTime(float time)
        {
            timetofinish = time;
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

        public void CollectStrawberry()
        {
            strawberryCollected++;
        }
        public int StrawberryCollected => strawberryCollected;
        public int DeathCount => Death;
        public string Name => name;
        public void SetName(string name)
        {
            this.name = name;
        }

        public void ReturnToDefault()
        {
            timetofinish = -1;
            strawberryCollected = 0;
            Death = 0;
            strawberries.Clear();
        }
    }
}
