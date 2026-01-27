using UnityEngine;

namespace WeekProject
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = false)
            : base(ctx, factory, isRoot) { }

        public override void CheckSwitchStates()
        {
            if(Controller.IsMovementPressed && Controller.IsSprintPressed)
            {
                SwitchState(Factory.Sprint());
            }
            else if(Controller.IsMovementPressed)
            {
                SwitchState(Factory.Walk());
            }
        }

        public override void EnterState()
        {
            Debug.Log("Idle State");
            Controller.Animator.SetBool(Controller.IsSprintingHash, false);
            Controller.Animator.SetBool(Controller.IsWalkingHash, false);
        }

        public override void ExitState()
        {
            Debug.Log("Idle Exit");

        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            //Controller.MoveSpeed = 0f;
        }
    }
}
