using UnityEngine;

namespace HSM {
    public class Attacking : State {
        readonly PlayerContext ctx;
        public readonly Idle Idle;
        public readonly Move Move;

        public Attacking(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
            Idle = new Idle(m, this, ctx);
            Move = new Move(m, this, ctx);
            Add(new ColorPhaseActivity(ctx.renderer){
                enterColor = Color.yellow,  // runs while Grounded is activating
            });
        }
        
        protected override State GetInitialState()
        {
            Debug.Log($"GET INITIAL STATE");
            return Idle;
        }

        protected override State GetTransition() {
            return ctx.grounded ? null : ((PlayerRoot)Parent).Airborne;
        }
    }
}