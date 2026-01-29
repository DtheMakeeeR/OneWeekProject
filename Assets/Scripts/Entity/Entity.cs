using System;
using UnityEngine;

namespace WeekProject
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField]
        protected int health;
        [SerializeField]
        protected int maxHealth;

        public Action OnDeath;

        public int Health
        {
            get => health;
            protected set
            {
                health = value;
                if (health > maxHealth) health = maxHealth;
            }
        }
        public float HealthNormalized => (health / (float)maxHealth);
        private void Start()
        {
            health = maxHealth;
        }
        public virtual void TakeDamage(int amount)
        {
            Health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }
        
        protected abstract void Die();
    }
}
