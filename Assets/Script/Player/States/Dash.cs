using UnityEngine;

namespace Player_State
{
    public class Dash : PlayerState
    {
        public Dash(PlayerController playerController) : base(playerController)
        {
        }

        public override void Enter()
        {
            playerController.GetAnimator().Play("Dash");
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            Debug.Log("Dash FixedUpdate" + " " + GetStateName());
            playerController.SetState(prevState);
        }

        public override void Update()
        {

        }
    }
}
