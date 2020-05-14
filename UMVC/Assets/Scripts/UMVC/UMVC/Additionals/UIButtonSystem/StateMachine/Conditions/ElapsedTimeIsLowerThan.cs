﻿namespace UMVC
{
    public class ElapsedTimeIsLowerThan : ConditionBase
    {
        public override bool CheckCondition(params object[] param)
        {
            int passedTime = (int)param[0];

            return passedTime <= UIButtonUtilities.SensivityInMilliseconds;
        }
    }
}
