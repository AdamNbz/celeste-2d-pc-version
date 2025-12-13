using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class SwitchingObjectManager : MonoBehaviour
{
    static SwitchingObjectManager _instance;
    [SerializeField] float switchCooldown = 1.0f;
    float Timer = 0.0f;
    public static SwitchingObjectManager GetInstance()
    {
        if (_instance != null) return _instance;
        _instance = FindAnyObjectByType<SwitchingObjectManager>();
        DontDestroyOnLoad(_instance);
        if (_instance == null)
        {
            var obj = new GameObject("SwitchingObjectManager");
            _instance = obj.AddComponent<SwitchingObjectManager>();
            DontDestroyOnLoad(obj);
        }
        return _instance;
    }
    private void Start()
    {
        SaveSystem.GetBaseSaveFilePath();
        Debug.Log(SaveSystem.GetBaseSaveFilePath());
        _instance =GetInstance();
        var instances = FindObjectsByType<SwitchingObjectManager>(FindObjectsSortMode.None);
        for (int i = 0; i < instances.Length; i++)
        {
            if (instances[i] != _instance)
            {
                Destroy(instances[i].gameObject);
            }
        }
        objList.Clear();
        var objs = FindObjectsByType<SwitchingObject>(FindObjectsSortMode.None);
        foreach (var obj in objs)
        {
            objList.Add(obj);
        }
        SwitchMode();
        Timer = switchCooldown;
    }
    enum SwitchingMode
    {
        Pink,
        Blue
    }
    SwitchingMode mode= SwitchingMode.Pink;
    List<SwitchingObject> objList = new List<SwitchingObject>();
    public void SwitchMode()
    {
        if (mode == SwitchingMode.Pink)
        {
            mode = SwitchingMode.Blue;
            for (int i = 0; i < objList.Count; i++)
            {
                if (objList[i] is BlueSwitchingObject blueObj)
                {
                    blueObj.SwitchOn();
                }
                else if (objList[i] is PinkSwitchingObject pinkObj)
                {
                    pinkObj.SwitchOff();
                }
            }
        }
        else
        {
            mode = SwitchingMode.Pink;
            for (int i = 0; i < objList.Count; i++)
            {
                if (objList[i] is BlueSwitchingObject blueObj)
                {
                    blueObj.SwitchOff();
                }
                else if (objList[i] is PinkSwitchingObject pinkObj)
                {
                    pinkObj.SwitchOn();
                }
            }
        }
    }

    private void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0.0f)
        {
            SwitchMode();
            Timer = switchCooldown;
        }
    }
}
