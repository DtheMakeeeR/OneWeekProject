using UnityEngine;
namespace WeekProject
{ 
    public class PlayerFreeState : PlayerBaseState
    {
        public PlayerFreeState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = true)
            : base(ctx, factory, isRoot) { }
        public override void CheckSwitchStates()
        {
            if(Controller.CameraController.IsLocked)
            {
                SwitchState(Factory.Locked());
            }
        }

        public override void EnterState()
        {
            Debug.Log("ENTER FREE");
            Controller.Animator.SetBool(Controller.IsLockedHash, false);
        }

        public override void ExitState()
        {
            Controller.Animator.SetBool(Controller.IsLockedHash, true);
        }

        public override void InitializeSubState()
        {
            if (!Controller.CharacterController.isGrounded)
            {
                Debug.Log("FREE STATE CREATES FALL");
                SetSubState(Factory.Fall());
            }
            else
            {
                Debug.Log("FREE STATE CREATES GROUND");
                SetSubState(Factory.Grounded());
            }
        }

        public override void UpdateState()
        {
            CheckSwitchStates();
        }
    }
}
