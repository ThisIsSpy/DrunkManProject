using Movement;
using Player;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{

    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private InputListener inputListener;
        [SerializeField] private List<MovementSetup> movingEntities;

        private void Start()
        {
            foreach (var entity in movingEntities)
            {
                entity.Setup();
                if (entity.IsPlayer) inputListener.Construct(entity.GetController());
            }
        }
    }
    
}
