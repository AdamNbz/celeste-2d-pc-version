using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorTextTriggerBird : MonoBehaviour
{
    [SerializeField] string TutorialText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextMeshPro textMesh = transform.parent.Find("TutorialTextMesh").GetComponent<TextMeshPro>();
            if (textMesh==null)
            {
                Debug.Log("Text Mesh Not found");
                return;
            }
            textMesh.text = TutorialText;
            textMesh.gameObject.SetActive(true);
            textMesh.enableAutoSizing = true;
            textMesh.fontSizeMin = 0;
            textMesh.alignment = TextAlignmentOptions.Center;
            StartCoroutine(TextVanish(3f));
        }
    }

    private IEnumerator TextVanish(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.parent.Find("TutorialTextMesh").gameObject.SetActive(false);
    }
}
