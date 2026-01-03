using Player_State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Player.States
{
    internal class Climb : PlayerState
    {
        private float originalGravityScale;
        float climbTimer;
        public Climb(PlayerController playerController) : base(playerController)
        {
            originalGravityScale = playerController.GetComponent<Rigidbody2D>().gravityScale;
        }

        public override void Enter()
        {
            playerController.GetAnimator().Play("PlayerWallSiding");
            playerController.GetComponent<Rigidbody2D>().gravityScale = 0f;
            climbTimer = playerController.MaxClimbTime;
        }

        public override void Exit()
        {
            playerController.GetComponent<Rigidbody2D>().gravityScale = originalGravityScale;
            if(climbTimer <= 0)
            {
                playerController.StartWallCooldown();
            }
        }

        public override void FixedUpdate()
        {
            climbTimer -= Time.fixedDeltaTime;

            // HẾT THỜI GIAN BÁM
            if (climbTimer <= 0)
            {
                playerController.SetState(new Fall(playerController));
                return;
            }

            if (playerController.HandleJump())
            {
                playerController.GetComponent<Rigidbody2D>().linearVelocityX += playerController.WallJumpForce * -playerController.Direction;
                playerController.Direction= -playerController.Direction;
                playerController.SetState(new Jump(playerController));
                return;
            }

            if (!playerController.IsTouchingWall() || !playerController.IsPressingTowardWall())
            {
                playerController.SetState(new Fall(playerController));
                return;
            }

            // KHÓA rơi — cực kỳ quan trọng
            playerController.SetObjectVelocity(0, 0);

        }

        public override void Update()
        {
            
        }
    }
}
