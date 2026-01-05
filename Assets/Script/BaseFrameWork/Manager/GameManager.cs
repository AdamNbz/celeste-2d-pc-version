using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    [Header("Game Manager Settings")]
    [SerializeField]private PlayerController playerPrefab;
    [SerializeField] InputAction reload;
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
        reload.Enable();
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
            currentPlayingStatus = PlayingChapterStatus.Playing;
            StartCoroutine(LoadPlayerAndCheckPointAfterLoadScene(currentSaveSlot.PlayerData.GetStage()));
        }
    }

    IEnumerator<WaitUntil> LoadPlayerAndCheckPointAfterLoadScene(string SceneName)
    {
        if(SceneManager.GetActiveScene().buildIndex != StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex)
            SceneManager.LoadScene(StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex);
        yield return new WaitUntil(()=>SceneManager.GetActiveScene().buildIndex== StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex);
        GetCheckPoints();
        SpawnPlayerAtCheckPoint();
    }

    IEnumerator<WaitForSeconds> SpawnPlayerAfterDelayCoroutine(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        SpawnPlayerAtCheckPoint();
    }

    IEnumerator<WaitUntil> SpawnPlayerAfterLoadScene(string SceneName)
    {
        if(SceneManager.GetActiveScene().buildIndex != StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex)
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
    }

    public void SpawnPlayerAtCheckPoint()
    {
        if(player!=null)
        {
            player.gameObject.name = "OldPlayerController";
            Destroy(player.gameObject);
        }
        player=Instantiate(playerPrefab);
        player.SetPlayerData(currentSaveSlot.PlayerData);
        SceneManager.MoveGameObjectToScene(player.gameObject, SceneManager.GetActiveScene());
        if (player.GetPlayerData().GetCheckpoint() != "")
        {
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
    //If go to menu,Save Game
    public void ChangeScene(string newSceneName)
    {
        currentSaveSlot.PlayerData.SetStage(newSceneName);
        currentSaveSlot.PlayerData.SetCheckpoint("");
        SaveSlot(currentSaveSlot.SlotID);
        StartCoroutine(SpawnPlayerAfterLoadScene(currentSaveSlot.PlayerData.GetStage()));
        currentPlayingStatus = PlayingChapterStatus.Playing;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SaveSlot(currentSaveSlot.SlotID);
    }
    public enum PlayingChapterStatus
    {
        ChapterEnding,
        ChapterComplete,
        Playing
    }
    PlayingChapterStatus currentPlayingStatus;

    public PlayingChapterStatus GetCurrentPlayingStatus()
    {
        return currentPlayingStatus;
    }

    public IEnumerator<WaitForSeconds> ChangeSceneAfterADelay(string newSceneName,float delaytime)
    {
        yield return new WaitForSeconds(delaytime);
        ChangeScene(newSceneName);
    }

    public void OnChapterEnding(string nextSceneName)
    {
        if (currentPlayingStatus!=PlayingChapterStatus.Playing)
        {
            return;
        }
        else
        {
            currentPlayingStatus = PlayingChapterStatus.ChapterEnding;
            player.DisableInput();
            Invoke("ChapterEnded", 1.5f);
            StartCoroutine(ChangeSceneAfterADelay(nextSceneName, 3f));
        }
    }
    private void FixedUpdate()
    {
        if (currentPlayingStatus==PlayingChapterStatus.ChapterEnding)
        {
        }
        if (reload.IsPressed())
        {
            StartCoroutine(ReloadScene());
        }
    }

    IEnumerator<WaitForNextFrameUnit> ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForNextFrameUnit();
        SpawnPlayerAtCheckPoint();
        reload.Enable();
    }

    public void ChapterEnded()
    {
        currentPlayingStatus = PlayingChapterStatus.ChapterComplete;
        Debug.Log("ChapterComplete");
    }

    public void SpawnPlayerAfterADelay(float timeDelay)
    {
        StartCoroutine(SpawnPlayerAfterDelayCoroutine(timeDelay));
    }

    public void OnPlayerDeath()
    {
        if(currentPlayingStatus!= PlayingChapterStatus.Playing)
        {
            return;
        }
        player.SetState(new Player_State.Death(player));
        GameManager.GetInstance().SpawnPlayerAfterADelay(2f);
        StartCoroutine(SpawnPlayerAfterDelayCoroutine(2f));
    }
}
