using UnityEngine;

namespace HSM
{
    public class Falling : State
    {
        readonly PlayerContext ctx;

        public Falling(StateMachine m, State parent, PlayerContext ctx) : base(m, parent)
        {
            this.ctx = ctx;
            Add(new AnimatorBoolActivity(ctx.anim, "isFalling", true, false));
        }
        protected override void OnEnter()
        {
            ctx.IsNeedChangeVel = false;
        }
        protected override void OnExit()
        {
            ctx.IsNeedChangeVel = true;
        }
        protected override State GetTransition()
        {
            if (ctx.IsFalling)
            {
                return null;
            }
            else
            {
                return ((PlayerRoot)((Airborne)Parent).Parent).Grounded;
            }
        }
    }
}
