using UnityEngine;

namespace WeekProject
{
    [CreateAssetMenu(fileName = "WeaponAttackInfo", menuName = "Scriptable Objects/WeaponAttackInfo")]
    public class WeaponAttackInfo : ScriptableObject
    {
        [SerializeField] 
        AnimationClip clip;
        void Start ()
        {
            Animator a = new Animator();
            AnimatorOverrideController a2 = new AnimatorOverrideController(a.runtimeAnimatorController);

        }
    }
}
