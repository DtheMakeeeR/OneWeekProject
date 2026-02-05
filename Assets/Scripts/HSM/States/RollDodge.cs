using MEC;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace HSM {
    public class RollDodge : State {
        readonly PlayerContext ctx;
        private readonly float _length = 0.25f;
        private bool _flag = false;

        public RollDodge(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
        }

        protected override State GetTransition()
        {
            if(!_flag) return null;
            return ((Grounded)Parent).Idle;
        }

        protected override void OnEnter() {
            //ctx.IsNeedChangeVel = false;
            Timing.RunCoroutine(_FlagCoroutine());
            ctx.IsDodging = false;
            ctx.IsNeedRotation = false;
            var rb = ctx.rb;
            if (rb != null)
            {
                var v = rb.linearVelocity;
                if (ctx.CameraSpaceMove.sqrMagnitude <= 0.1f)
                {
                    v = ctx.CameraForward * ctx.dodgeSpeed;
                }
                else
                {
                    v.x = ctx.CameraSpaceMove.x * ctx.dodgeSpeed;
                    v.z = ctx.CameraSpaceMove.z * ctx.dodgeSpeed;
                }
                    
                //rb.linearVelocity = v;
                rb.linearVelocity = new Vector3();
                rb.AddForce(v, ForceMode.Impulse);
            }
            ctx.IsNeedChangeVel = false;
        }
        protected override void OnExit()
        {
            _flag = false;
            ctx.IsNeedRotation = true;
            ctx.IsNeedChangeVel = true;
            //ctx.IsNeedChangeVel = true;
        }
        IEnumerator<float> _FlagCoroutine()
        {
            yield return Timing.WaitForSeconds(_length);
            _flag = true;
        }
    }
}