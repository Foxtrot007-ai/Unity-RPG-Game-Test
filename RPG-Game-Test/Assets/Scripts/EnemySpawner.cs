using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject[] Enemies = new GameObject[2];

    public int LevelOfEnemy;
    public GameObject EnemyPrefab;
    public GameObject SpawnPoint0;
    public GameObject SpawnPoint1;
    public float respawnTime = 30f;
    public float sightRange = 50f;
    public bool respawning = false;
    private void Awake()
    {
        RespawnEnemies();
    }

    private void Update()
    {
        if (Enemies[0] == null && Enemies[1] == null && !respawning)
        {
            respawning = true;
            Invoke("RespawnEnemies", respawnTime);
        }
    }
    private GameObject CreateEnemy(GameObject SpawnPoint, int number)
    {
        GameObject TempEnemy = Instantiate(EnemyPrefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        EnemyAI TempAI = TempEnemy.GetComponent<EnemyAI>();
        TempAI.enemySpawner = this;
        TempAI.level = LevelOfEnemy;
        TempAI.spawnPoint = SpawnPoint;
        TempAI.sightRange = sightRange;
        TempAI.attackRange = 5f;
        TempAI.timeBetweenAttacks = 5f;
        TempAI.myNumber = number;
        return TempEnemy;
    }

    private void RespawnEnemies()
    {
        Enemies[0] = CreateEnemy(SpawnPoint0, 0);
        Enemies[1] = CreateEnemy(SpawnPoint1, 1);
        respawning = false;
    }

    public void ResetMe(int number)
    {
        Enemies[number] = null;
    }
}
