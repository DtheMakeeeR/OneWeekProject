using UnityEngine;

namespace WeekProject
{
    public class PlayerLockedState : PlayerBaseState
    {
        public PlayerLockedState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = true)
            : base(ctx, factory, isRoot) { }
        public override void CheckSwitchStates()
        {
            if (!Controller.CameraController.IsLocked)
            {
                SwitchState(Factory.Free());
            }
        }

        public override void EnterState()
        {
            Debug.Log("ENTER LOCKED");
            Controller.Animator.SetBool(Controller.IsLockedHash, true);
        }

        public override void ExitState()
        {
            Controller.Animator.SetBool(Controller.IsLockedHash, false);
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
