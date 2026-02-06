using UnityEngine;
using Utilities;

namespace HSM {
    public class Move : State {
        readonly PlayerContext ctx;

        public Move(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
            Add(new AnimatorBoolActivity(ctx.anim, "isMoving", true, false));
        }
        //protected override void OnEnter()
        //{
        //    ctx.IsNeedRotation = true;
        //}
        //protected override void OnExit()
        //{
        //    ctx.IsNeedRotation = false;
        //}
        protected override State GetTransition() {

            if (ctx.IsJumping)
            {
                //ctx.jumpPressed = false;
                //var rb = ctx.rb;
                //if (rb != null)
                //{
                //    var v = rb.linearVelocity;
                //    v.y = ctx.jumpSpeed;
                //    rb.linearVelocity = v;
                //}
                return ((PlayerRoot)((Grounded)Parent).Parent).Airborne.Jump;
            }
            if (!ctx.IsGrounded) return ((PlayerRoot)Parent).Airborne;
            else if (ctx.IsHitted)
            {
                return ((PlayerRoot)((Grounded)Parent).Parent).Hitted;
            }
            else if (ctx.IsAttacking)
            {
                return ((PlayerRoot)((Grounded)Parent).Parent).Attacking.GroundAttack;
            }
            else if (ctx.IsDodging)
            {
                return ((Grounded)Parent).Dodge;
            }
            return !ctx.IsMoveInput ? ((Grounded)Parent).Idle : null;
        }

        protected override void OnUpdate(float deltaTime) {
            var coef = ctx.IsSprinting ? ctx.sprintCoef : 1;
            var targetX = ctx.move.x * ctx.moveSpeed * coef;
            var targetZ = ctx.move.z * ctx.moveSpeed * coef;
            ctx.velocity.x = Mathf.MoveTowards(ctx.velocity.x, targetX, ctx.accel * deltaTime);
            ctx.velocity.z = Mathf.MoveTowards(ctx.velocity.z, targetZ, ctx.accel * deltaTime);
        }
    }
}