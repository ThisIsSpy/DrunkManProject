using UnityEngine;

namespace Movement
{
    
    public class MovementModel
    {
        private float currentSpeed;
        private float afraidSpeed;
        private float normalSpeed;

        public float CurrentSpeed { get { return currentSpeed; } 
            set 
            {
                currentSpeed = Mathf.Clamp(value, 0, 10);
            } 
        }
        public float AfraidSpeed { get { return afraidSpeed; } private set { afraidSpeed = Mathf.Clamp(value, 0, NormalSpeed); } }
        public float NormalSpeed { get { return normalSpeed; } private set { normalSpeed = Mathf.Clamp(value, AfraidSpeed, 10); } }

        public MovementModel(float normalSpeed)
        {
            NormalSpeed = normalSpeed;
            AfraidSpeed = normalSpeed / 2;
            CurrentSpeed = normalSpeed;
        }
    }
    
}
