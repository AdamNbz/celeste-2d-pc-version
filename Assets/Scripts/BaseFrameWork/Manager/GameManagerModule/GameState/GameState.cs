using GameStateEvent;
using UnityEngine;
namespace GameState
{
    public abstract class GameState
    {
        public abstract void OnStateEnter();
        public abstract void OnStateExit();
        public string GetStateName()
        {
            return this.GetType().Name;
        }
    }

    public class Playing : GameState
    {
        public override void OnStateEnter()
        {
            GameManagerRefactor.Instance.GetModule<EventBus>().Publish<UnpauseGameEvent>(new UnpauseGameEvent());
            GameplayGate.ResumeGameplay();
        }

        public override void OnStateExit()
        {
        }
    }

    public class Paused : GameState
    {
        public override void OnStateEnter()
        {
            Time.timeScale = 0f;
            GameplayGate.PauseGameplay();
            GameManagerRefactor.Instance.GetModule<EventBus>().Publish<PauseGameEvent>(new PauseGameEvent());
        }
        public override void OnStateExit()
        {
            Time.timeScale = 1f;
        }
    }
}

namespace GameStateEvent
{
    public class UnpauseGameEvent:Event
    {
    }

    public class  PauseGameEvent:Event
    {
        
    }
}
