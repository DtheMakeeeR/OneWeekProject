using UnityEngine;

namespace WeekProject
{
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = false)
            : base(ctx, factory, isRoot) { }

        public override void CheckSwitchStates()
        {
            if (Controller.IsMovementPressed && Controller.IsSprintPressed)
            {
                SwitchState(Factory.Sprint());
            }
            else if (!Controller.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
            }
        }

        public override void EnterState()
        {
            Debug.Log("Walk State");
            Controller.Animator.SetBool(Controller.IsSprintingHash, false);
            Controller.Animator.SetBool(Controller.IsWalkingHash, true);
        }

        public override void ExitState()
        {
            Debug.Log("Walk Exit");
        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            Debug.Log($"Walk State: MoveInput:{Controller.MoveInput}");
            CheckSwitchStates();
            Controller.MoveSpeed = Controller.WalkSpeed;
        }
    }
}
