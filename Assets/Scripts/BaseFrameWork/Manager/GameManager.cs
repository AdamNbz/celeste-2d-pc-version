using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using JetBrains.Annotations;
public class GameManager : MonoBehaviour
{
    [Header("Game Manager Settings")]
    [SerializeField]private PlayerController playerPrefab;
    [SerializeField] private ParticleSystem deathEffectPrefab;
    private GameObject levelCompleteCanvas;
    private GameObject pauseMenuCanvas;
    [SerializeField] InputAction reload;
    [SerializeField] InputAction pauseAction;
    private static GameManager __instance;
    private bool isPaused = false;
    
    [Header("SaveSlot")]
    SaveSlot currentSaveSlot;
    [Header("Player Manager")]
    PlayerController player;
    [Header("CheckPoint")]
    List<CheckPoint> CheckPointsList;

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


    private void Awake()
    {
        if (__instance == null)
        {
            __instance = this;
            DontDestroyOnLoad(this.gameObject);
            reload.Enable();
            pauseAction.Enable();
            pauseAction.performed += _ => TogglePauseMenu();
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    private void FindCanvasesByTag()
    {
        // FindObjectsByType with includeInactive to find inactive objects too
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.isLoaded) // Only objects in loaded scenes
            {
                if (obj.CompareTag("Victory"))
                {
                    levelCompleteCanvas = obj;
                }
                else if (obj.CompareTag("Pause"))
                {
                    pauseMenuCanvas = obj;
                }
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCanvasesByTag();
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuCanvas == null) return;
        
        isPaused = !isPaused;
        pauseMenuCanvas.SetActive(isPaused);
        
        // Freeze/unfreeze game
        Time.timeScale = isPaused ? 0f : 1f;
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
        LoadStrawberryData(SceneName);
        SpawnPlayerAtCheckPoint();
    }

    private void LoadStrawberryData(string SceneName)
    {
        List<BerryCollectable> allBerryCollectable = new List<BerryCollectable>(FindObjectsByType<BerryCollectable>(FindObjectsSortMode.None));
        if (allBerryCollectable.Count > currentSaveSlot.GetChapterDataByName(SceneName).GetStrawberries().Count || allBerryCollectable.Count < currentSaveSlot.GetChapterDataByName(SceneName).GetStrawberries().Count)//quatity mismatch,reset data
        {
            currentSaveSlot.GetChapterDataByName(SceneName).SetStrawberries(new List<StrawberryData>());
            List<StrawberryData> tempList = new List<StrawberryData>();
            for (int i = 0; i < allBerryCollectable.Count; i++)
            {
                StrawberryData temp = new StrawberryData(allBerryCollectable[i].gameObject.name);
                temp.SetCollect(false);
                tempList.Add(temp);
                allBerryCollectable[i].SetData(temp);
            }
            currentSaveSlot.GetChapterDataByName(SceneName).SetStrawberries(tempList);
            SaveSlot(currentSaveSlot.SlotID);
        }
        else//load data
        {
            foreach (var strawberryData in currentSaveSlot.GetChapterDataByName(SceneName).GetStrawberries())
            {
                var strawberryObj = allBerryCollectable.Find(s => s.gameObject.name == strawberryData.GetStrawberryID());
                if (strawberryObj != null)
                {
                    strawberryObj.SetData(strawberryData);
                    if (strawberryData.IsCollected())
                    {
                        strawberryObj.gameObject.SetActive(false);
                    }
                    else
                    {
                        strawberryObj.gameObject.SetActive(true);
                    }
                }
            }
        }
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
        if(newSceneName != "MainMenu" && newSceneName != "")
            currentSaveSlot.ChapterDatas.Find(chapter => chapter.Name == newSceneName)?.UnlockChapter();
        SaveSlot(currentSaveSlot.SlotID);
        StartCoroutine(LoadPlayerAndChapterDataLoadScene(currentSaveSlot.PlayerData.GetStage()));
        currentPlayingStatus = PlayingChapterStatus.Playing;
    }

    public void ReturnToMainMenu()
    {
        // Save current progress
        SaveSlot(currentSaveSlot.SlotID);
        
        // Reset pause state if paused
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            if (pauseMenuCanvas != null)
                pauseMenuCanvas.SetActive(false);
        }
        
        // Destroy current player
        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }
        
        // Load main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public enum PlayingChapterStatus
    {
        ChapterEnding,
        ChapterComplete,
        SceneLoading,
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
            
            // Play level complete audio
            AudioManager.Instance.PlayPlayerSFX("altitudecount");
            
            // Activate level complete canvas
            if (levelCompleteCanvas != null)
            {
                levelCompleteCanvas.SetActive(true);
            }
            
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
            if (currentPlayingStatus != PlayingChapterStatus.SceneLoading)
            {
                StartCoroutine(ReloadScene());
            }
            
        }
    }

    IEnumerator<WaitForNextFrameUnit> ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        currentPlayingStatus = PlayingChapterStatus.SceneLoading;
        yield return new WaitForNextFrameUnit();
        SpawnPlayerAtCheckPoint();
        reload.Enable();
        currentPlayingStatus = PlayingChapterStatus.Playing;
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
        
        // Play death sound effect
        AudioManager.Instance.PlayPlayerSFX("death");
        
        // Respawn player after delay (animation + effect time)
        StartCoroutine(SpawnPlayerAfterDelayCoroutine(2f));
    }

    public void SpawnDeathEffect(Vector3 position)
    {
        if(deathEffectPrefab != null)
        {
            ParticleSystem deathEffect = Instantiate(deathEffectPrefab, position, Quaternion.identity);
            deathEffect.Play();
            Destroy(deathEffect.gameObject, 2f);
        }
        StartCoroutine(SpawnPlayerAfterDelayCoroutine(1.5f));
    }
    public void CreateNewSaveSlot(int slotID)
    {
        SaveSlot newSaveSlot = new SaveSlot(slotID);
        currentSaveSlot = newSaveSlot;
        newSaveSlot.SaveToFile();
        
        // Play save file begin sound effect
        AudioManager.Instance.PlayPlayerSFX("ui_main_savefile_begin");
        
        ChangeScene("Prologue");
        Debug.Log("Created new save slot with ID: " + slotID);
    }

}
