using UnityEngine;
using Utilities;

namespace WeekProject
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = true)
            : base(ctx, factory, isRoot) { }

        bool IsFalling => Controller.MoveInput.y <= 0.0f || !Controller.IsJumpPressed;

        public override void CheckSwitchStates()
        {
            if(Controller.CharacterController.isGrounded)
            {
                SwitchState(Factory.Grounded());
            }
            else if(IsFalling)
            {
                SwitchState(Factory.Fall());
            }    
        }

        public override void EnterState()
        {
            Debug.Log("Jump State");
            HandleJump();
        }

        private void HandleJump()
        {
            Controller.Animator.SetBool(Controller.IsJumpingHash, true);
            Controller.MoveInputY = Controller.InitialJumpVelocity;
            Controller.StoredYVelocity = Controller.InitialJumpVelocity;
        }
        private void HandleGravity()
        {
            if(!IsFalling)
            {
                float previousYVelocity = Controller.StoredYVelocity;
                Controller.StoredYVelocity = Controller.StoredYVelocity + (Controller.Gravity * Time.deltaTime);
                float nextYVelocity = (previousYVelocity + Controller.StoredYVelocity) * 0.5f;
                Controller.MoveInputY = nextYVelocity;
            }
        }
        public override void ExitState()
        {
            Debug.Log("Exit Jump");
            Controller.Animator.SetBool(Controller.IsJumpingHash, false);
        }

        public override void InitializeSubState()
        {
            if(!Controller.IsMovementPressed)
            {
                SetSubState(Factory.Idle());
            }
            else if(!Controller.IsSprintPressed)
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
            HandleGravity();
        }
    }
}
