using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PowerUps
{
    public class HealthPowerUp : MonoBehaviour
    {
        public float Health;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.Heal(Health);
                Destroy(gameObject);
            }
        }
    }
}
