using Enemies;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        private PlayerLivesController controller;

        public void Construct(PlayerLivesController controller)
        {
            this.controller = controller;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out EnemyHandler enemyHandler))
            {
                controller.InvokeLivesUpdate(1);
            }
        }
    }
}
