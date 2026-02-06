using UnityEngine;
using Utilities;

namespace WeekProject
{
    public class Player : Entity
    {
        protected override void Die()
        {
            Debug.Log("Player Died");
            OnDeath?.Invoke();
        }
        public void AddHealth(int amount)
        {
            Health += amount;
        }
    }
}
