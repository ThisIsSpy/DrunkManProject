using UnityEngine;

namespace Score.Powerups
{
    [CreateAssetMenu(fileName = "PowerUpSO", menuName = "SO/PowerUp SO", order = 1)]
    public class PowerUpSO : ScriptableObject
    {
        [field: SerializeField] public int ScoreGivenOnPickUp { get; private set; }
        [field: SerializeField] public Sprite PowerUpSprite { get; private set; }
        [field: SerializeField] public AudioClip PickUpSFX { get; private set; }
    }
}