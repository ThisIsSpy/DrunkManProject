using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player{
    
    public class PlayerFollower : MonoBehaviour
    {
        [SerializeField] private GameObject player;

        private void LateUpdate()
        {
            transform.position = new(player.transform.position.x, player.transform.position.y+2, transform.position.z);
        }
    }
    
}
