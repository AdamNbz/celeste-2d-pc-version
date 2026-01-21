using UnityEngine;

namespace Player_State
{
    public class Walk : PlayerState
    {
        private float footstepTimer = 0f;
        private float footstepInterval = 0.3f; // Time between footstep sounds
        
        public Walk(PlayerController playerController) : base(playerController)
        {
        }
        public override void Enter()
        {
            playerController.GetAnimator().Play("PlayerWalk");
            
            // Play footstep audio immediately
            AudioManager.Instance.PlayPlayerSFX("foot_00_asphalt_01");
            footstepTimer = footstepInterval;
        }
        public override void Exit()
        {
            
        }
        public override void FixedUpdate()
        {
            if (!playerController.HandleMovement())
            {
                playerController.SetState(new Idle(playerController));
                return;
            }

            if (!playerController.IsOnTheGround())
            {
                playerController.SetState(new Fall(playerController));
                return;
            }

            playerController.HandleJump();
            playerController.HandleDash();
        }
        public override void Update()
        {
            // Play footstep audio at intervals
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                AudioManager.Instance.PlayPlayerSFX("foot_00_asphalt_01");
                footstepTimer = footstepInterval;
            }
        }
    }
}   