using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static WeekProject.InputSystem_Actions;

namespace WeekProject
{
    public interface IInputReader
    {
        Vector2 Direction { get; }
        void EnablePlayerActions();
    }
    [CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, IInputReader, IPlayerActions
    {
        public UnityAction<Vector2> Move = delegate { };
        public UnityAction<bool> Jump = delegate { };
        public UnityAction<bool> Attack = delegate { };
        public UnityAction<bool> Sprint = delegate { };
        public UnityAction Lock = delegate { };

        InputSystem_Actions _inputActions;
        public Vector2 Direction => _inputActions.Player.Move.ReadValue<Vector2>();

        public void EnablePlayerActions()
        {
            if (_inputActions == null)
            {
                _inputActions = new InputSystem_Actions();
                _inputActions.Player.SetCallbacks(this);
            }
            _inputActions.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
            Debug.Log("MOVING");
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    Attack?.Invoke(true);
                    break;
                default:
                    Attack?.Invoke(false);
                    break;
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    Jump?.Invoke(true);
                    break;
                default:
                    Jump?.Invoke(false);
                    break;
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            //noop
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    Sprint?.Invoke(true);
                    break;
                default:
                    Sprint?.Invoke(false);
                    break;
            }
        }

        public void OnLock(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    Lock?.Invoke();
                    break;
                default:
                    Lock?.Invoke();
                    break;
            }
        }
    }
}