using HSM;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;
using MEC;

namespace HSM
{
    public class WaitAnimationActivity : Activity
    {
        readonly PlayerContext context;
        readonly string _subMachine;
        readonly string _state;

        bool flag;

        CoroutineHandle _coroutine;

        public WaitAnimationActivity(PlayerContext ctx, string subMachine, string state)
        {
            context = ctx;
            AnimatorController ac = (AnimatorController)ctx.anim.runtimeAnimatorController;
            AnimatorStateMachine combatMachine = ac.layers[0].stateMachine.stateMachines
                .First(sm => sm.stateMachine.name == subMachine).stateMachine;

            AnimatorState groundAttackState = combatMachine.states
                .First(s => s.state.name == state).state;

            AnimationClip clip = groundAttackState.motion as AnimationClip;
            float length = clip.length;
            Debug.Log($"Длина: {length}");
        }

        public override async Task ActivateAsync(CancellationToken ct)
        {
            Debug.Log("ActivateAsync");
            if (this.Mode != ActivityMode.Inactive || context == null) return;
            this.Mode = ActivityMode.Activating;
            context.IsNeedChangeVel = false;
            context.IsNeedRotation = false;
            this.Mode = ActivityMode.Active;
        }

        public override async Task DeactivateAsync(CancellationToken ct)
        {
            if (this.Mode != ActivityMode.Active || context == null) return;
            this.Mode = ActivityMode.Deactivating;
            context.IsNeedChangeVel = true;
            context.IsNeedRotation = true;
            this.Mode = ActivityMode.Inactive;
        }
    }
}
