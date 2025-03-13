using Core;
using Enemies;
using Movement;
using Score;
using System;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        private PlayerLivesController playerLivesController;
        private ScoreController scoreController;
        public bool CanEat = false;

        public void Construct(PlayerLivesController playerLivesController, ScoreController scoreController)
        {
            this.playerLivesController = playerLivesController;
            this.scoreController = scoreController;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out EnemyHandler enemyHandler))
            {
                if(!CanEat)
                {
                    playerLivesController.InvokeLivesUpdate(1);
                    scoreController.InvokeResetPowerUpsEatenInThisLife();
                    PlayerDied?.Invoke();
                }
                else
                {
                    Debug.Log("ghost eaten");
                    scoreController.InvokeScoreUpdateFromEatingGhosts();
                    Color normalGhostColor = enemyHandler.GetComponent<SpriteRenderer>().color;
                    enemyHandler.GetComponent<SpriteRenderer>().color = new(normalGhostColor.r, normalGhostColor.g, normalGhostColor.b, 0.5f);
                    enemyHandler.IsRespawning = true;
                }
            }
        }

        public event Action PlayerDied;
    }
}
