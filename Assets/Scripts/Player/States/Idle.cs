using UnityEngine;

namespace Player_State
{
    public class Idle : PlayerState
    {
        public Idle(PlayerController playerController) : base(playerController)
        {
        }

        public override void Enter()
        {
            playerController.GetAnimator().Play("PlayerIdle");
        }

        public override void Exit()
        {

        }

        public override void FixedUpdate()
        {
            if (!playerController.IsOnTheGround())
            {
                playerController.SetState(new Fall(playerController));
                return;
            }

            if (playerController.HandleMovement())
            {
                playerController.SetState(new Walk(playerController));
                return;
            }

            playerController.HandleJump();
            playerController.HandleDash();
        }

        public override void Update()
        {
           
        }
    }
}
