using UnityEngine;

namespace HSM {
    public class Airborne : State {
        readonly PlayerContext ctx;
        public readonly Jump Jump;
        public readonly Falling Falling;

        public Airborne(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
            Jump = new Jump(m, this, ctx);
            Falling = new Falling(m, this, ctx);
            Add(new ColorPhaseActivity(ctx.renderer){
                enterColor = Color.red, // runs while Airborne is activating
            });
        }
        protected override State GetInitialState()
        {
            if (!ctx.IsFalling)
            {
                return Jump;
            }
            else
            {
                return Falling;
            }
        }
        protected override State GetTransition()
        {
            //Debug.Log($"get transition airborne lvelocity: {ctx.rb.linearVelocity.y}");
            //if (ctx.grounded && ctx.rb.linearVelocity.y <= 0f)
            //{
            //    Debug.Log("AIRBORNE RETURN GROUNDED");
            //    return ((PlayerRoot)Parent).Grounded;
            //}
            //else 
                return null;
        }

        //protected override void OnEnter() {
        //    ctx.IsNeedRotation = false;
        //}
        //protected override void OnExit()
        //{
        //    ctx.IsNeedRotation = true;
        //}
    }
}