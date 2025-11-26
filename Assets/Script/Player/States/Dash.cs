using System;
using UnityEngine;

namespace Player_State
{
    public class Dash : PlayerState
    {
        float dashDuration = 0.2f; // thời gian dash
        float dashTimer = 0;
        public Dash(PlayerController playerController) : base(playerController)
        {
        }

        public override void Enter()
        {
            playerController.GetAnimator().SetBool("isDashing", true);
            dashTimer = dashDuration;
        }

        public override void Exit()
        {
            playerController.GetAnimator().SetBool("isDashing", false);
        }

        public override void FixedUpdate()
        {

            dashTimer -= Time.fixedDeltaTime;
            // vẫn di chuyển trong dash nếu muốn
            if (dashTimer <= 0)
            {
                
                playerController.SetState(prevState); // quay về state trước
            }



        }

        public override void Update()
        {

        }
    }
}
