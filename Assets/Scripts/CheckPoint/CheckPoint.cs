using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool activated = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")&&!activated)
        {
            GameManager.GetInstance().GetPlayerController().GetPlayerData().SetCheckpoint(gameObject.name);
            ActiveCheckPoint();
            GameManager.GetInstance().SaveSlot(GameManager.GetInstance().GetCurrentSaveSlot().SlotID);
        }
    }

    public void ActiveCheckPoint()
    {
        if (!activated)
        {
            activated = true;
        }
    }

    public int GetIndex()
    {
        string name = gameObject.name;
        return int.Parse(name);
    }
}
