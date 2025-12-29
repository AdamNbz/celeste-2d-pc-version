using UnityEngine;

public class EndObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Level Completed!");
            GameManager.GetInstance().OnChapterEnding();
            // Here you can add code to transition to the next level or show a completion screen.
        }
    }
}
