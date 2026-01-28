using UnityEngine;

namespace WeekProject
{
    public class IKFoots : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField]
        Animator _animator;
        [Header("Measure Variables")]
        [SerializeField]
        float _distanceToGround;
        [SerializeField]
        float _distanceToMidOfFoot;

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        private void OnAnimatorIK(int layerIndex)
        {
            RaycastHit hit;
            if (Physics.Raycast(_animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down, out hit, _distanceToGround + 1f))
            {
                Vector3 footPosition = hit.point;
                footPosition.y = _distanceToGround;
                _animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                _animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }
            if (Physics.Raycast(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down, out hit, _distanceToGround + 1f))
            {
                Vector3 footPosition = hit.point;
                footPosition.y = _distanceToGround;
                _animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                _animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }
        }


    }
}
