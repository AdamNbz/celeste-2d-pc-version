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
        
    }

    private void Start()
    {
        currentSaveSlot.LoadFromSaveFile();
        player = FindAnyObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
            return;
        }
        if (currentSaveSlot==null)
        {
            Debug.Log("Save slot null");
        }
        player.SetPlayerData(currentSaveSlot.PlayerData);
        SceneManager.LoadScene(StaticChaptersDataManager.Instance.GetStaticChaptersData(player.GetPlayerData().GetStage()).BuiltIndex);
    }

    public SaveSlot GetCurrentSaveSlot()
    {
        return currentSaveSlot;
    }
}
