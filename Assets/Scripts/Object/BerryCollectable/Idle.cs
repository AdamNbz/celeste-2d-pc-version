using Assets.Scripts.BaseFrameWork.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Object.BerryCollectable
{
    internal class Idle : BerryState
    {
        public Idle(global::BerryCollectable berryCollectable) : base(berryCollectable)
        {
        }

        public override void Enter()
        {
            berry.Animator.Play("BerryIdle");
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
