using System;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Utilities;
namespace WeekProject
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        float _walkSpeed = 5f;
        [SerializeField]
        float _sprintMultiplier = 1.5f;

        [Header("Rotation")]
        [SerializeField, Range(0.1f, 1f)]
        float _rotationFactorPerFrame;

        [Header("Jumping")]
        [SerializeField]
        float _initialJumpVelocity;

        [Header("Gravity")]
        [SerializeField]
        float _gravity = -9.8f;
        [SerializeField]
        float _groundGravity = -0.5f;
        [SerializeField]
        float _fallMultiplier;

        [Header("References")]
        [SerializeField]
        InputReader _input;
        [SerializeField]
        CharacterController _characterController;
        [SerializeField]
        Animator _animator;

        Vector3 _moveInput;

        float _moveSpeed;

        float _storedYVelocity = 0f;

        bool _isMovementPressed = false;
        bool _isSprintPressed = false;
        bool _isAttacking = false;
        bool _isJumpPressed = false;
        bool _isJumping = false;

        int _isWalkingHash;
        int _isSprintingHash;
        int _isJumpingHash;
        int _isFallingHash;


        public Animator Animator { get => _animator; }
        public int IsWalkingHash { get => _isWalkingHash; }
        public int IsSprintingHash { get => _isSprintingHash; }
        public int IsJumpingHash { get => _isJumpingHash; }
        public int IsFallingHash { get => _isFallingHash; }

        public CharacterController CharacterController { get => _characterController; }
        public bool IsMovementPressed { get => _isMovementPressed; }
        public bool IsSprintPressed { get => _isSprintPressed; }
        public Vector3 MoveInput { get => _moveInput; set => _moveInput = value; }
        public float MoveInputX { get => _moveInput.x; set => _moveInput.x = value; }
        public float MoveInputY { get => _moveInput.y; set => _moveInput.y = value; }
        public float MoveInputZ { get => _moveInput.z; set => _moveInput.z = value; }

        public float WalkSpeed { get => _walkSpeed; }
        public float SprintMultiplier { get => _sprintMultiplier; }
        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
        public float Gravity { get => _gravity; set => _gravity = value; }
        public float GroundGravity { get => _groundGravity; set => _groundGravity = value; }
        public float FallMultiplier => _fallMultiplier;
        public float InitialJumpVelocity { get => _initialJumpVelocity; set => _initialJumpVelocity = value; }
        public float StoredYVelocity { get => _storedYVelocity; set => _storedYVelocity = value; }


        public bool IsJumpPressed { get => _isJumpPressed; set => _isJumpPressed = value; }


        private void Awake()
        {
            _isWalkingHash = Animator.StringToHash("isWalking");
            _isSprintingHash = Animator.StringToHash("isSprinting");
            _isJumpingHash = Animator.StringToHash("isJumping");
            _isFallingHash = Animator.StringToHash("isFalling");
        }


        private void Start()
        {
            _input.Move += direction =>
            {
                _moveInput.x = direction.x;
                _moveInput.z = direction.y;
                //Debug.Log($"direction.x: {direction.x}");
                //Debug.Log($"direction.y: {direction.y}");
                //Debug.Log($"_moveInput1: {_moveInput}");
                _moveInput = _moveInput.With(y: 0).normalized.With(y: _moveInput.y);
                //Debug.Log($"_moveInput2: {_moveInput}");
                _isMovementPressed = _moveInput.With(y:0).magnitude > 0.01f;
                //Debug.Log($"_moveInput3: {_moveInput}");
            };
            _input.Sprint += val => _isSprintPressed = val;
            _input.Attack += val => _isAttacking = val;
            _input.Jump += val =>
            {
                _isJumpPressed = val;
                Debug.Log("JUMP PRESSED");
            };
            _input.EnablePlayerActions();
        }
        private void LateUpdate()
        {
            HandleRotation();
            //HandleAnimation();
            Move();
            //HandleGravity();
            //HandleJump();
        }

        private void HandleRotation()
        {
            Vector3 positionToLookAt;
            positionToLookAt = _moveInput.With(y: 0);
            Quaternion currentRotation = transform.rotation;
            if (_isMovementPressed)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame);
            }
        }

        private void Move()
        {
            Vector3 moveDir;
            //if (_isSprintPressed) 
            //{ 
            //    moveDir = (_moveInput * _moveSpeed).With(y: 0) * _sprintMultiplier;
            //    moveDir.y = _moveInput.y;
            //}
            //else
            //{
            //    moveDir = (_moveInput * _moveSpeed).With(y: _moveInput.y);
            //    moveDir.y = _moveInput.y;
            //}
            moveDir = (_moveInput * _moveSpeed).With(y: _moveInput.y);
            moveDir.y = _moveInput.y;
            _characterController.Move(moveDir * Time.deltaTime);
        }
    }
}
