using UnityEngine;

namespace Score
{
    public class ScoreModel
    {
        private int score;

        public int Score { get { return score; }
            set 
            { 
                score = Mathf.Clamp(value, 0, 999999);
            }
        }

        public ScoreModel()
        {
            Score = 0;
        }
    }
}