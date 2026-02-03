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
        //protected override void OnEnter()
        //{
        //    ctx.anim.SetBool("isJumping", true);
        //}
        //protected override void OnExit()
        //{
        //    Debug.Log("EXIT JUMP");
        //    ctx.anim.SetBool("isJumping", false);
        //}
        protected override State GetTransition()
        {
            return ctx.IsFalling ? ((Airborne)Parent).Falling : null;
        }
    }
}
