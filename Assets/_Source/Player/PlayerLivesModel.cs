using System;
using UnityEngine;

namespace Player
{
    public class PlayerLivesModel
    {
        private int lives;
        public bool HadDiedOnThisLevel;

        public int Lives { get { return lives; } 
            set 
            {
                if (lives == value) return;
                lives = Mathf.Clamp(value, 0, 3);
                if(lives == 0) LivesReachedZero?.Invoke();
            } 
        }

        public PlayerLivesModel(int lives)
        {
            Lives = lives;
            HadDiedOnThisLevel = false;
        }

        public event Action LivesReachedZero;
    }
}