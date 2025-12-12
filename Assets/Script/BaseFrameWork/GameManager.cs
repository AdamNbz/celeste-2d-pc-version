using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager Settings")]
    private static GameManager __instance;
    public static GameManager GetInstance()
    {
        if (__instance == null)
        {
            __instance = FindAnyObjectByType<GameManager>();
            if (__instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                __instance = obj.AddComponent<GameManager>();
            }
            else
            {
                DontDestroyOnLoad(__instance.gameObject);
            }
        }
        return __instance;
    }

    [Header("Player Manager")]
    GameObject player;
    [Header("Enermy Manager")]
    GameObject enermy;
}
