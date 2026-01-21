using TMPro;
using UnityEngine;

/// <summary>
/// Script to display the collected berry count on a TextMeshPro UI element.
/// Reads from current save slot and counts berries with isCollected = true in current chapter.
/// Attach this to a Canvas/Text object.
/// </summary>
public class BerryCountDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI berryCountText;
    private static BerryCountDisplay instance;
    
    public static BerryCountDisplay Instance => instance;
    
    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        if (berryCountText == null)
        {
            berryCountText = GetComponent<TextMeshProUGUI>();
        }
        UpdateBerryCount();
    }

    private void OnEnable()
    {
        UpdateBerryCount();
    }

    public void UpdateBerryCount()
    {
        if (berryCountText == null) return;
        
        int collectedCount = GetCollectedBerryCount();
        berryCountText.text = collectedCount.ToString();
    }

    /// <summary>
    /// Get the total number of collected berries from current chapter save data.
    /// Reads from save file (save01.txt to save03.txt based on slotID).
    /// </summary>
    private int GetCollectedBerryCount()
    {
        var saveSlot = GameManager.GetInstance().GetCurrentSaveSlot();
        if (saveSlot == null) 
        {
            Debug.Log("BerryCountDisplay: No save slot loaded");
            return 0;
        }
        
        // Get current stage from playerData.currentstage
        string currentStage = saveSlot.PlayerData.GetStage();
        if (string.IsNullOrEmpty(currentStage)) 
        {
            Debug.Log("BerryCountDisplay: No current stage");
            return 0;
        }
        
        // Find the chapter data matching current stage
        var chapterData = saveSlot.GetChapterDataByName(currentStage);
        if (chapterData == null) 
        {
            Debug.Log($"BerryCountDisplay: Chapter data not found for {currentStage}");
            return 0;
        }
        
        // Get strawberries list from chapter data
        var strawberries = chapterData.GetStrawberries();
        if (strawberries == null || strawberries.Count == 0) 
        {
            return 0;
        }
        
        // Count berries with isCollected = true
        int count = 0;
        foreach (var strawberry in strawberries)
        {
            if (strawberry.IsCollected())
            {
                count++;
            }
        }
        
        Debug.Log($"BerryCountDisplay: Save slot {saveSlot.SlotID}, Chapter {currentStage}, Collected {count}/{strawberries.Count}");
        return count;
    }

    /// <summary>
    /// Get the total number of collected berries across ALL chapters
    /// </summary>
    public int GetTotalCollectedBerryCount()
    {
        var saveSlot = GameManager.GetInstance().GetCurrentSaveSlot();
        if (saveSlot == null) return 0;
        
        int totalCount = 0;
        foreach (var chapterData in saveSlot.ChapterDatas)
        {
            var strawberries = chapterData.GetStrawberries();
            if (strawberries != null)
            {
                foreach (var strawberry in strawberries)
                {
                    if (strawberry.IsCollected())
                    {
                        totalCount++;
                    }
                }
            }
        }
        return totalCount;
    }
}
