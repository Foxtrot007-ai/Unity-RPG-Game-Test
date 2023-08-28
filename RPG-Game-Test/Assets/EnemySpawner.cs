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
            Invoke("Respawn", 30f);
        }
    }
    private GameObject CreateEnemy(GameObject SpawnPoint, int number)
    {
        GameObject TempEnemy = Instantiate(EnemyPrefab, SpawnPoint.transform.position, SpawnPoint.transform.rotation);
        EnemyAI TempAI = TempEnemy.GetComponent<EnemyAI>();
        TempAI.enemySpawner = this;
        TempAI.level = LevelOfEnemy;
        TempAI.spawnPoint = SpawnPoint;
        TempAI.sightRange = 50f;
        TempAI.attackRange = 3f;
        TempAI.myNumber = number;
        return TempEnemy;
    }

    private void RespawnEnemies()
    {
        Enemies[0] = CreateEnemy(SpawnPoint0,0);
        Enemies[1] = CreateEnemy(SpawnPoint1,1);
        respawning = false;
    }

    public void ResetMe(int number)
    {
        Enemies[number] = null;
    }
}
