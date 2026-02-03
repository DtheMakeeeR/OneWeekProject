using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
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

        [Header("Rotation")]
        [SerializeField, Range(0.1f, 1f)]
        float _rotationFactorPerFrame;

        Rigidbody _rb;
        StateMachine _machine;
        State _root;
        string _lastPath;



        void Awake() {
            _input.Move += direction =>
            {
                ctx.move.x = direction.x;
                ctx.move.z = direction.y;
                //_isMovementPressed = _moveInput.With(y: 0).magnitude > 0.01f;
            };
            _input.Jump += val =>
            {
                ctx.jumpPressed = val;
            };
            _input.Sprint += val =>
            {
                ctx.sprintPressed = val;
            };
            _rb = gameObject.GetOrAdd<Rigidbody>();
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

            ctx.rb = _rb;
            ctx.anim = GetComponentInChildren<Animator>();
            ctx.renderer = GetComponent<Renderer>();

            _root = new PlayerRoot(null, ctx);
            var builder = new StateMachineBuilder(_root);
            _machine = builder.Build();
            
            // fallback: create a groundCheck just below the collider's bounds
            if (groundCheck == null) {
                var col = GetComponent<Collider>();
                var t = new GameObject("groundCheck").transform;
                t.SetParent(transform, false);
                var y = col ? (-col.bounds.extents.y + 0.01f) : -0.5f;
                t.localPosition = new Vector3(0, y, 0);
                groundCheck = t;
            }
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
            _input.EnablePlayerActions();
        }

        void FixedUpdate() {
            var v = _rb.linearVelocity;
            v.x = ctx.velocity.x;
            v.z = ctx.velocity.z;
            Debug.Log($"ctx.velocity.z{ctx.velocity.z}");
            _rb.linearVelocity = v;

            ctx.velocity.x = _rb.linearVelocity.x;
            ctx.velocity.z = _rb.linearVelocity.z;
            //Debug.Log($"###ctx.velocity.x: {ctx.velocity.x}");
            //Debug.Log($"###ctx.velocity.z: {ctx.velocity.z}");
            var maxSpeed = (ctx.moveSpeed * ctx.sprintCoef);
            var normalized = ctx.velocity.normalized;
            var XDIr = normalized.x;// Helpers.Remap(ctx.velocity.x, 0, ctx.moveSpeed * ctx.sprintCoef, 0, 1);
            var ZDIr = normalized.z; //Helpers.Remap(ctx.velocity.z, 0, ctx.moveSpeed * ctx.sprintCoef, 0, 1);
            var speedNormalized = ctx.velocity.magnitude / maxSpeed;
            ctx.anim.SetFloat("XDir", XDIr);
            ctx.anim.SetFloat("ZDir", ZDIr);
            ctx.anim.SetFloat("magnitude", speedNormalized);
            if (ctx.IsNeedRotation)
            {
                HandleRotation();
            }
        }

            void OnDrawGizmosSelected() {
            if (!drawGizmos || groundCheck == null) return;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }

        void HandleRotation()
        {
            if(!ctx.IsMoveInput)
            {
                return;
            }
            Vector3 lookAtPos = ctx.move.With(y: 0).normalized;
            Quaternion curRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(lookAtPos);
            transform.rotation = Quaternion.Slerp(curRot, targetRot, _rotationFactorPerFrame);
        }

        static string StatePath(State s) {
            return string.Join(" > ", s.PathToRoot().Reverse().Select(n => n.GetType().Name));
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
        public bool IsMoveInput => move.With(y: 0).sqrMagnitude >= 0.01f;
        public bool IsNeedRotation;
        public bool sprintPressed;

        public bool IsFalling => rb.linearVelocity.y < 0;
    }
}