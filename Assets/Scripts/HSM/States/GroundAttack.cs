using UnityEditor.Animations;
using UnityEngine;
using Utilities;
using System.Linq;
using System.Collections.Generic;
using MEC;

namespace HSM {
    public class GroundAttack : State {
        readonly PlayerContext ctx;
        float _length;
        bool _flag = false;

        public GroundAttack(StateMachine m, State parent, PlayerContext ctx) : base(m, parent) {
            this.ctx = ctx;
            Add(new AnimatorBoolActivity(ctx.anim, "isAttacking", true, false));
            AnimatorController ac = (AnimatorController)ctx.anim.runtimeAnimatorController;
            AnimatorStateMachine combatMachine = ac.layers[0].stateMachine.stateMachines
                .First(sm => sm.stateMachine.name == "Attacking").stateMachine;

            AnimatorState groundAttackState = combatMachine.states
                .First(s => s.state.name == "Ground Attack").state;

            AnimationClip clip = groundAttackState.motion as AnimationClip;
            _length = clip.length;
        }

        protected override State GetTransition()
        {
            if (ctx.IsHitted)
            {
                return ((PlayerRoot)((Attacking)Parent).Parent).Hitted;
            }
            if (!_flag)
            {
                return null;
            }
            else if(ctx.IsMoveInput)
            {
                foreach(State state in PathToRoot())
                {
                    Debug.Log($"state: {state}");
                    if(state is PlayerRoot root)
                    {
                        return root.Grounded.Move;
                    }
                }
            }
            else
            {
                foreach (State state in PathToRoot())
                {
                    Debug.Log($"state: {state}");
                    if (state is PlayerRoot root)
                    {
                        return root.Grounded.Idle;
                    }
                }
            }
            return null;
        }

        protected override void OnEnter()
        {
            ctx.IsNeedRotation = false;
            ctx.swordHitBox.Refresh();
            ctx.swordHitBox.IsActive = true;
            ctx.velocity.x = 0f;
            ctx.velocity.z = 0f;
            Timing.RunCoroutine(_FlagCoroutine());
        }
        protected override void OnExit()
        {
            _flag = false;
            ctx.IsNeedRotation = true;
            ctx.swordHitBox.IsActive = false;
        }
        IEnumerator<float> _FlagCoroutine()
        {
            yield return Timing.WaitForSeconds(_length);
            _flag = true;
        }
    }
}