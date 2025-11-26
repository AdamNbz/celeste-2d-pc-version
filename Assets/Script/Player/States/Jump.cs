using UnityEngine;
namespace Player_State
{
    public class Jump : PlayerState
    {
        public Jump(PlayerController playerController) : base(playerController)
        {
        }
        public override void Enter()
        {
            playerController.GetAnimator().SetBool("isJumping", true);
        }
        public override void Exit()
        {
            playerController.GetAnimator().SetBool("isJumping", false);
        }
        public override void FixedUpdate()
        {
            playerController.HandleMovement();
            playerController.HandleDash();

            if (playerController.GetObjectVelocity().y<0)
            {
               
                playerController.SetState(new Fall(playerController));
            }
            
        }
        public override void Update()
        {
        }
    }
}