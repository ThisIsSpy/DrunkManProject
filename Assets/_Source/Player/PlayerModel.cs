using UnityEngine;

namespace Player
{
    
    public class PlayerModel
    {
        private int movementSpeed;

        public int MovementSpeed { get { return movementSpeed; }
            private set 
            {
                if(movementSpeed == value) return;
                movementSpeed = Mathf.Clamp(value, 0, 10);
            } 
        }

        public PlayerModel(int movementSpeed)
        {
            MovementSpeed = movementSpeed;
        }
    }
}
