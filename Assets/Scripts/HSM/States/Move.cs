using UnityEngine;
using Utilities;

namespace HSM {
    public class Move : State {
        readonly PlayerContext ctx;

        public Move(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
        }
        protected override void OnEnter()
        {
            ctx.IsNeedRotation = true;
        }
        protected override void OnExit()
        {
            ctx.IsNeedRotation = false;
        }
        protected override State GetTransition() {
            if (!ctx.grounded) return ((PlayerRoot)Parent).Airborne;
            
            return !ctx.IsMoveInput ? ((Grounded)Parent).Idle : null;
        }

        protected override void OnUpdate(float deltaTime) {
            var targetX = ctx.move.x * ctx.moveSpeed;
            var targetZ = ctx.move.z * ctx.moveSpeed;
            ctx.velocity.x = Mathf.MoveTowards(ctx.velocity.x, targetX, ctx.accel * deltaTime);
            ctx.velocity.z = Mathf.MoveTowards(ctx.velocity.z, targetZ, ctx.accel * deltaTime);
        }
    }
}