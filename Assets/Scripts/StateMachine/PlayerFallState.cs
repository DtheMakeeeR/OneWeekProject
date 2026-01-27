using UnityEngine;

namespace WeekProject
{
    public class PlayerFallState : PlayerBaseState
    {
        public PlayerFallState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = true)
            : base(ctx, factory, isRoot) { }
        public override void CheckSwitchStates()
        {
            if(Controller.CharacterController.isGrounded)
            {
                SwitchState(Factory.Grounded());
            }
        }

        public override void EnterState()
        {
            Debug.Log("Fall State");
            Controller.Animator.SetBool(Controller.IsFallingHash, false);
        }

        public override void ExitState()
        {
            Debug.Log("Exit Fall");
            Controller.Animator.SetBool(Controller.IsFallingHash, true);
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
            HandleGravity();
        }
        private void HandleGravity()
        {
            float previousYVelocity = Controller.StoredYVelocity;
            Controller.StoredYVelocity = Controller.StoredYVelocity + (Controller.Gravity * Time.deltaTime * Controller.FallMultiplier);
            float nextYVelocity = (previousYVelocity + Controller.StoredYVelocity) * 0.5f;
            Controller.MoveInputY = nextYVelocity;
        }
    }
}
