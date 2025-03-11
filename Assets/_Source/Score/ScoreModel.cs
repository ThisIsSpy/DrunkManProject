using Enemies;
using System;
using UnityEngine;

namespace Score
{
    public class ScoreModel
    {
        private int score;
        private readonly int maxPowerUps;
        private int powerUpsLeft;
        private int powerUpsEatenInThisLife;
        private int ghostEatingScore;
        private int ghostEatingMult;

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
                PowerUpCollected?.Invoke();
                if (powerUpsLeft == 0) AllPowerUpsCollected?.Invoke();
            }
        }

        public int PowerUpsEatenInThisLife { get { return powerUpsEatenInThisLife; } 
            set 
            {
                if(powerUpsEatenInThisLife == value) return;
                powerUpsEatenInThisLife = Mathf.Clamp(value, 0, maxPowerUps);
            } 
        }

        public int GhostEatingScore { get { return ghostEatingScore; }
            private set 
            {
                ghostEatingScore = Mathf.Clamp(value, 0, 999);
            }
        }

        public int GhostEatingMult { get { return ghostEatingMult; } 
            set 
            {
                ghostEatingMult = Mathf.Clamp(value, 1, 4);
            } 
        }

        public ScoreModel(int maxPowerUps, int ghostEatingScore)
        {
            Score = 0;
            this.maxPowerUps = maxPowerUps;
            PowerUpsLeft = maxPowerUps;
            GhostEatingScore = ghostEatingScore;
            GhostEatingMult = 1;
        }

        public event Action AllPowerUpsCollected;
        public event Action PowerUpCollected;
    }
}