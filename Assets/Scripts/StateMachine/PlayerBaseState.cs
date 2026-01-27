using UnityEngine;
namespace WeekProject
{
    public abstract class PlayerBaseState
    {
        private bool _isRootState = false;
        private PlayerStateMachine _ctx;
        private PlayerStateFactory _factory;
        private PlayerBaseState _currentSuperState;
        private PlayerBaseState _currentSubState;

        protected bool IsRootState { get =>  _isRootState; set => _isRootState = value; }
        protected PlayerStateMachine Ctx => _ctx;
        protected PlayerStateFactory Factory => _factory;
        protected PlayerBaseState CurrentSuperState => _currentSuperState;
        protected PlayerBaseState CurrentSubState => _currentSubState;
        protected PlayerController Controller { get => _ctx.PlayerController; }
        protected PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory factory, bool isRoot = false)
        {
            _ctx = ctx;
            _factory = factory;
            _isRootState = isRoot;
            InitializeSubState();
        }
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubState();

        protected void UpdateStates()
        {
            UpdateState();
            CurrentSubState?.UpdateStates();
        }
        protected void SwitchState(PlayerBaseState newState)
        {
            ExitState();

            newState.EnterState();

            if(_isRootState)
            {
                _ctx.CurrentState = newState;
            }
            else
            {
                CurrentSuperState?.SetSubState(newState);
            }
        }
        protected void SetSuperState(PlayerBaseState newSuperState)
        {
            _currentSuperState = newSuperState;
            //newSuperState.SetSubState (this);
        }
        protected void SetSubState(PlayerBaseState newSubState)
        {
            _currentSubState = newSubState;
            newSubState.SetSuperState(this);
        }

    }
}
