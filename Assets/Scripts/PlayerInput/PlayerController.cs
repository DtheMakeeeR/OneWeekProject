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
        float _sprintMultiplyier = 1.5f;

        [Header("Rotation")]
        [SerializeField, Range(0.1f, 1f)]
        float _rotationFactorPerFrame;

        [Header("References")]
        [SerializeField]
        InputReader _input;
        [SerializeField]
        CharacterController _characterController;
        [SerializeField]
        Animator _animator;

        Vector3 _moveInput;
        bool _isMovementPressed;
        bool _isSprintPressed;
        bool _isAttacking;

        int _isWalkingHash;
        int _isSprintingHash;


        private void Awake()
        {
            _isWalkingHash = Animator.StringToHash("isWalking");
            _isSprintingHash = Animator.StringToHash("isSprinting");
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
            _input.EnablePlayerActions();
        }
        private void Update()
        {
            HandleGravity();
            HandleRotation();
            HandleAnimation();
            Move();
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
            if(_characterController.isGrounded)
            {
                _moveInput.y = -0.5f;
            }
            else
            {
                _moveInput.y = -9.8f;

            }
        }

        private void Move()
        {
            var moveDir = _moveInput * _moveSpeed;
            if (_isSprintPressed) 
            { 
                moveDir *= _sprintMultiplyier;
            }
            _characterController.Move(moveDir * Time.deltaTime);
        }


    }
}
