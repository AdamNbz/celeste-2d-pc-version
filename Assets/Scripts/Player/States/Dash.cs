using Assets.Script.Player.States;
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
            playerController.TurnOffDashAble();
            previousSpeed =playerController.GetObjectVelocity().x / playerController.Direction;
            playerController.SetObjectVelocity(playerController.dashSpeed*playerController.Direction, 0);
            playerController.GetComponent<Rigidbody2D>().gravityScale = 0f;
            
            // Play dash audio based on direction
            if (playerController.Direction > 0)
            {
                AudioManager.Instance.PlayPlayerSFX("dash_red_right");
            }
            else
            {
                AudioManager.Instance.PlayPlayerSFX("dash_red_left");
            }
        }

        public override void Exit()
        {
            playerController.GetComponent<Rigidbody2D>().gravityScale = playerController.GetBaseGravityScale();
        }

        public override void FixedUpdate()
        { 
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0)
            {
                playerController.SetState(prevState);
                playerController.SetObjectVelocity(previousSpeed*playerController.Direction,0);
            }

            if(playerController.IsTouchingWall()&& !playerController.IsOnTheGround())
            {
                playerController.SetState(new Climb(playerController));
            }

        }

        public override void Update()
        {

        }
    }
}
