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
     
            playerController.GetAnimator().Play("PlayerFall");
        }
        public override void Exit()
        {
            playerController.SpawnLandingEffect();
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
