using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;

    [Header("Box Size")]
    [SerializeField] Vector2 size;

    [Header("Enemy Count")]
    [SerializeField] int maxEnemyCount;

    [Header("Color")]
    [SerializeField] Color color;

    int currentEnemyCount;
    float spawnDelay = 6;
    bool canSpawn = true;

    private void Update()
    {
        if (currentEnemyCount < maxEnemyCount)
        {
            if (canSpawn)
            {
                StartCoroutine(Delay());
            }
        }
    }

    IEnumerator Delay()
    {
        canSpawn = false;

        yield return new WaitForSeconds(spawnDelay);

        canSpawn = true;
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0f, 0f);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);

        enemy.GetComponent<Enemy>().EnemySpawner = this;

        currentEnemyCount++;
    }

    public void DecreaseEnemyCount()
    {
        currentEnemyCount--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
