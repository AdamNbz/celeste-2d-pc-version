using UnityEngine;
//this for unity timescale independent game control component
using GameState;
using GameStateEvent;
public class GameStateHolderModule : IFixedUpdateModule
{
    GameState.GameState currentGameState =new Playing() ;
    GameState.GameState nextGameState = new Playing();

    void IModule.AwakeModule()
    {
        
    }

    public void ChangeStateRequest(GameState.GameState gameState)
    {
        nextGameState = gameState;
    }

    public GameState.GameState GetGameState()
    {
        return currentGameState;
    }

    void OnChangeStateConfirmed()
    {
        Debug.Log("Game State Changed to: " + nextGameState.GetStateName());
        currentGameState.OnStateExit();
        nextGameState.OnStateEnter();
        currentGameState = nextGameState;

    }

    void IFixedUpdateModule.FixedUpdateModule()
    {
        if (currentGameState.GetStateName()!=nextGameState.GetStateName())
        {
            OnChangeStateConfirmed();
        }
    }
}


