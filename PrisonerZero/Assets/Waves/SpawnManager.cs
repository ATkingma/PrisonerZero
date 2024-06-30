using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawnMaxPosition;
    [SerializeField]
    private Vector2 spawnMinPosition;

    [SerializeField]
    private List<Wave> waves;

    private bool waveComplete = false;
    private bool waveInProgress = false;
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

            while(!waveComplete || waveInProgress)
                yield return null;

            print("Wave: " + (i + 1) + " | index(" + i + ")" + " complete!");
            waveComplete = false;

            if(i < count - 1 && waves[i].waitWaveTime > 0) // Skips wait if not wait time.
            {
                print("Waiting: " + waves[i].waitWaveTime + " seconds");
                yield return new WaitForSeconds(waves[i].waitWaveTime);
            }
        }

        while (!waveComplete || waveInProgress) // Wait till everything is killed.
            yield return null;

        print("DONE!");
    }

    private IEnumerator SpawnEnemies(List<WaveEnemy> enemiesToSpawn)
    {
        waveInProgress = true;
        int count = enemiesToSpawn.Count;
        for (int i = 0; i < count; i++)
        {
            print("Type: " + (i + 1) + " of " + count + " | amount: " + enemiesToSpawn[i].amount + " | name: " + enemiesToSpawn[i].enemy.name);
            for (int j = 0; j < enemiesToSpawn[i].amount; j++)
            {
                SpawnEnemy(enemiesToSpawn[i].enemy);

                if(enemiesToSpawn[i].waitTime > 0) // Skips wait if not wait time.
                    yield return new WaitForSeconds(enemiesToSpawn[i].waitTime);
            }

            if (i < count - 1 && enemiesToSpawn[i].waitTimeNextEnemy > 0) // Skips wait if not wait time.
            {
                print("Waiting: " + enemiesToSpawn[i].waitTimeNextEnemy + " seconds for next batch.");
                yield return new WaitForSeconds(enemiesToSpawn[i].waitTimeNextEnemy);
            }
        }
        waveInProgress = false;
    }

    private void SpawnEnemy(GameObject enemyToSpawn)
    {
        Vector2 spawnPosition = new(Random.Range(spawnMinPosition.x, spawnMaxPosition.x), Random.Range(spawnMinPosition.y, spawnMaxPosition.y));
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
