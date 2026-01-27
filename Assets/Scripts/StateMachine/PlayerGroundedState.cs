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
        }

        public override void EnterState()
        {
            Debug.Log("Grounded State");
            Controller.MoveInput = Controller.MoveInput.With(y: Controller.GroundGravity);
        }

        public override void ExitState()
        {
            Debug.Log("Exit Grounded");
        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }
    }
}
