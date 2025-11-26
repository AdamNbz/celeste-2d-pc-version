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
            
        }

        public override void Exit()
        {

        }

        public override void FixedUpdate()
        {
           

            if (playerController.HandleMovement()&&playerController.IsOnTheGround())
            {
                playerController.SetState(new Walk(playerController));
            }
            playerController.HandleJump();
            playerController.HandleDash();
        }

        public override void Update()
        {
           
        }
    }
}
