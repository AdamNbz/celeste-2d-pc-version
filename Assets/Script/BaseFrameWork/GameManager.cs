using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    [Header("Game Manager Settings")]
    [SerializeField]private PlayerController playerPrefab;
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
                DontDestroyOnLoad(__instance.gameObject);
            }
            else
            {
                DontDestroyOnLoad(__instance.gameObject);
            }
        }
        return __instance;
    }

    [Header("SaveSlot")]
    SaveSlot currentSaveSlot;
    [Header("Player Manager")]
    PlayerController player;
    [Header("CheckPoint")]
    List<CheckPoint> CheckPointsList;
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
        LoadSlot(1);
    }

    public void LoadSlot(int slotID)
    {
        currentSaveSlot = new SaveSlot(1);
        currentSaveSlot.LoadFromSaveFile();
       
        if (currentSaveSlot.PlayerData.GetStage() == "")
        {
            Debug.Log("MainMenu Dont Spawn Player");
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            StartCoroutine(LoadPlayerAndCheckPointAfterLoadScene(currentSaveSlot.PlayerData.GetStage()));

        }
    }

    IEnumerator<WaitUntil> LoadPlayerAndCheckPointAfterLoadScene(string SceneName)
    {
        SceneManager.LoadScene(StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex);
        yield return new WaitUntil(()=>SceneManager.GetActiveScene().buildIndex== StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex);
        GetCheckPoints();
        SpawnPlayerAtCheckPoint();
    }

    IEnumerator<WaitUntil> SpawnPlayerAfterLoadScene(string SceneName)
    {
        SceneManager.LoadScene(StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex);
        SpawnPlayerAtCheckPoint();
    }

    public void SaveSlot(int slotID)
    {
        currentSaveSlot.SaveToFile();
    }

    public SaveSlot GetCurrentSaveSlot()
    {
        return currentSaveSlot;
    }

    public PlayerController GetPlayerController()
    {
        return player;
    }

    public void GetCheckPoints()
    {
        if(CheckPointsList!=null)
        CheckPointsList.Clear();
        CheckPointsList= new List<CheckPoint>(FindObjectsByType<CheckPoint>(FindObjectsSortMode.None));
        CheckPointsList.Sort((a, b) => a.GetIndex().CompareTo(b.GetIndex()));
        for (int i = 0; i < CheckPointsList.Count; i++)
        {
            Debug.Log("CheckPoint Found: " + CheckPointsList[i].gameObject.name);
        }
    }

    public void SpawnPlayerAtCheckPoint()
    {
        if(player!=null)
        {
            Destroy(player.gameObject);
        }
        player=Instantiate(playerPrefab);
        player.SetPlayerData(currentSaveSlot.PlayerData);
        SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetActiveScene());
        if (player.GetPlayerData().GetCheckpoint() != "")
        {
            Debug.Log("Spawn At CheckPoint: " + player.GetPlayerData().GetCheckpoint());
            CheckPoint cp = CheckPointsList.Find(c => c.gameObject.name == player.GetPlayerData().GetCheckpoint());
            if (cp != null)
            {
                foreach (var checkPoint in CheckPointsList)
                {
                    if (checkPoint.GetIndex() <= cp.GetIndex())
                    {
                        checkPoint.ActiveCheckPoint();
                    }
                }
                player.transform.position = cp.transform.position;
            }
            else
            {
                Debug.LogWarning("Checkpoint not found: " + player.GetPlayerData().GetCheckpoint()+"Spawn As Default Pos");
                player.transform.position = StaticChaptersDataManager.Instance.GetStaticChaptersData(currentSaveSlot.PlayerData.GetStage()).DefaultPlayerPos;
            }
        }
        else
        {
            Debug.Log("Spawn As Default Pos");
            player.transform.position = StaticChaptersDataManager.Instance.GetStaticChaptersData(currentSaveSlot.PlayerData.GetStage()).DefaultPlayerPos;
        }
    }
    // If you want to change scene and reset checkpoint
    //If go to menu, reset both stage and checkpoint
    public void ChangeScene(string newSceneName)
    {
        currentSaveSlot.PlayerData.SetStage(newSceneName);
        currentSaveSlot.PlayerData.SetCheckpoint("");
        SaveSlot(currentSaveSlot.SlotID);
        StartCoroutine(SpawnPlayerAfterLoadScene(currentSaveSlot.PlayerData.GetStage()));
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        currentSaveSlot.PlayerData.SetCheckpoint("");
        currentSaveSlot.PlayerData.SetStage("");
        SaveSlot(currentSaveSlot.SlotID);
    }
}
