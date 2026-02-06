using UnityEngine;
using System;
namespace WeekProject
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(Entity))]
    public class HurtBox : MonoBehaviour
    {
        [Header("Entity")]
        [SerializeField]
        Entity _entity;
        [SerializeField]
        bool _isActive = false;
        public Action Callback;

        public bool IsActive { get => _isActive; set => _isActive = value; }

        public void TakeDamage(int amount)
        {
            if(IsActive)
            {
                _entity.TakeDamage(amount);
                Callback?.Invoke();
            }            
        }
    }
    
}
