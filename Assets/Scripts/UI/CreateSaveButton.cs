using UnityEngine;

public class CreateSaveButton : MonoBehaviour
{
    public void OnClick()
    {
        int saveSlotID = GetComponentInParent<SaveContainerScript>().GetSaveSlotID();
        GameManager.GetInstance().CreateNewSaveSlot(saveSlotID);
    }    
}
