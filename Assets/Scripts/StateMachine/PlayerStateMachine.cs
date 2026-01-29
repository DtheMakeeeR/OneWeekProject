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
            _currentState = _states.Free();
            _currentState.EnterState();
        }
        private void Update()
        {
            _currentState.UpdateStates();
        }
    }
}
