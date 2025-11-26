using UnityEngine;

namespace Player_State
{
    public class Walk : PlayerState
    {
        public Walk(PlayerController playerController) : base(playerController)
        {
        }
        public override void Enter()
        {
            playerController.GetAnimator().Play("Walk");

        }
        public override void Exit()
        {
            
        }
        public override void FixedUpdate()
        {
            if (!playerController.HandleMovement())
            {
                playerController.SetState(new Idle(playerController));
            }
            playerController.HandleJump();
            playerController.HandleDash();
        }
        public override void Update()
        {
        }
    }
}   