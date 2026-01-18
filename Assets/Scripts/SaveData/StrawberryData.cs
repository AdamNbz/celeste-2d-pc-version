using UnityEngine;
[System.Serializable]
public class StrawberryData
{
    [SerializeField] bool isCollected = false;
    [SerializeField] string strawberryID;
    public StrawberryData(string id)
    {
        strawberryID = id;
    }

    public string GetStrawberryID()
    {
        return strawberryID;
    }

    public bool IsCollected()
    {
        return isCollected;
    }
    public void SetCollect(bool status)
    {
        isCollected = status;
    }

}
