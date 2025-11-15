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
            Debug.Log("Idle FixedUpdate" + " " + GetStateName());
        }

        public override void Update()
        {

        }
    }
}
