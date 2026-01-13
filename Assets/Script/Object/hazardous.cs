using UnityEngine;

public class hazardous : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetInstance().OnPlayerDeath();
        }
    }
}
