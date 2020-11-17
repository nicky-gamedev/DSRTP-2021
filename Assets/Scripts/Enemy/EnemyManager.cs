//This system works by counting how many minutes the player still have
//And using this value to spawn enemies based on a fixed number of enemies per minute.
//It also checks for idle enemies, stores in a dictionary and clears them once it stays in idle for too long

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<Enemy> enemiesSpawned;
    [SerializeField] int maxEnemiesSpawnedPerMinute = 2;
    [SerializeField] float maxTimeIdle = 25;
    [SerializeField] EnemyGenerator generator;

    int minutesTotal;
    Dictionary<Enemy, float> idleEnemies;

    private void Awake()
    {
        //Load
        enemiesSpawned = new List<Enemy>();
        generator = GetComponent<EnemyGenerator>();
        idleEnemies = new Dictionary<Enemy, float>();
    }

    private void Start()
    {
        //Getting total minutes in int, to compare
        minutesTotal = Mathf.RoundToInt(GameManager.instance.fullTime / 60);
    }

    void Update()
    {
        //getting the actual minutes
        int minutesRemaining = (GameManager.instance.ActualTime / 60);

        //if the list is smaller than the minutes passed times the enemies per minute
        if(maxEnemiesSpawnedPerMinute * (minutesTotal - minutesRemaining) >= enemiesSpawned.Count)
        {
            //Spawn more pls :)
            Debug.Log("Enemies spawned per minute are less than enemies spawned");
            SpawnFromGeneratorAndAdd(maxEnemiesSpawnedPerMinute * (minutesTotal - minutesRemaining));
        }

        //Checking for Idle Enemies
        foreach(var item in enemiesSpawned)
        {
            //If they're not idle and aren't in the list, skip.
            if (!(item.enemyState == EnemyBrain.States.IDLE) && !idleEnemies.ContainsKey(item)) continue;
            //If the item isn't idle, but is inside the list, we remove it.
            else if(idleEnemies.ContainsKey(item)) idleEnemies.Remove(item);

            //if the item is inside the list and its idle, we subtract Time.deltaTime
            if (idleEnemies.ContainsKey(item))
            {
                idleEnemies[item] -= Time.deltaTime;

                //If the enemy value is zero or below, we destroy it.
                if(idleEnemies[item] <= 0)
                {
                    enemiesSpawned.Remove(item);
                    Destroy(item.gameObject);
                }
            }
            //If it's idle and aren't in the list, we add it :)
            else
            {
                idleEnemies.Add(item, maxTimeIdle);
            }
        }
    }

    //Just calls the generator and adds to the list
    private void SpawnFromGeneratorAndAdd(int v)
    {
        for (int i = 0; i < v; i++)
        {
            var temp = generator.SpawnEnemy();
            enemiesSpawned.Add(temp.GetComponent<Enemy>());
        }
    }
}
