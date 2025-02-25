using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerLivesView
    {
        private List<SpriteRenderer> livesIcons;

        public PlayerLivesView(List<SpriteRenderer> livesIcons)
        {
            this.livesIcons = livesIcons;
        }

        public void UpdateLivesUI(int livesLeft)
        {
            for(int i = 0; i < livesIcons.Count; i++)
            {
                if(livesLeft > 0)
                {
                    livesIcons[i].enabled = true;
                    livesLeft--;
                }
                else
                {
                    livesIcons[i].enabled = false;
                }
            }
        }
    }
}