using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public static int spawnRate = 5;
    float lastSpawned;

    void Update(){
        lastSpawned += Time.deltaTime;
        if (lastSpawned >= spawnRate)
        {
            lastSpawned = 0;
            Spawn_Enemy();
        }
    }

    void Spawn_Enemy(){
        Instantiate(enemyPrefab, this.gameObject.transform.position, Quaternion.identity);
    }
}
