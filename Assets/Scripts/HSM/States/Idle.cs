using UnityEngine;
using Utilities;

namespace HSM {
    public class Idle : State {
        readonly PlayerContext ctx;

        public Idle(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
        }

        protected override State GetTransition()
        {
            return ctx.IsMoveInput ? ((Grounded)Parent).Move : null;
        }

        protected override void OnEnter() {
            ctx.velocity.x = 0f;
            ctx.velocity.z = 0f;
        }
    }
}