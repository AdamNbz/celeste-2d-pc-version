using UnityEngine;

public class FlyTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (transform.parent != null)
            {
                TutorTextTriggerBird tutorTextTriggerBird = transform.parent.Find("TutorialShowTrigger").GetComponent<TutorTextTriggerBird>();
                tutorTextTriggerBird.RequestTextVanish();
                Bird bird = transform.parent.Find("Bird").GetComponent<Bird>();
                bird.Fly();
            }
        }
    }
}
