using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] powerups;
    [SerializeField] float spawnThreshold;
    float spawnChance;

    private void OnEnable()
    {
        Enemy.OnShieldDeath += TrySpawnPowerup;
    }

    private void OnDisable()
    {
        Enemy.OnShieldDeath -= TrySpawnPowerup;
    }


    public void TrySpawnPowerup(GameObject enemy)
    {
        spawnChance = Random.Range(0f, 1f);

        if(spawnChance < spawnThreshold && enemy == gameObject)
        {
            int powerupType;
            float powerupChance = Random.Range(0f, 2f);
            if (powerupChance > 1.5f)
                powerupType = 1;
            else
                powerupType = 0;
            Instantiate(powerups[powerupType], transform.position, Quaternion.identity);
        }
    }
}
