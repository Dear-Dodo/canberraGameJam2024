using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PowerUps
{
    public class DashPowerUp : MonoBehaviour
    {
        public int Count;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.GiveDashs(Count);
                Destroy(gameObject);
            }
        }
    }
}
