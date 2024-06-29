using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawnPosition;
    
    [SerializeField]
    private List<Wave> waves;

    private bool waveComplete = false;
    private List<GameObject> enemiesAlive = new();

    private void Start()
    {
        StartCoroutine(StartWaves());
    }

    public IEnumerator StartWaves()
    {
        int count = waves.Count;
        for (int i = 0; i < count; i++)
        {
            print("Starting wave: " + (i + 1) + " | index(" + i + ")");
            StartCoroutine(SpawnEnemies(waves[i].enemies));

            while(!waveComplete)
                yield return null;

            print("Wave: " + i + " : complete!");
            waveComplete = false;

            if(i < count - 1)
            {
                print("Waiting: " + waves[i].waitWaveTime + " seconds");
                yield return new WaitForSeconds(waves[i].waitWaveTime);
            }
        }

        print("DONE!");
    }

    private IEnumerator SpawnEnemies(List<WaveEnemy> enemiesToSpawn)
    {
        int count = enemiesToSpawn.Count;
        for (int i = 0; i < count; i++)
        {
            print("Type: " + (i + 1) + " of " + count + " | amount: " + enemiesToSpawn[i].amount + " | name: " + enemiesToSpawn[i].enemy.name);
            for (int j = 0; j < enemiesToSpawn[i].amount; j++)
            {
                SpawnEnemy(enemiesToSpawn[i].enemy);
                yield return new WaitForSeconds(enemiesToSpawn[i].waitTime);
            }
        }
    }

    private void SpawnEnemy(GameObject enemyToSpawn)
    {
        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, transform);
        enemiesAlive.Add(spawnedEnemy);
        Simple_Enemy enemy = spawnedEnemy.GetComponent<Simple_Enemy>();
        enemy.OnDeath += () => RemoveDeadEnemy(spawnedEnemy);
    }

    private void RemoveDeadEnemy(GameObject enemyToRemove){
        enemiesAlive.Remove(enemyToRemove);
        
        if(enemiesAlive.Count == 0)
            waveComplete = true;
    }
}
