using UnityEngine;

namespace HSM {
    public class Attacking : State {
        readonly PlayerContext ctx;
        public readonly GroundAttack GroundAttack;

        public Attacking(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
            GroundAttack = new GroundAttack(m, this, ctx);
            Add(new AnimatorBoolActivity(ctx.anim, "isAttacking", true, false));
        }
        
        protected override State GetInitialState()
        {
            return GroundAttack;
        }

        protected override State GetTransition() {
            return null;
        }
    }
}