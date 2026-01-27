using UnityEngine;

namespace WeekProject
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField]
        PlayerController _playerController;


        PlayerBaseState _currentState;
        PlayerStateFactory _states;

        public PlayerBaseState CurrentState { get => _currentState; set { _currentState = value; } }
        public PlayerController PlayerController => _playerController;
        private void Awake()
        {
            _states = new PlayerStateFactory(this);
            _currentState = _states.Grounded();
            _currentState.EnterState();
        }
        private void Update()
        {
            _currentState.UpdateState();
        }
    }
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory factory)
            : base(ctx, factory) { }

        public override void CheckSwitchStates()
        {
            
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            
        }
    }
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory factory)
            : base(ctx, factory) { }

        public override void CheckSwitchStates()
        {
            
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            
        }
    }
    public class PlayerRunState : PlayerBaseState
    {
        public PlayerRunState(PlayerStateMachine ctx, PlayerStateFactory factory)
            : base(ctx, factory) { }

        public override void CheckSwitchStates()
        {
            
        }

        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void InitializeSubState()
        {
            
        }

        public override void UpdateState()
        {
            
        }
    }
}
