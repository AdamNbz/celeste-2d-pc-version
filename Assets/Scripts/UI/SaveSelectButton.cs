using UnityEngine;

public class SaveSelectButton : MonoBehaviour
{
    public void OnClick()
    {
        int saveSlotID = GetComponentInParent<SaveContainerScript>().GetSaveSlotID();
        GameManager.GetInstance().LoadSlot(saveSlotID);
    }
}
