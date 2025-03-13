using Core;
using Movement;
using System;
using UnityEngine;

namespace Score.Powerups
{
    public class PowerUp : MonoBehaviour
    {
        [field: SerializeField] public bool IsPowerPellet { get; private set; } = false;
        [field: SerializeField] public bool IsSpawnPoint { get; private set; } = false;
        [field: SerializeField] public bool IsSpawned { get; private set; } = false;
        private GameManager gameManager;
        private PowerUpCollectionUI powerUpCollectionUI;
        private int scoreGivenOnPickUp;
        private Sprite powerUpSprite;
        private AudioClip pickUpSFX;
        private SpriteRenderer spriteRenderer;
        private AudioSource sfxPlayer;
        private ScoreController scoreController;
        private bool isPickedUp = false;

        public void Construct(AudioSource sfxPlayer, ScoreController scoreController, GameManager gameManager, PowerUpCollectionUI powerUpCollectionUI)
        {
            this.sfxPlayer = sfxPlayer;
            this.scoreController = scoreController;
            this.gameManager = gameManager;
            this.powerUpCollectionUI = powerUpCollectionUI;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }

        public void Spawn(PowerUpSO powerUpSO)
        {
            scoreGivenOnPickUp = powerUpSO.ScoreGivenOnPickUp;
            powerUpSprite = powerUpSO.PowerUpSprite;
            pickUpSFX = powerUpSO.PickUpSFX;
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = powerUpSprite;
            IsSpawned = true;
            isPickedUp = false;
            gameObject.transform.rotation = Quaternion.identity;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsSpawned || isPickedUp) return;
            if (collision.gameObject.TryGetComponent(out MovementSetup setup) && setup.IsPlayer)
            {
                if (IsPowerPellet) gameManager.ActivateAfraidState();
                if (IsSpawnPoint) 
                {
                    powerUpCollectionUI.AddPowerUpToCollection(spriteRenderer.sprite);
                    SpawnedPowerUpPickedUp?.Invoke();
                }
                scoreController.InvokeScoreUpdate(scoreGivenOnPickUp, true);
                sfxPlayer.PlayOneShot(pickUpSFX);
                spriteRenderer.enabled = false;
                isPickedUp = true;
            }
        }

        public event Action SpawnedPowerUpPickedUp;
    }
}
