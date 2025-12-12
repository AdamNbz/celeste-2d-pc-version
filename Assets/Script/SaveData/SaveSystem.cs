using UnityEditor;
using UnityEngine;

public static class DataConverter:SerializedObject
{
    public static TSAVE SOtoSaveData<TSAVE,TSO>(TSO source)
        where TSAVE:class,new()
        where TSO:class
    {

    }
}
