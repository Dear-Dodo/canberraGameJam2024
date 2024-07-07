using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PowerUps
{
    public class LaserPowerUp : MonoBehaviour
    {
        public float Duration;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.GiveLaser(Duration);
                Destroy(gameObject);
            }
        }
    }
}
