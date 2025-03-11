using Core;
using Movement;
using UnityEngine;

namespace Score.Powerups
{
    public class PowerUp : MonoBehaviour
    {
        [field: SerializeField] public bool IsPowerPellet { get; private set; } = false;
        private GameManager gameManager;
        private int scoreGivenOnPickUp;
        private Sprite powerUpSprite;
        private AudioClip pickUpSFX;
        private SpriteRenderer spriteRenderer;
        private AudioSource sfxPlayer;
        private ScoreController scoreController;
        private bool isSetup = false;
        private bool isPickedUp = false;

        public void Setup(PowerUpSO powerUpSO, AudioSource sfxPlayer, ScoreController scoreController, GameManager gameManager)
        {
            scoreGivenOnPickUp = powerUpSO.ScoreGivenOnPickUp;
            powerUpSprite = powerUpSO.PowerUpSprite;
            pickUpSFX = powerUpSO.PickUpSFX;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = powerUpSprite;
            this.sfxPlayer = sfxPlayer;
            this.scoreController = scoreController;
            this.gameManager = gameManager;
            isSetup = true;
            isPickedUp = false;
            gameObject.transform.rotation = Quaternion.identity;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isSetup || isPickedUp) return;
            if (collision.gameObject.TryGetComponent(out MovementSetup setup) && setup.IsPlayer)
            {
                if (IsPowerPellet) gameManager.ActivateAfraidState();
                scoreController.InvokeScoreUpdate(scoreGivenOnPickUp, true);
                sfxPlayer.PlayOneShot(pickUpSFX);
                spriteRenderer.enabled = false;
                isPickedUp = true;
            }
        }
    }
}
