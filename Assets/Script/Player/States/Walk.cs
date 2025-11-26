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
            playerController.GetAnimator().SetBool("isWalking", true);

        }
        public override void Exit()
        {
            playerController.GetAnimator().SetBool("isWalking", false);
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