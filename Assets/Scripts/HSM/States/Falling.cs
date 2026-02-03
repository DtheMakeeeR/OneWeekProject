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
        //protected override void OnEnter()
        //{
        //    ctx.anim.SetBool("isFalling", true);
        //}
        //protected override void OnExit()
        //{
        //    ctx.anim.SetBool("isFalling", false);
        //}
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
