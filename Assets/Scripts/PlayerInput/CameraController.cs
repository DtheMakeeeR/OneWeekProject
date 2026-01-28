using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UIElements;
using System.Linq;
namespace WeekProject
{
    public class CameraController : MonoBehaviour
    {
        [Header("Cameras")]
        [SerializeField]
        Camera _mainCamera;
        [SerializeField]
        CinemachineCamera _cinemachine;

        [Header("Camera Ofset")]
        [SerializeField]
        Vector3 _cameraOfset;

        [Header("Variables")]
        [SerializeField, Range(.01f, 1f)]
        float _smooth;
        [SerializeField]
        float _findingRange = 20f;
        [SerializeField]
        LayerMask _layerMask;


        Transform _target;
        bool _isLocked;

        public bool IsLocked
        {
            get { return _isLocked; }

            set
            {
                _isLocked = value;
                if(IsLocked)
                {
                    _cinemachine.enabled = false;
                }
                else
                {
                    _target = null;
                    _cinemachine.enabled = true;
                }
            }
        }
        void Update()
        {
            if(IsLocked)
            {
                RotateToTarget();
            }
        }

        private void RotateToTarget()
        {
            var lookDir = _target.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            _mainCamera.transform.rotation = Quaternion.Slerp(_mainCamera.transform.rotation, lookRot, _smooth);
        }
        public void FindTarget()
        {
            if(IsLocked)
            {
                IsLocked = false;
                return;
            }
            var enemies = Physics.OverlapSphere(_mainCamera.transform.position, _findingRange, _layerMask);
            var lockTargets = from enemy in enemies where enemy.tag == "LockTarget" select enemy.transform;
            _target = lockTargets.FirstOrDefault();
            Debug.Log($"lockTagets.Count:{lockTargets.Count()}");
            if (_target != null)
            {
                IsLocked = true;
            }
        }
    }
}
