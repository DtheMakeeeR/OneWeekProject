using UnityEngine;
using MEC;
using System.Collections.Generic;
namespace WeekProject
{
    public class PlayerAttackState : PlayerBaseState
    {
        bool _isAnimationEnd = false;
        float ANIMATION_DUR = 0.7f;
        public PlayerAttackState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = true)
            : base(ctx, factory, isRoot) { }
        public override void CheckSwitchStates()
        {
            if(!_isAnimationEnd) return;
            else
            {
                if (Controller.IsMovementPressed && Controller.IsSprintPressed)
                {
                    SwitchState(Factory.Sprint());
                }
                else if (Controller.IsMovementPressed && !Controller.IsSprintPressed)
                {
                    SwitchState(Factory.Walk());
                }
                else if (!Controller.IsMovementPressed)
                {
                    SwitchState(Factory.Idle());
                }
            }
        }

        public override void EnterState()
        {
            Timing.RunCoroutine(_AnimCoroutine());
            Controller.Animator.SetBool(Controller.IsAttackingHash, true);
        }

        public override void ExitState()
        {
            Controller.Animator.SetBool(Controller.IsAttackingHash, false);

        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            Controller.MoveInputX = 0f;
            Controller.MoveInputZ = 0f;
        }
        IEnumerator<float> _AnimCoroutine()
        {
            yield return Timing.WaitForSeconds(ANIMATION_DUR);
            _isAnimationEnd =true;
        }
    }
}
