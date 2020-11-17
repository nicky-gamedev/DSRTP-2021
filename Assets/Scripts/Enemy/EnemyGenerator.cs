//Follows the player in the X axis and spawns based on bounds coordinates

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefabEnemy;
    [SerializeField] Bounds areaOfSpawning;
    [SerializeField] Transform player;

    private void Awake()
    {
        areaOfSpawning = GetComponent<Collider>().bounds;
        GetComponent<Collider>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }

    public GameObject SpawnEnemy()
    {
        Debug.Log("Spawning enemy");
        Vector3 spawnPosition = new Vector3(
        Random.Range(areaOfSpawning.min.x, areaOfSpawning.max.x),
        transform.position.y,
        Random.Range(areaOfSpawning.min.z, areaOfSpawning.max.z));

        return Instantiate(prefabEnemy, spawnPosition, Quaternion.identity);
    }
}
