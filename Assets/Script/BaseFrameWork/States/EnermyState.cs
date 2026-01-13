using UnityEngine;
namespace Enermy_State
{
    public abstract class EnermyState : State
    {
        protected EnermyController enermyController;
        public EnermyState prevState;
        public EnermyState(EnermyController enermyController)
        {
            this.enermyController = enermyController;//init enermyController
        }
        abstract public void Enter();

        abstract public void Exit();
        abstract public void FixedUpdate();
        public string GetStateName()
        {
            return this.GetType().Name;
        }
        abstract public void Update();
    }
}
