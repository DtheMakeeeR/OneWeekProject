using UnityEngine;

namespace WeekProject
{
    [RequireComponent(typeof(Collider),  typeof(Rigidbody))]
    public class HitBox : MonoBehaviour
    {
        [SerializeField]
        int _damageOnCollide;

        [SerializeField]
        bool _isActive = false;

        public bool IsActive { get => _isActive; set => _isActive = value; }
        public int DamageOnCollide { get => _damageOnCollide; set => _damageOnCollide = value; }

        private void OnTriggerEnter(Collider other)
        {
            if(!IsActive)
            {
                return;
            }
            HurtBox entity = other.GetComponent<HurtBox>();
            entity?.TakeDamage(DamageOnCollide);
        }
    }
    
}
