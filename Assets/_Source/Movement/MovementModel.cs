using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement{
    
    public class MovementModel
    {
        private int speed;

        public int Speed { get { return speed; } 
            private set 
            {
                speed = Mathf.Clamp(value, 0, 10);
            } 
        }

        public MovementModel(int speed)
        {
            Speed = speed;
        }
    }
    
}
