using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Header("SaveSlot")]
    SaveSlot currentSaveSlot= new SaveSlot(1);
    [Header("Player Manager")]
    PlayerController player;

    private void Awake()
    {
        if (__instance == null)
        {
            __instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        currentSaveSlot.LoadFromSaveFile();
        player = FindAnyObjectByType<PlayerController>();
        player.SetPlayerData(currentSaveSlot.PlayerData);
        SceneManager.LoadScene(StaticChaptersDataManager.Instance.GetStaticChaptersData(player.GetPlayerData().GetStage()).BuiltIndex);
        Debug.Log(StaticChaptersDataManager.Instance.GetStaticChaptersData(player.GetPlayerData().GetStage()).BuiltIndex);
    }

    public SaveSlot GetCurrentSaveSlot()
    {
        return currentSaveSlot;
    }
}
