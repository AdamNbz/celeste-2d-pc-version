using UnityEngine;
[CreateAssetMenu(fileName = "StaticChaptersData", menuName = "StaticChaptersData")]
//static data
public class StaticChaptersData : ScriptableObject
{
    [Header("Static Data for Chapter")]
    [SerializeField] string name;
    [SerializeField] int builtIndex;
    [SerializeField] int maxStrawberry;
    [Header("Default Settings")]
    [SerializeField] Vector2 defaultPlayerPos;
    public string ChapterName => name;
    public int BuiltIndex => builtIndex;
    public int MaxStrawberry => maxStrawberry;
    public Vector2 DefaultPlayerPos => defaultPlayerPos;
}
