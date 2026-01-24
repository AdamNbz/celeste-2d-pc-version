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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPause(PauseGameEvent e)
    {
            Debug.Log("Game Paused");
    }
}
