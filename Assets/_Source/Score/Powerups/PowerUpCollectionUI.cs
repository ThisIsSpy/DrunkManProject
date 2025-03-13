using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Score.Powerups
{
    public class PowerUpCollectionUI : MonoBehaviour
    {
        [SerializeField] private GameObject powerUpCollectionUI;
        [SerializeField] private GameObject iconPrefab;

        public void AddPowerUpToCollection(Sprite powerUpSprite)
        {
            GameObject instantiatedPowerUpIcon = Instantiate(iconPrefab, powerUpCollectionUI.transform.position, Quaternion.identity, powerUpCollectionUI.transform);
            instantiatedPowerUpIcon.transform.localScale *= 0.15f;
            instantiatedPowerUpIcon.GetComponent<SpriteRenderer>().sprite = powerUpSprite;
        }
    }
}