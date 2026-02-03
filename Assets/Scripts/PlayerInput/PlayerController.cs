using System;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Utilities;
namespace WeekProject
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField]
        Camera _camera;

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
        [SerializeField]
        CameraController _cameraController;

        Vector3 _moveInput;

        float _moveSpeed;

        float _storedYVelocity = 0f;

        bool _needToRotate;

        bool _isMovementPressed = false;
        bool _isSprintPressed = false;
        bool _isAttacking = false;
        bool _isJumpPressed = false;

        int _isWalkingHash;
        int _isSprintingHash;
        int _isJumpingHash;
        int _isFallingHash;
        int _isLockedHash;
        int _isAttackingHash;
        int _mirrorSideWalkHash;
        int _XDirHash;
        int _ZDirHash;

        public Animator Animator { get => _animator; }
        public int IsWalkingHash { get => _isWalkingHash; }
        public int IsSprintingHash { get => _isSprintingHash; }
        public int IsJumpingHash { get => _isJumpingHash; }
        public int IsFallingHash { get => _isFallingHash; }
        public int IsLockedHash { get => _isLockedHash; }
        public int MirrorSideWalk { get => _mirrorSideWalkHash; }
        public int XDir { get => _XDirHash; }
        public int ZDir { get => _ZDirHash; }

        public CharacterController CharacterController { get => _characterController; }
        public CameraController CameraController { get => _cameraController; }
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
        public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }
        public int IsAttackingHash { get => _isAttackingHash; set => _isAttackingHash = value; }

        private void Awake()
        {
            _isWalkingHash = Animator.StringToHash("isWalking");
            _isSprintingHash = Animator.StringToHash("isSprinting");
            _isJumpingHash = Animator.StringToHash("isJumping");
            _isFallingHash = Animator.StringToHash("isFalling");
            _isLockedHash = Animator.StringToHash("isLocked");
            _isAttackingHash = Animator.StringToHash("isAttacking");
            _mirrorSideWalkHash = Animator.StringToHash("mirrorSideWalk");
            _ZDirHash = Animator.StringToHash("ZDir");
            _XDirHash = Animator.StringToHash("XDir");
        }


        private void Start()
        {
            _input.Move += direction =>
            {
                _moveInput.x = direction.x;
                _moveInput.z = direction.y;
                
                _animator.SetBool(MirrorSideWalk, direction.x < 0);
                _animator.SetFloat(_XDirHash, direction.x);          
                _animator.SetFloat(_ZDirHash, direction.y);
                Debug.Log($"direction.x: {direction.x}");
                Debug.Log($"direction.y: {direction.y}");
                Debug.Log($"_moveInput1: {_moveInput}");
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
            _input.Lock += () =>
            {
                _cameraController.FindTarget();
            };
            _input.EnablePlayerActions();
        }
        //
        private void LateUpdate()
        {
            HandleRotation();
            Move();
        }

        private void HandleRotation()
        {
            if(IsAttacking)
            {
                return;
            }
            Vector3 positionToLookAt;
            if (CameraController.IsLocked && !_needToRotate)
            {
                positionToLookAt = CameraController.Target.position.With(y:0) - transform.position.With(y: 0);
            }
            else 
            {
                positionToLookAt = ConvertToCameraSpace(_moveInput.With(y: 0));
            }            
            Quaternion currentRotation = transform.rotation;
            if (_isMovementPressed || CameraController.IsLocked)
            {
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame);
            }
        }

        private void Move()
        {
            Vector3 moveDir;
            moveDir = (_moveInput * _moveSpeed).With(y: _moveInput.y);
            moveDir.y = _moveInput.y;
            moveDir = ConvertToCameraSpace(moveDir);
            _characterController.Move(moveDir * Time.deltaTime);
        }

        private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
        {
            float curY = vectorToRotate.y;
            //ignores Y axis and 1f length
            Vector3 cameraForward = _camera.transform.forward.With(y:0).normalized;
            Vector3 cameraRight = _camera.transform.right.With(y: 0).normalized;

            Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
            Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

            Vector3 res = cameraForwardZProduct + cameraRightXProduct;
            res.y = curY;
            return res;
        }
    }
}
