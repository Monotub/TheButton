using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Enemy enemy;
    [SerializeField] Button button;

    [Header("Spawn Customization")]
    [SerializeField] float minSpawnDistance = 10;

    [Header("Casual Difficulty")]
    [SerializeField] float casualSpawnDelay = 3f;
    [SerializeField] float casualEnemyMoveSpeed = 2;

    [Header("Normal Difficulty")]
    [SerializeField] float normalSpawnDelay = 3f;
    [SerializeField] float normalEnemyMoveSpeed = 5;

    [Header("Hardcore Difficulty")]
    [SerializeField] float hardSpawnDelay = 3f;
    [SerializeField] float hardEnemyMoveSpeed = 5;

    Vector3 spawnLocation;
    float spawnY = 0.3f;
    float spawnDelay;
    float enemyMoveSpeed;

    public bool canSpawnEnemies = false;


    private void Update()
    {
        int difficulty = GameManager.Instance.difficulty;

        switch (difficulty)
        {
            case 0:
                spawnDelay = casualSpawnDelay;
                enemyMoveSpeed = casualEnemyMoveSpeed;
                break;
            case 1:
                spawnDelay = normalSpawnDelay;
                enemyMoveSpeed = normalEnemyMoveSpeed;
                break;
            case 2:
                spawnDelay = hardSpawnDelay;
                enemyMoveSpeed = hardEnemyMoveSpeed;
                break;
        }
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
        newEnemy.SetSpeed(enemyMoveSpeed);

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
