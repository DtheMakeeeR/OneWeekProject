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
        float _moveSpeed = 5f;
        [SerializeField]
        float _sprintMultiplier = 1.5f;

        [Header("Rotation")]
        [SerializeField, Range(0.1f, 1f)]
        float _rotationFactorPerFrame;

        [Header("Jumping")]
        [SerializeField]
        float _initialJumpVelocity;
        [SerializeField]
        float _maxJumpHeight;
        [SerializeField]
        float _maxJumpTime;
        [SerializeField]
        float _timeToApex = 0.3f;

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

        float _storedYVelocity = 0f;

        bool _isMovementPressed = false;
        bool _isSprintPressed = false;
        bool _isAttacking = false;
        bool _isJumpPressed = false;
        bool _isJumping = false;

        int _isWalkingHash;
        int _isSprintingHash;
        int _isJumpingHash;

        private void Awake()
        {
            _isWalkingHash = Animator.StringToHash("isWalking");
            _isSprintingHash = Animator.StringToHash("isSprinting");
            _isJumpingHash = Animator.StringToHash("isJumping");
            //PrepareJumpVariables();
        }


        private void Start()
        {
            _input.Move += direction =>
            {
                _moveInput.x = direction.x;
                _moveInput.z = direction.y;
                _isMovementPressed = _moveInput.With(y:0).magnitude > 0.01f;
            };
            _input.Sprint += val => _isSprintPressed = val;
            _input.Attack += val => _isAttacking = val;
            _input.Jump += val => _isJumpPressed = val;
            _input.EnablePlayerActions();
        }
        private void Update()
        {
            HandleRotation();
            HandleAnimation();
            Move();
            HandleGravity();
            HandleJump();
        }

        private void HandleAnimation()
        {
            bool isWalking = _animator.GetBool(_isWalkingHash);
            bool isSprinting = _animator.GetBool(_isSprintingHash);

            if(_isMovementPressed && !isWalking)
            {
                _animator.SetBool(_isWalkingHash, true);
            }
            else if(!_isMovementPressed && isWalking)
            {
                _animator.SetBool(_isWalkingHash, false);
            }
            if((_isMovementPressed && _isSprintPressed) && !isSprinting)
            {
                _animator.SetBool(_isSprintingHash, true);
            }
            else if((!_isMovementPressed || !_isSprintPressed) && isSprinting)
            {
                _animator.SetBool(_isSprintingHash, false);
            }
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

        private void HandleGravity()
        {
            bool isFalling = _moveInput.y <= 0.0f || !_isJumpPressed;
            if(_characterController.isGrounded)
            {
                _animator.SetBool(_isJumpingHash, false);
                _moveInput.y = _groundGravity;
            }
            else if(isFalling)
            {
                float previousYVelocity = _storedYVelocity;
                _storedYVelocity = _storedYVelocity + (_gravity * Time.deltaTime * _fallMultiplier);
                float nextYVelocity = (previousYVelocity + _storedYVelocity) * 0.5f;
                _moveInput.y = nextYVelocity;
            }
            else
            {
                Debug.Log("***");
                float previousYVelocity = _storedYVelocity;
                _storedYVelocity = _storedYVelocity + (_gravity * Time.deltaTime);
                float nextYVelocity = (previousYVelocity + _storedYVelocity) * 0.5f;
                _moveInput.y = nextYVelocity;
            }
        }

        private void HandleJump()
        {
            Debug.Log($"IsGounded:{_characterController.isGrounded}");
            Debug.Log($"_isJumpPressed:{_isJumpPressed}");
            Debug.Log($"_isJumping:{_isJumping}");
            if(!_isJumping && _isJumpPressed && _characterController.isGrounded)
            {
                _isJumping = true;
                _animator.SetBool(_isJumpingHash, true);
                //float previousYVelocity = _moveInput.y;
                //float newYVelocity = _moveInput.y + _initialJumpVelocity;
                //float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;

                //previous velocity is zero cuz was grounded
                _moveInput.y = _initialJumpVelocity;
                _storedYVelocity = _initialJumpVelocity;
            }
            else if(_isJumping && _characterController.isGrounded)
            {
                _isJumping = false;
            }
        }

        private void Move()
        {
            var moveDir = (_moveInput * _moveSpeed).With(y:_moveInput.y);
            if (_isSprintPressed) 
            { 
                moveDir *= _sprintMultiplier;
            }
            _characterController.Move(moveDir * Time.deltaTime);
        }


    }
}
