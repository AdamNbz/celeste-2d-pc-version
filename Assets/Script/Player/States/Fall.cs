using Assets.Script.Player.States;
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

            if (playerController.IsOnTheGround())
            {
                playerController.SetState(new Idle(playerController));
                return;
            }

            if (playerController.IsTouchingWall() && playerController.IsPressingTowardWall() && playerController.CanClimbWall())
            {
                playerController.SetState(new Climb(playerController));
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
