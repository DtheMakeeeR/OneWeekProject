using UnityEngine;

namespace WeekProject
{
    public class PlayerStateMachine : MonoBehaviour
    {
        PlayerBaseState _currentState;
        PlayerStateFactory _states;
        private void Awake()
        {
            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterState();
        }
    }
    public abstract class PlayerBaseState
    {
        PlayerStateMachine _ctx;
        PlayerStateFactory _factory;
        protected PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory factory)
        {
            _ctx = ctx;
            _factory = factory;
        }
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubState();

        private void UpdateStates();
        private void SwitchState();
        private void SetSuperState();
        private void SetSubStates();

    }
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine ctx, PlayerStateFactory factory)
            : base(ctx, factory) { }

        public override void CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
    public class PlayerJumpState : PlayerBaseState
    {
        public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory factory)
            : base(ctx, factory) { }

        public override void CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory)
            : base(ctx, factory) { }

        public override void CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory)
            : base(ctx, factory) { }

        public override void CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }
    public class PlayerRunState : PlayerBaseState
    {
        public PlayerRunState(PlayerStateMachine ctx, PlayerStateFactory factory)
            : base(ctx, factory) { }

        public override void CheckSwitchStates()
        {
            throw new System.NotImplementedException();
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void InitializeSubState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }
    }

    public class PlayerStateFactory
    {
        PlayerStateMachine _context;
        public PlayerStateFactory(PlayerStateMachine currentContext)
        {
            _context = currentContext;
        }
        public PlayerBaseState Idle()
        {
            return new PlayerIdleState();
        }
        public PlayerBaseState Walk()
        {
            return new PlayerWalkState();
        }
        public PlayerBaseState Run()
        {
            return new PlayerRunState();
        }
        public PlayerBaseState Jump()
        {
            return new PlayerJumpState();
        }
        public PlayerBaseState Grounded()
        {
            return new PlayerGroundedState();
        }
    }
}
