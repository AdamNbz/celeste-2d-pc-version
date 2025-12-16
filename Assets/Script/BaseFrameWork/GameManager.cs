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
        player.SetPlayerData(currentSaveSlot.PlayerData);
        if (player == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
            return;
        }
        if (currentSaveSlot == null)
        {
            Debug.Log("Save slot null");
        }
        if (StaticChaptersDataManager.Instance==null)
        {
            Debug.Log("StaticDataNotFound");
        }
        if (StaticChaptersDataManager.Instance.GetStaticChaptersData("Chapter1")==null)
        {
            Debug.Log("Chapter not found");
        }
        if (StaticChaptersDataManager.Instance.GetStaticChaptersData("Chapter1").BuiltIndex != default(int))
            SceneManager.LoadScene(StaticChaptersDataManager.Instance.GetStaticChaptersData("Chapter1").BuiltIndex);
        else
            Debug.Log("BuiltIndex not found");
    }

    public SaveSlot GetCurrentSaveSlot()
    {
        return currentSaveSlot;
    }
}
