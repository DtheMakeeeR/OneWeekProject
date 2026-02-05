using HSM;
using UnityEngine;

namespace HSM
{
    public class Jump : State
    {
        readonly PlayerContext ctx;

        public Jump(StateMachine m, State parent, PlayerContext ctx) : base(m, parent)
        {
            this.ctx = ctx;
            Add(new AnimatorBoolActivity(ctx.anim, "isJumping", true, false));
        }
        protected override void OnEnter()
        {
            ctx.jumpPressed = false;
            var rb = ctx.rb;
            if (rb != null)
            {
                var v = rb.linearVelocity;
                v.y = ctx.jumpSpeed;
                rb.linearVelocity = v;
            }
            ctx.IsNeedChangeVel = false;
        }
        protected override void OnExit()
        {
            ctx.IsNeedChangeVel = true;
        }
        protected override State GetTransition()
        {
            return ctx.IsFalling ? ((Airborne)Parent).Falling : null;
        }
    }
}
