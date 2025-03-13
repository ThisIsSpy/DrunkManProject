using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Score.Powerups
{
    public class PowerUpSpawner : MonoBehaviour
    {
        [SerializeField] private float minWaitTime;
        [SerializeField] private float maxWaitTime;
        private List<PowerUp> powerUpSpawnPoints;
        private List<PowerUpSO> powerUpList;

        public void Construct(List<PowerUp> powerUpSpawnPoints, List<PowerUpSO> powerUpList)
        {
            this.powerUpSpawnPoints = powerUpSpawnPoints;
            this.powerUpList = powerUpList;
            StartCoroutine(SpawnPowerUp());
        }

        public IEnumerator SpawnPowerUp()
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            PowerUp randomSpawnPoint = powerUpSpawnPoints[Random.Range(0,powerUpSpawnPoints.Count-1)];
            if(!randomSpawnPoint.IsSpawned) randomSpawnPoint.Spawn(powerUpList[Random.Range(2, powerUpList.Count-1)]);
            StartCoroutine(SpawnPowerUp());
        }
    }
}