using System;
using UnityEngine;
namespace WeekProject
{
    public class BasicEnemy : Entity
    {
        protected override void Die()
        {
            Destroy(gameObject);
        }
    }
}
