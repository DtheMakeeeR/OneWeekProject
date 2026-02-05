using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Utilities;
using WeekProject;

namespace HSM {
    public class PlayerStateDriver : MonoBehaviour {
        public PlayerContext ctx = new PlayerContext();
        public Transform groundCheck;
        public float groundRadius = 0.2f;
        public LayerMask groundMask;
        public bool drawGizmos = true;

        [Header("References")]
        [SerializeField]
        InputReader _input;
        [SerializeField]
        Camera _cam;
        [SerializeField]
        CameraController _camController;
        [SerializeField]
        Rigidbody _rb;

        [Header("Rotation")]
        [SerializeField, Range(0.1f, 1f)]
        float _rotationFactorPerFrame;

        StateMachine _machine;
        State _root;
        string _lastPath;



        void Awake() {
            
            _input.Jump += val =>
            {
                ctx.jumpPressed = val;
            };
            _input.Sprint += val =>
            {
                ctx.sprintPressed = val;
            };
            _input.Move += direction =>
            {
                ctx.move.x = direction.x;
                ctx.move.z = direction.y;
                //var coef = ctx.sprintPressed ? ctx.sprintCoef : 1;
                //var XDir = Helpers.Remap(direction.x * coef, 0, 1 * ctx.sprintCoef, 0, 1);
                //var ZDir = Helpers.Remap(direction.y * coef, 0, 1 * ctx.sprintCoef, 0, 1);
                //ctx.anim.SetFloat("XDir", XDir);
                //ctx.anim.SetFloat("ZDir", ZDir);
            };
            _input.Lock += () =>
            {
                _camController.FindTarget();
                ctx.anim.SetBool("isLocked", _camController.IsLocked);
            };
            _input.Attack += val =>
            {
                ctx.IsAttacking = val;
            };

            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;


            _root = new PlayerRoot(null, ctx);
            var builder = new StateMachineBuilder(_root);
            _machine = builder.Build();
            _input.EnablePlayerActions();

            // fallback: create a groundCheck just below the collider's bounds
        }

        private void Start()
        {
            
        }

        void Update() {
            ctx.move.x = Mathf.Clamp(ctx.move.x, -1f, 1f);
            ctx.move.z = Mathf.Clamp(ctx.move.z, -1f, 1f);

            ctx.grounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
            //Debug.Log($"###CheckSphere:{Physics.CheckSphere(groundCheck.position, groundRadius, groundMask)}");
            //Debug.Log($"###Mathf.Abs(_rb.linearVelocity.y):{Mathf.Abs(_rb.linearVelocity.y)}");
            _machine.Tick(Time.deltaTime);

            var path = StatePath(_machine.Root.Leaf());

            if (path != _lastPath) {
                Debug.Log("State" + path);
                _lastPath = path;
            }
        }

        void FixedUpdate()
        {
            var v = _rb.linearVelocity;
            var convertedVel = ConvertToCameraSpace(ctx.velocity);


            //if true will change direction with camera rotation
            //else will contain move diraciton
            if (ctx.IsNeedChangeVel)
            {
                v.x = convertedVel.x;
                v.z = convertedVel.z;
                _rb.linearVelocity = v;
            }

            SetAnimParameters();

            if (ctx.IsNeedRotation)
            {
                HandleRotation();
            }
        }

        private void SetAnimParameters()
        {
            var coef = ctx.sprintPressed ? ctx.sprintCoef : 1;
            var XDir = Helpers.Remap(ctx.move.x * coef, 0, 1 * ctx.sprintCoef, 0, 1);
            var ZDir = Helpers.Remap(ctx.move.z * coef, 0, 1 * ctx.sprintCoef, 0, 1);
            ctx.anim.SetFloat("XDir", XDir);
            ctx.anim.SetFloat("ZDir", ZDir);
            var maxSpeed = (ctx.moveSpeed * ctx.sprintCoef);
            var speedNormalized = _rb.linearVelocity.magnitude / maxSpeed;
            ctx.anim.SetFloat("magnitude", speedNormalized);
        }

        void OnDrawGizmosSelected() {
            if (!drawGizmos || groundCheck == null) return;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }

        void HandleRotation()
        {
            Debug.Log($"!ctx.IsMoveInput && !_camController.IsLocked: {!ctx.IsMoveInput} && {!_camController.IsLocked}");
            if (!ctx.IsMoveInput && !_camController.IsLocked)
            {
                Debug.Log($"RETURNED ROT islocked:{_camController.IsLocked}");
                return;
            }
            Vector3 lookAtPos;
            if(_camController.IsLocked)
            {
                lookAtPos = _camController.Target.position.With(y: 0) - transform.position.With(y: 0);
            }
            else
            {
                lookAtPos = _rb.linearVelocity.With(y: 0).normalized;
            }
            Quaternion curRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(lookAtPos);
            transform.rotation = Quaternion.Slerp(curRot, targetRot, _rotationFactorPerFrame);
        }

        static string StatePath(State s) {
            return string.Join(" > ", s.PathToRoot().Reverse().Select(n => n.GetType().Name));
        }
        private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
        {
            float curY = vectorToRotate.y;
            //ignores Y axis and 1f length
            Vector3 cameraForward = _cam.transform.forward.With(y: 0).normalized;
            Vector3 cameraRight = _cam.transform.right.With(y: 0).normalized;

            Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
            Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

            Vector3 res = cameraForwardZProduct + cameraRightXProduct;
            res.y = curY;
            return res;
        }
    }

    [Serializable]
    public class PlayerContext {
        public Vector3 move;
        public Vector3 velocity;
        public bool grounded;
        public float moveSpeed = 6f;
        public float sprintCoef = 1.5f;
        public float accel = 40f;
        public float jumpSpeed = 7f;
        public bool jumpPressed;
        public Animator anim;
        public Rigidbody rb;
        public Renderer renderer;

        public bool IsNeedChangeVel = true;
        public bool IsMoveInput => move.With(y: 0).sqrMagnitude >= 0.01f;
        public bool IsNeedRotation = true;
        public bool sprintPressed;

        public bool IsAttacking = false;
        public bool IsFalling => rb.linearVelocity.y < 0;
    }
}