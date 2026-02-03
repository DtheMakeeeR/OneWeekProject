using UnityEngine;
using Utilities;

namespace HSM {
    public class Move : State {
        readonly PlayerContext ctx;

        public Move(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
            Add(new AnimatorBoolActivity(ctx.anim, "isMoving", true, false));
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

            if (ctx.jumpPressed)
            {
                ctx.jumpPressed = false;
                var rb = ctx.rb;
                if (rb != null)
                {
                    var v = rb.linearVelocity;
                    v.y = ctx.jumpSpeed;
                    rb.linearVelocity = v;
                }
                return ((PlayerRoot)((Grounded)Parent).Parent).Airborne.Jump;
            }
            if (!ctx.grounded) return ((PlayerRoot)Parent).Airborne;
            
            return !ctx.IsMoveInput ? ((Grounded)Parent).Idle : null;
        }

        protected override void OnUpdate(float deltaTime) {
            var coef = ctx.sprintPressed ? ctx.sprintCoef : 1;
            var targetX = ctx.move.x * ctx.moveSpeed * coef;
            var targetZ = ctx.move.z * ctx.moveSpeed * coef;
            ctx.velocity.x = Mathf.MoveTowards(ctx.velocity.x, targetX, ctx.accel * deltaTime);
            ctx.velocity.z = Mathf.MoveTowards(ctx.velocity.z, targetZ, ctx.accel * deltaTime);
        }
    }
}