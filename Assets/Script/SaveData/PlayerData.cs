using UnityEditor;
using UnityEngine;
using System.Collections;
namespace Assets.Script.SaveData
{
    [System.Serializable]
    public class PlayerData
    {
        [Header("Position")]
        string currentcheckpoint;
        string currentstage;

        [Header("Progress")]
        string SaveId;
    }
}