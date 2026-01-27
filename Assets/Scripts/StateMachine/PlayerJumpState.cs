using UnityEngine;
using Utilities;

namespace WeekProject
{
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = true)
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
            Debug.Log("Jump State");
            HandleJump();
        }

        private void HandleJump()
        {
            Controller.Animator.SetBool(Controller.IsJumpingHash, true);
            Controller.MoveInput = Controller.MoveInput.With(y: Controller.InitialJumpVelocity);
            Controller.StoredYVelocity = Controller.InitialJumpVelocity;
        }
        private void HandleGravity()
        {
            bool isFalling = Controller.MoveInput.y <= 0.0f || !Controller.IsJumpPressed;
            if (isFalling)
            {
                float previousYVelocity = Controller.StoredYVelocity;
                Controller.StoredYVelocity = Controller.StoredYVelocity + (Controller.Gravity * Time.deltaTime * Controller.FallMultiplier);
                float nextYVelocity = (previousYVelocity + Controller.StoredYVelocity) * 0.5f;
                Controller.MoveInput = Controller.MoveInput.With(y:nextYVelocity);
            }
            else
            {
                float previousYVelocity = Controller.StoredYVelocity;
                Controller.StoredYVelocity = Controller.StoredYVelocity + (Controller.Gravity * Time.deltaTime);
                float nextYVelocity = (previousYVelocity + Controller.StoredYVelocity) * 0.5f;
                Controller.MoveInput = Controller.MoveInput.With(y: nextYVelocity);
            }
        }
        public override void ExitState()
        {
            Debug.Log("Exit Jump");
            Controller.Animator.SetBool(Controller.IsJumpingHash, false);
        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
            HandleGravity();
        }
    }
}
