using System.Runtime.CompilerServices;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject vCam;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !col.isTrigger)
        {
            vCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !col.isTrigger)
        {
            vCam.SetActive(false);
        }
    }
}
