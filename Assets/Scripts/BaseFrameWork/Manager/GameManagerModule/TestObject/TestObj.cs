using GameState;
using GameStateEvent;
using UnityEngine;

public class TestObj : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManagerRefactor.Instance.GetModule<EventBus>().Subscribe<PauseGameEvent>(OnPause);
        GameManagerRefactor.Instance.GetModule<GameStateHolderModule>()?.ChangeStateRequest(new Paused());
        GameManagerRefactor.Instance.GetModule<InputManager>().StartListeningForKey("Up");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerRefactor.Instance.GetModule<InputManager>().GetGamePlayAction("Up")!=null)
        {
            if (GameManagerRefactor.Instance.GetModule<InputManager>().GetGamePlayAction("Up").WasPressedThisFrame())
                Debug.Log("Pressed");
        }
    }

    void OnPause(PauseGameEvent e)
    {
            Debug.Log("Game Paused");
    }
}
