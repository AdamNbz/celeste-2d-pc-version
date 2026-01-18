using UnityEngine;

public class BlueSwitchingObject : SwitchingObject
{
    public override void SwitchOff()
    {
        gameObject.SetActive(false);
    }

    public override void SwitchOn()
    {
        gameObject.SetActive(true);
    }
}
