using UnityEngine;

namespace Player_State
{
    public class Fall : PlayerState
    {
        public Fall(PlayerController playerController) : base(playerController)
        {
        }
        public override void Enter()
        {
     
            playerController.GetAnimator().SetBool("isFalling", true);
        }
        public override void Exit()
        {
            playerController.SpawnLandingEffect();
            playerController.GetAnimator().SetBool("isFalling", false);
        }
        public override void FixedUpdate()
        {
            
            if(playerController.IsOnTheGround())
            {
                playerController.SetObjectVelocity(playerController.GetObjectVelocity().x, 0);
                playerController.SetState(new Idle(playerController));
                return;
            }
            playerController.HandleMovement();
            playerController.HandleDash();
        }
        public override void Update()
        {
        }
    }
}
