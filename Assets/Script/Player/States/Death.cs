using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
namespace Player_State
{
    public class Death : Player_State.PlayerState
    {
        public Death(PlayerController playerController) : base(playerController)
        {
        }
        public override void Enter()
        { 
            Debug.Log("Player has died.");
            //playerController.GetAnimator().Play("PlayerDeath");
            playerController.DisableInput();
        }
        public override void Exit()
        {
        }
        public override void FixedUpdate()
        {
        }
        public override void Update()
        {
        }
        
        
    }
}