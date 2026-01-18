using UnityEngine;

public class GoToSaveChose : MonoBehaviour
{
    public void OnClick()
    {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SaveChose");
    }
}
