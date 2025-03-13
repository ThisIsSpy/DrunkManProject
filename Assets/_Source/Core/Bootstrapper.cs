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
        [SerializeField] private PowerUpSpawner powerUpSpawner;
        [SerializeField] private PowerUpCollectionUI powerUpCollectionUI;

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
            int spawnPointsCount = 0;
            foreach (PowerUp powerUp in powerUps)
            {
                if(powerUp.IsSpawnPoint) spawnPointsCount++;
            }
            scoreModel = new(powerUps.Length - spawnPointsCount, ghostEatingScore);
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
            List<PowerUp> powerUpSpawnPoints = new();
            foreach (var powerup in powerUps)
            {
                powerup.Construct(sfxSource, scoreController, gameManager, powerUpCollectionUI);
                if (powerup.IsPowerPellet) powerup.Spawn(powerUpSOList[0]);
                else if (powerup.IsSpawnPoint) powerUpSpawnPoints.Add(powerup);
                else powerup.Spawn(powerUpSOList[1]);
            }
            powerUpSpawner.Construct(powerUpSpawnPoints, powerUpSOList);
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