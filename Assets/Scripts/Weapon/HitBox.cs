using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

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

        public List<GameObject> Hitted;
        private void OnTriggerEnter(Collider other)
        {
            if (!IsActive)
            {
                return;
            }
            if (Hitted.Find(obj => obj == other.gameObject) != null) return;
            Hitted.Add(other.gameObject);
            
            HurtBox hurtBox = other.GetComponent<HurtBox>();
            hurtBox?.TakeDamage(DamageOnCollide);
        }
        public void Refresh()
        {
            Hitted.Clear();
        }
    }
    
}
