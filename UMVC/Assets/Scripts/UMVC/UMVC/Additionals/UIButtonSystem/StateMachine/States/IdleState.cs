﻿using UnityEngine;

namespace UMVC
{
    public class IdleState : StateBase
    {
        public IdleState()
        {
            StateEnum = InteractionStateEnum.Idle;
        }

        public override void UpdateFrame()
        {
            if (CanUpdate)
            {
            }
        }

        public override void EnterStateHandler(params object[] addParams)
        {
            StateEnterTime = Time.realtimeSinceStartup;

            FireOnEnterStateHandled();
        }

        public override void ExitStateHandler()
        {
            StateExitTime = Time.realtimeSinceStartup;

            CanUpdate = false;

            FireOnExitStateHandled();
        }

        public override void StateHandler()
        {
            StateHandleTime = Time.realtimeSinceStartup;

            CanUpdate = true;

            FireOnStateHandled();
        }
    }
}
