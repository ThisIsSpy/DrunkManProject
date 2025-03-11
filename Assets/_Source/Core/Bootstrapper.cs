using Enemies;
using Movement;
using Player;
using Score;
using Score.Powerups;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core
{
    public class Bootstrapper : MonoBehaviour
    {
        [Header("Game Manager stuff")]
        [SerializeField] private GameManager gameManager;

        [Header("Player stuff")]
        [SerializeField] private GameObject livesHUD;
        [SerializeField] private GameObject lifeIconPrefab;
        [SerializeField] private PlayerCollisionDetector collisionDetector;
        [SerializeField, Range(1, 3)] private int playerLives;

        [Header("Score stuff")]
        [SerializeField] private TextMeshProUGUI scoreField;
        [SerializeField] private int ghostEatingScore;

        [Header("Moving stuff")]
        [SerializeField] private InputListener inputListener;
        [SerializeField] private List<MovementSetup> movingEntities;

        [Header("Power Up stuff")]
        [SerializeField] private List<PowerUpSO> powerUpSOList;

        [Header("Audio stuff")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private ScoreModel scoreModel;
        private ScoreView scoreView;
        private ScoreController scoreController;

        private PlayerLivesModel playerLivesModel;
        private PlayerLivesView playerLivesView;
        private PlayerLivesController playerLivesController;
        private List<SpriteRenderer> livesIconList;

        private void Start()
        {
            ScoreSetup();
            PlayerLivesSetup();
            gameManager.Construct(scoreModel, playerLivesModel, musicSource);
            MovingStuffSetup();
            PowerUpSetup();
            collisionDetector.Construct(playerLivesController, scoreController);
        }

        private void ScoreSetup()
        {
            PowerUp[] powerUps = FindObjectsOfType<PowerUp>(true);
            scoreModel = new(powerUps.Length, ghostEatingScore);
            scoreView = new(scoreField);
            scoreController = new(scoreModel, scoreView);
        }

        private void MovingStuffSetup()
        {
            foreach (var entity in movingEntities)
            {
                entity.Setup();
                if (entity.IsPlayer) inputListener.Construct(entity.GetController());
                if (entity.TryGetComponent(out EnemyHandler enemy)) gameManager.AddGhost(enemy);
            }
        }

        private void PowerUpSetup()
        {
            PowerUp[] powerUps = FindObjectsOfType<PowerUp>(true);
            foreach (var powerup in powerUps)
            {
                if (powerup.IsPowerPellet) powerup.Setup(powerUpSOList[0], sfxSource, scoreController, gameManager);
                else powerup.Setup(powerUpSOList[1], sfxSource, scoreController, gameManager);
            }
        }

        private void PlayerLivesSetup()
        {
            livesIconList = new();
            for (int i = 0; i < playerLives; i++)
            {
                GameObject instantiatedPlayerLifeIcon = Instantiate(lifeIconPrefab, livesHUD.transform.position, Quaternion.identity, livesHUD.transform);
                SpriteRenderer iconSprite = instantiatedPlayerLifeIcon.GetComponent<SpriteRenderer>();
                livesIconList.Add(iconSprite);
            }

            playerLivesModel = new(playerLives);
            playerLivesView = new(livesIconList);
            playerLivesController = new(playerLivesModel, playerLivesView);

            inputListener.Construct(playerLivesController);
        }
    }
}