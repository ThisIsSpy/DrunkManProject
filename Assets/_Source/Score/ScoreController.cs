using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Score
{
    public class ScoreController
    {
        private readonly ScoreModel model;
        private readonly ScoreView view;

        public ScoreController(ScoreModel model, ScoreView view)
        {
            this.model = model;
            this.view = view;
            Subscribe();
        }

        private void Subscribe()
        {
            model.AllPowerUpsCollected += OnAllPowerUpsCollected;
        }

        public void InvokeScoreUpdate(int score, bool updatePowerUpsCount)
        {
            model.Score += score;
            if(updatePowerUpsCount)
            {
                model.PowerUpsLeft--;
                model.PowerUpsEatenInThisLife++;
            }
            view.UpdateScoreUI(model.Score);
        }

        public void InvokeScoreUpdateFromEatingGhosts()
        {
            InvokeScoreUpdate(model.GhostEatingScore*model.GhostEatingMult, false);
            model.GhostEatingMult++;
        }

        public void InvokeResetPowerUpsEatenInThisLife()
        {
            model.PowerUpsEatenInThisLife = 0;
        }

        public void OnAllPowerUpsCollected()
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}