using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorTextTriggerBird : MonoBehaviour
{
    [SerializeField] string TutorialText;
    bool isVanishing = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TextMeshPro textMesh = transform.parent.Find("TutorialTextMesh").GetComponent<TextMeshPro>();
            if (textMesh == null)
            {
                Debug.Log("Text Mesh Not found");
                return;
            }
            SetUpTextMesh(textMesh);
            Bird bird = transform.parent.Find("Bird").GetComponent<Bird>();
            if (bird != null)
            {
                Debug.Log("Activate Bird");
                bird.gameObject.SetActive(true);
                // Make bird face towards player
                bird.LookAtTarget(collision.transform.position);
            }
        }
    }

    private void SetUpTextMesh(TextMeshPro textMesh)
    {
        textMesh.text = TutorialText;
        textMesh.gameObject.SetActive(true);
        textMesh.enableAutoSizing = true;
        textMesh.fontSizeMin = 0;
        textMesh.alignment = TextAlignmentOptions.Center;
        if (!isVanishing && textMesh.rectTransform.localScale.x < 1)
            textMesh.rectTransform.localScale = Vector3.one;
    }

    private IEnumerator TextVanish()
    {
        TextMeshPro textMesh = transform.parent.Find("TutorialTextMesh").GetComponent<TextMeshPro>();

        yield return new WaitUntil(() => textMesh.rectTransform.localScale.x <= 0);
        isVanishing = false;
        Bird bird = transform.parent.Find("Bird").GetComponent<Bird>();
        bird.OnTextVanishComplete();
        DeActiveBubble();
    }

    private void DeActiveBubble()
    {
        transform.parent.Find("TutorialTextMesh").gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isVanishing)
        {
            TextMeshPro textMesh = transform.parent.Find("TutorialTextMesh").GetComponent<TextMeshPro>();
            if (textMesh.gameObject.activeSelf)
            {
                textMesh.rectTransform.localScale -= new Vector3(1f, 0, 0) * Time.deltaTime;
                if (textMesh.rectTransform.localScale.x < 0)
                {
                    textMesh.rectTransform.localScale = Vector3.zero;
                }
            }
        }
    }

    public void RequestTextVanish()
    {
        if (isVanishing) return;
        if(!transform.parent.Find("TutorialTextMesh").gameObject.activeSelf) return;
        isVanishing = true;
        StartCoroutine(TextVanish());
    }
}
