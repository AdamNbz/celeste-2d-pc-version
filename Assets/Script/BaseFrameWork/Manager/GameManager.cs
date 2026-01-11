using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using JetBrains.Annotations;
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

    public void LoadSlot(int slotID)
    {
        currentSaveSlot = new SaveSlot(slotID);
        if (SaveSystem.getFileJson(slotID) == "")
        {
            currentSaveSlot.PlayerData.SetStage("Prologue");
            ChangeScene("Prologue");
            SaveSlot(slotID);
            return;
        }   
        currentSaveSlot.LoadFromSaveFile();
        
        if (currentSaveSlot.PlayerData.GetStage() == ""|| currentSaveSlot.PlayerData.GetStage() == "MainMenu")
        {
            Debug.Log("MainMenu Dont Spawn Player");
            SceneManager.LoadScene("ChapterSelector");
        }
        else
        {
            currentPlayingStatus = PlayingChapterStatus.Playing;
            StartCoroutine(LoadPlayerAndChapterDataLoadScene(currentSaveSlot.PlayerData.GetStage()));
        }
    }

    IEnumerator<WaitUntil> LoadPlayerAndChapterDataLoadScene(string SceneName)
    {
        if (SceneManager.GetActiveScene().buildIndex != StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex)
            SceneManager.LoadScene(StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == StaticChaptersDataManager.Instance.GetStaticChaptersData(SceneName).BuiltIndex);
        if (SceneName == "MainMenu" || SceneName == "")
        {
            yield break;
        }
        GetCheckPoints();
        //LoadStrawberryData(SceneName);
        SpawnPlayerAtCheckPoint();
    }

    //private void LoadStrawberryData(string SceneName)
    //{
    //    List<Strawberies> allStrawberies = new List<Strawberies>(FindObjectsByType<Strawberies>(FindObjectsSortMode.None));
    //    if (allStrawberies.Count > currentSaveSlot.GetChapterDataByName(SceneName).GetStrawberries().Count || allStrawberies.Count < currentSaveSlot.GetChapterDataByName(SceneName).GetStrawberries().Count)//quatity mismatch,reset data
    //    {
    //        currentSaveSlot.GetChapterDataByName(SceneName).SetStrawberries(new List<StrawberryData>());
    //        List<StrawberryData> tempList = new List<StrawberryData>();
    //        for (int i = 0; i < allStrawberies.Count; i++)
    //        {
    //            StrawberryData temp = new StrawberryData(allStrawberies[i].gameObject.name);
    //            temp.SetCollect(false);
    //            tempList.Add(temp);
    //            allStrawberies[i].SetData(temp);
    //        }
    //        currentSaveSlot.GetChapterDataByName(SceneName).SetStrawberries(tempList);
    //        SaveSlot(currentSaveSlot.SlotID);
    //    }
    //    else//load data
    //    {
    //        foreach (var strawberryData in currentSaveSlot.GetChapterDataByName(SceneName).GetStrawberries())
    //        {
    //            var strawberryObj = allStrawberies.Find(s => s.gameOjbect.name == strawberryData.GetStrawberryID());
    //            if (strawberryObj != null)
    //            {
    //                strawberryObj.SetData(strawberryData);
    //                if (strawberryData.IsCollected())
    //                {
    //                    strawberryObj.gameOjbect.SetActive(false);
    //                }
    //                else
    //                {
    //                    strawberryObj.gameOjbect.SetActive(true);
    //                }
    //            }
    //        }
    //    }
    //}

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
        if(SceneName=="MainMenu"||SceneName=="")
        {
            yield break;
        }
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
        StartCoroutine(LoadPlayerAndChapterDataLoadScene(currentSaveSlot.PlayerData.GetStage()));
        currentPlayingStatus = PlayingChapterStatus.Playing;
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
