using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Enemy enemy;
    [SerializeField] Button button;

    [Header("Spawn Customization")]
    [SerializeField] float spawnDelay = 3f;
    [SerializeField] float minSpawnDistance = 10;

    [Header("Enemy Customization")]
    [SerializeField] float EnemyMoveSpeed = 5;

    Vector3 spawnLocation;
    float spawnY = 0.3f;
    public bool canSpawnEnemies = false;
    

    private void Start()
    {
        
        //StartCoroutine(SpawnEnemiesOnTimer());
    }

    IEnumerator SpawnEnemiesOnTimer()
    {
        while (canSpawnEnemies)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemies()
    {
        spawnLocation = new Vector3(Random.Range(-20, 20), spawnY, Random.Range(-20, 20));

        var newEnemy = Instantiate(enemy, spawnLocation, Quaternion.identity);
        newEnemy.transform.LookAt(button.transform);
        newEnemy.SetSpeed(EnemyMoveSpeed);

        if (Vector3.Distance(newEnemy.transform.position, button.transform.position) < minSpawnDistance)
        {
            Destroy(newEnemy.gameObject);
            SpawnEnemies();
        }
    }

    [ContextMenu("Toggle Enemy Spawn")]
    public void ToggleEnemySpawn()
    {
        canSpawnEnemies = true;
        StartCoroutine(SpawnEnemiesOnTimer());
    }
}
