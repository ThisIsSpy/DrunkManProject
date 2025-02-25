using Movement;
using UnityEngine;

namespace Score.Powerups
{
    public class PowerUp : MonoBehaviour
    {
        private int scoreGivenOnPickUp;
        private Sprite powerUpSprite;
        private AudioClip pickUpSFX;
        private SpriteRenderer spriteRenderer;
        private AudioSource sfxPlayer;
        private ScoreController scoreController;
        private bool isSetup = false;
        private bool isPickedUp = false;

        public void Setup(PowerUpSO powerUpSO, AudioSource sfxPlayer, ScoreController scoreController)
        {
            scoreGivenOnPickUp = powerUpSO.ScoreGivenOnPickUp;
            powerUpSprite = powerUpSO.PowerUpSprite;
            pickUpSFX = powerUpSO.PickUpSFX;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = powerUpSprite;
            this.sfxPlayer = sfxPlayer;
            this.scoreController = scoreController;
            isSetup = true;
            isPickedUp = false;
            gameObject.transform.rotation = Quaternion.identity;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isSetup || isPickedUp) return;
            if (collision.gameObject.TryGetComponent(out MovementSetup setup) && setup.IsPlayer)
            {
                scoreController.InvokeScoreUpdate(scoreGivenOnPickUp);
                sfxPlayer.PlayOneShot(pickUpSFX);
                spriteRenderer.enabled = false;
                isPickedUp = true;
            }
        }
    }
}
