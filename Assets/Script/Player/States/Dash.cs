using System;
using UnityEngine;

namespace Player_State
{
    public class Dash : PlayerState
    {
        float dashDuration = 0.2f; // thời gian dash
        float dashTimer = 0;
        float previousSpeed;
        public Dash(PlayerController playerController) : base(playerController)
        {
        }

        public override void Enter()
        {
            playerController.GetAnimator().Play("PlayerDash");
            dashTimer = dashDuration;
            previousSpeed=playerController.GetObjectVelocity().x/playerController.Direction;
            playerController.SetObjectVelocity(playerController.dashSpeed*playerController.Direction,0);
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
            playerController.SetObjectVelocity(playerController.GetObjectVelocity().x, 0);
            dashTimer -= Time.fixedDeltaTime;
            // vẫn di chuyển trong dash nếu muốn
            if (dashTimer <= 0)
            {
                playerController.SetState(prevState); // quay về state trước
                playerController.SetObjectVelocity(previousSpeed*playerController.Direction,0);
            }
            
        }

        public override void Update()
        {

        }
    }
}
