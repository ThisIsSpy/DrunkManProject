using System;
using UnityEngine.SceneManagement;

namespace Score
{
    public class ScoreController : IDisposable
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

        public void Dispose()
        {
            model.AllPowerUpsCollected -= OnAllPowerUpsCollected;
        }

        public void InvokeScoreUpdate(int score)
        {
            model.Score += score;
            model.PowerUpsLeft--;
            view.UpdateScoreUI(model.Score);
        }

        public void OnAllPowerUpsCollected()
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}