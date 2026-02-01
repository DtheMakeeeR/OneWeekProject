using UnityEngine;

namespace WeekProject
{
    public class PlayerSprintState : PlayerBaseState
    {
        public PlayerSprintState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = false)
            : base(ctx, factory, isRoot) { }

        public override void CheckSwitchStates()
        {
            if (Controller.IsAttacking)
            {
                SwitchState(Factory.Attack());
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

        public override void EnterState()
        {
            Debug.Log("Sprint State");
            Controller.Animator.SetBool(Controller.IsSprintingHash, true);
            Controller.Animator.SetBool(Controller.IsWalkingHash, true);
        }

        public override void ExitState()
        {
            Controller.Animator.SetBool(Controller.IsWalkingHash, true);
            Controller.Animator.SetBool(Controller.IsSprintingHash, false);
            Debug.Log("Sprint Exit");

        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            Controller.MoveSpeed = Controller.WalkSpeed * Controller.SprintMultiplier;
        }
    }
}
