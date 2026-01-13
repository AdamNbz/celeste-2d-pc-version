using UnityEngine;

public class SaveContainerScript : MonoBehaviour
{
    [SerializeField] private int saveSlotID;
    [SerializeField] GameObject SaveSelectButton;
    [SerializeField] GameObject SaveCreateButton;
    private void Start()
    {
        if (SaveSystem.IfSaveFileExist(saveSlotID))
        {
            SaveSelectButton.SetActive(true);
            SaveSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Save Slot " + saveSlotID.ToString();
            SaveCreateButton.SetActive(false);
        }
        else
        {
            SaveSelectButton.SetActive(false);
            SaveCreateButton.SetActive(true);
        }
    }

    public int GetSaveSlotID()
    {
        return saveSlotID;
    }
}
