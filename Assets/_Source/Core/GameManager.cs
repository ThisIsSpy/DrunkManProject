using Enemies;
using Movement;
using Player;
using Score;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyHandler> ghostList;
        [field: SerializeField] public GameObject Player { get; private set; }
        [field: SerializeField] public GhostMode GhostMode { get; private set; }
        [field: SerializeField] public bool GameIsRunning { get; private set; }
        [SerializeField] private int scatterModeDuration = 7;
        [SerializeField] private int chaseModeDuration = 20;
        [SerializeField] private float afraidStateDuration;
        [SerializeField] private AudioClip startMusic;
        [SerializeField] private AudioClip normalMusic;
        [SerializeField] private AudioClip powerupMusic;
        private AudioSource musicPlayer;
        private ScoreModel scoreModel;
        private PlayerLivesModel playerLivesModel;
        private int modeSwitchCycle;

        private void Awake()
        {
            GhostMode = GhostMode.Scatter;
            modeSwitchCycle = 0;
            GameIsRunning = false;
        }

        public void Construct(ScoreModel scoreModel, PlayerLivesModel playerLivesModel, AudioSource musicPlayer)
        {
            this.scoreModel = scoreModel;
            this.playerLivesModel = playerLivesModel;
            this.musicPlayer = musicPlayer;
            Subscribe();
            StartCoroutine(GameStartCoroutine());
        }

        private void Subscribe()
        {
            scoreModel.PowerUpCollected += CheckIfGhostShouldAwake;
            Player.GetComponent<PlayerCollisionDetector>().PlayerDied += ReturnEveryoneToStartingPosition;
        }

        public void OnDestroy()
        {
            scoreModel.PowerUpCollected -= CheckIfGhostShouldAwake;
            Player.GetComponent<PlayerCollisionDetector>().PlayerDied -= ReturnEveryoneToStartingPosition;
        }

        public void AddGhost(EnemyHandler ghost)
        {
            ghostList.Add(ghost.GetComponent<EnemyHandler>());
        }

        private void CheckIfGhostShouldAwake()
        {
            if (!GameIsRunning) return;

            int pelletsForBlueAwakening;
            int pelletsForOrangeAwakening;
            if (playerLivesModel.HadDiedOnThisLevel)
            {
                pelletsForBlueAwakening = 12;
                pelletsForOrangeAwakening = 32;
            }
            else
            {
                pelletsForBlueAwakening = 30;
                pelletsForOrangeAwakening = 60;
            }

            if (scoreModel.PowerUpsEatenInThisLife >= pelletsForBlueAwakening
                && !GetGhost(GhostType.Blue).GetComponent<EnemyHandler>().LeftHomeBefore)
            {   GetGhost(GhostType.Blue).GetComponent<EnemyHandler>().ReadyToLeaveHome = true; }
            if (scoreModel.PowerUpsEatenInThisLife >= pelletsForOrangeAwakening
                && !GetGhost(GhostType.Orange).GetComponent<EnemyHandler>().LeftHomeBefore)
            {  GetGhost(GhostType.Orange).GetComponent<EnemyHandler>().ReadyToLeaveHome = true; }
        }

        public void ActivateAfraidState()
        {
            StartCoroutine(AfraidStateCoroutine());
        }

        public GameObject GetGhost(GhostType ghostType)
        {
            foreach(EnemyHandler handler in ghostList)
            {
                if (handler.GhostType == ghostType) return handler.gameObject;
            }
            return null;
        }

        private IEnumerator GameStartCoroutine()
        {
            musicPlayer.PlayOneShot(startMusic);
            yield return new WaitForSeconds(startMusic.length);
            musicPlayer.clip = normalMusic;
            musicPlayer.loop = true;
            musicPlayer.Play();
            GameIsRunning = true;
            StartCoroutine(GhostModeSwitcherCoroutine());
        }

        private IEnumerator AfraidStateCoroutine()
        {
            StopCoroutine(GhostModeSwitcherCoroutine());
            ChangeMusic(powerupMusic);
            foreach(EnemyHandler handler in ghostList)
            {
                if (handler.ReadyToLeaveHome && handler.GhostNodeState == GhostNodeState.MovingInNodes) 
                {
                    handler.IsAfraid = true;
                    handler.GetComponent<MovementView>().ChangeMovementSpeed(handler.GetComponent<MovementView>().Model.AfraidSpeed);
                }
            }
            Player.GetComponent<PlayerCollisionDetector>().CanEat = true;
            yield return new WaitForSeconds(afraidStateDuration);
            foreach(EnemyHandler handler in ghostList)
            {
                if (handler.IsAfraid)
                {
                    handler.IsAfraid = false;
                    handler.GetComponent<MovementView>().ChangeMovementSpeed(handler.GetComponent<MovementView>().Model.NormalSpeed);
                }
            }
            Player.GetComponent<PlayerCollisionDetector>().CanEat = false;
            scoreModel.GhostEatingMult = 1;
            ChangeMusic(normalMusic);
            StartCoroutine(GhostModeSwitcherCoroutine());
        }

        private IEnumerator GhostModeSwitcherCoroutine()
        {
            GhostMode = GhostMode.Scatter;
            yield return new WaitForSeconds(scatterModeDuration);
            GhostMode = GhostMode.Chase;
            modeSwitchCycle++;
            if(modeSwitchCycle < 4)
            {
                yield return new WaitForSeconds(chaseModeDuration);
                if (modeSwitchCycle > 2) scatterModeDuration -= 2;
                StartCoroutine(GhostModeSwitcherCoroutine());
            }
        }

        public void ReturnEveryoneToStartingPosition()
        {
            GameIsRunning = false;
            Player.GetComponent<MovementSetup>().ReturnToStartingNode();
            foreach(EnemyHandler handler in ghostList)
            {
                handler.GetComponent<MovementSetup>().ReturnToStartingNode();
            }
            musicPlayer.Stop();
            StartCoroutine(GameStartCoroutine());
        }

        private void ChangeMusic(AudioClip music)
        {
            musicPlayer.Stop();
            musicPlayer.clip = music;
            musicPlayer.Play();
        }
    }

    public enum GhostMode
    {
        Chase,
        Scatter
    }
}