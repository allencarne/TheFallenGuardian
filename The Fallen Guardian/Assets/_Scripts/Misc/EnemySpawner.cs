using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Vector2 size;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int maxEnemyCount;
    [SerializeField] int currentEnemyCount;

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

        yield return new WaitForSeconds(2);

        canSpawn = true;
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0f, 0f);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        enemy.GetComponent<Enemy>().EnemySpawner = this;

        currentEnemyCount++;
    }

    public void DecreaseEnemyCount()
    {
        currentEnemyCount--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
