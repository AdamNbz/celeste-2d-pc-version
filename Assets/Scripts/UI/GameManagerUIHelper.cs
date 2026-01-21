using UnityEngine;

/// <summary>
/// Helper script to call GameManager methods from UI buttons.
/// Attach this to any UI button that needs to interact with GameManager.
/// </summary>
public class GameManagerUIHelper : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        GameManager.GetInstance().ReturnToMainMenu();
    }

    public void TogglePauseMenu()
    {
        GameManager.GetInstance().TogglePauseMenu();
    }

    public void LoadSlot(int slotID)
    {
        GameManager.GetInstance().LoadSlot(slotID);
    }

    public void CreateNewSaveSlot(int slotID)
    {
        GameManager.GetInstance().CreateNewSaveSlot(slotID);
    }

    public void ResumeGame()
    {
        // Same as toggle but explicitly unpause
        GameManager.GetInstance().TogglePauseMenu();
    }
}
