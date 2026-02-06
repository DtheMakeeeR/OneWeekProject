using MEC;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace HSM {
    public class Hitted : State {
        readonly PlayerContext ctx;
        float _length;
        bool _flag;

        public Hitted(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
            Add(new AnimatorBoolActivity(ctx.anim, "isHitted", true, false));
            AnimatorController ac = (AnimatorController)ctx.anim.runtimeAnimatorController;
            AnimatorStateMachine combatMachine = ac.layers[0].stateMachine.stateMachines
                .First(sm => sm.stateMachine.name == "Hitted").stateMachine;

            AnimatorState groundAttackState = combatMachine.states
                .First(s => s.state.name == "Regular Hit").state;

            AnimationClip clip = groundAttackState.motion as AnimationClip;
            _length = clip.length;
        }

        protected override State GetTransition()
        {
            if (!_flag) return null;
            return ((PlayerRoot)Parent).Grounded.Idle;
        }

        protected override void OnEnter()
        {
            ctx.IsNeedRotation = false;
            Timing.RunCoroutine(_FlagCoroutine());
            ctx.velocity.x = 0f;
            ctx.velocity.z = 0f;
        }
        protected override void OnExit()
        {
            ctx.IsNeedRotation = true;
            ctx.IsHitted = false;
            _flag = false;
        }
        IEnumerator<float> _FlagCoroutine()
        {
            yield return Timing.WaitForSeconds(_length);
            _flag = true;
        }
    }
}