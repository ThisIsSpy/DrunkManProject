using System;
using UnityEngine;

namespace Score
{
    public class ScoreModel
    {
        private int score;
        private readonly int maxPowerUps;
        private int powerUpsLeft;

        public int Score { get { return score; }
            set 
            { 
                if(score == value) return;
                score = Mathf.Clamp(value, 0, 999999);
            }
        }

        public int PowerUpsLeft { get { return powerUpsLeft; }
            set 
            { 
                if (powerUpsLeft == value) return;
                powerUpsLeft = Mathf.Clamp(value, 0, maxPowerUps);
                if (powerUpsLeft == 0) AllPowerUpsCollected?.Invoke();
            }
        }

        public ScoreModel(int maxPowerUps)
        {
            Score = 0;
            this.maxPowerUps = maxPowerUps;
            PowerUpsLeft = maxPowerUps;
        }

        public event Action AllPowerUpsCollected;
    }
}