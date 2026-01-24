using UnityEngine;
//this for unity timescale independent game control component
using GameState;
using GameStateEvent;
public class GameStateHolderModule : IUpdateModule
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
        currentGameState.OnStateExit();
        nextGameState.OnStateEnter();
        currentGameState = nextGameState;

    }

    void IUpdateModule.UpdateModule()
    {
        if (currentGameState.GetStateName()!=nextGameState.GetStateName())
        {
            Debug.Log("Game State Change Confirmed to: " + nextGameState.GetStateName());
            OnChangeStateConfirmed();
        }
    }
}


