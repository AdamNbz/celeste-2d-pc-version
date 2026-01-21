using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.BaseFrameWork.States
{
    public abstract class BerryState : State
    {
        protected BerryCollectable berry;
        public BerryState prevState;
        public BerryState(BerryCollectable berryCollectable)
        {
            berry = berryCollectable;
        }
        public abstract void Enter();


        public abstract void Exit();


        public abstract void FixedUpdate();

        public string GetStateName()
        {
            return this.GetType().Name;
        }


        public abstract void Update();
    }
}
