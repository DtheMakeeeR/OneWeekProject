using UnityEngine;
using Utilities;

namespace WeekProject
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = true)
            : base(ctx, factory, isRoot) { }

        public override void CheckSwitchStates()
        {
            if (Controller.IsJumpPressed)
            {
                SwitchState(Factory.Jump());
            }
            else if(!Controller.CharacterController.isGrounded)
            {
                SwitchState(Factory.Fall());
            }
        }

        public override void EnterState()
        {
            Debug.Log("Grounded State");
            Controller.MoveInputY = Controller.GroundGravity;
        }

        public override void ExitState()
        {
            Debug.Log("Exit Grounded");
        }

        public override void InitializeSubState()
        {
            if (!Controller.IsMovementPressed)
            {
                SetSubState(Factory.Idle());
            }
            else if (!Controller.IsSprintPressed)
            {
                SetSubState(Factory.Walk());
            }
            else
            {
                SetSubState(Factory.Sprint());
            }
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            Controller.MoveInputY = Controller.GroundGravity;
        }
    }
}
