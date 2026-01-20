using Assets.Script.SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectorButton : MonoBehaviour
{
    [SerializeField] string chapterName;
    void Start()
    {
        Button button = GetComponent<Button>();
        TextMeshProUGUI textComponent = GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = chapterName;
            if (GameManager.GetInstance() != null)
            {
                ChapterData data = GameManager.GetInstance().GetCurrentSaveSlot().ChapterDatas.Find(chapter => chapter.Name == chapterName);
                if (data!=null)
                {
                    if (data.IsUnlocked())
                    {
                        textComponent.color = Color.yellow;
                        button.interactable = true;
                    }
                    else
                    {
                        textComponent.color = Color.red;
                        button.interactable = false;
                    }
                }
            }
        }
    }

    public void OnClick()
    {
        GameManager.GetInstance().ChangeScene(chapterName);
    }

}
