using Unity.VisualScripting;
using UnityEngine;
namespace Player_State
{
    public abstract class PlayerState : State
    {
        protected PlayerController playerController;

        public PlayerState(PlayerController playerController)
        {
            this.playerController = playerController;
        }
        public PlayerState prevState;
        public string GetStateName()
        {
            return this.GetType().Name;
        }
        abstract public void Enter();

        abstract public void Exit();

        abstract public void FixedUpdate();

        abstract public void Update();
    }
}
