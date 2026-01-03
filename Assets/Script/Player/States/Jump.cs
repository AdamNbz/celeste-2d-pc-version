using Assets.Script.Player.States;
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
            playerController.GetAnimator().Play("PlayerJump");
            
        }
        public override void Exit()
        {
            
        }
        public override void FixedUpdate()
        {
            Debug.Log(prevState.GetStateName());
            if (prevState.GetStateName()!= "Climb")
            {
                playerController.HandleMovement();
            }

            if (playerController.IsTouchingWall() && playerController.IsPressingTowardWall() && playerController.CanClimbWall())
            {
                playerController.SetState(new Climb(playerController));
                return;
            }
            playerController.HandleJump();
            playerController.HandleDash();

            if (playerController.GetObjectVelocity().y < 0)
            {
                playerController.SetState(new Fall(playerController));
            }
        }
        public override void Update()
        {
        }
    }
}