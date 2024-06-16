using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class EnemyDefinition {
    public GameObject prefab;
    public int minimumChallengeLevel = 1;
}

public class BattleMaster : MonoBehaviour
{
    public EnemyDefinition[] enemyPrefabs;

    public List<EnemyDefinition> spawnPool {
        get {
            List<EnemyDefinition> temp = new List<EnemyDefinition>();

            for (int i = 0; i < enemyPrefabs.Length; i++) {
                if (enemyPrefabs[i].minimumChallengeLevel <= Global._nextChallengeLevel) {
                    temp.Add(enemyPrefabs[i]);
                }
            }

            return temp;
        }
    }

    public EnemyDefinition randomDefinition {
        get {
            List<EnemyDefinition> enemies = spawnPool;

            return enemies[UnityEngine.Random.Range(0, enemies.Count)];
        }
    }

    public Transform spawnHook;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int targetSpawn = Global._nextChallengeLevel;

        if (Global.temp_willChallenge >= 1) {
            targetSpawn += 1;
        }

        Setup(targetSpawn);
    }

    Vector3 spawnLocation
    {
        get
        {
            return new Vector3(
                spawnHook.transform.position.x + UnityEngine.Random.Range(-10.0f, 10.0f),
                0.0f,
                spawnHook.transform.position.z + UnityEngine.Random.Range(-10.0f, 10.0f)
            );
        }
    }

    public bool running = true;

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            int allyCount = 0;
            int enemyCount = 0;

            Alliance[] allAlliance = GameObject.FindObjectsByType<Alliance>(FindObjectsSortMode.None);

            for (int i = 0; i < allAlliance.Length; i++)
            {
                if (allAlliance[i].alliance == 0)
                {
                    enemyCount += 1;
                }
                else
                {
                    allyCount += 1;
                }
            }

            if (allyCount <= 0)
            {
                Global.Log("Lost combat, sorry :(");
                running = false;
                SceneManager.LoadScene("Game");
            }
            if (enemyCount <= 0)
            {
                int calculatedReward = 30 * Global._nextChallengeLevel;
                Debug.Log("won");
                Global.Log("Won combat, recieve " + calculatedReward.ToString() + " gold");
                Global._gold += calculatedReward;
                if (Global.temp_willChallenge >= 1) {
                    Global._nextChallengeLevel += 1;
                }
                Global.Save();
                running = false;
                SceneManager.LoadScene("Game");
            }
        }
    }

    void Setup(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            EnemyDefinition enemy = randomDefinition;

            GameObject g = GameObject.Instantiate(enemy.prefab);
            g.transform.position = spawnLocation;
        }
    }
}
