using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int count;
    [SerializeField] private float spawnDis;
    [SerializeField] private float spawnTime;
    private float spawnTimer;
    private void Update()
    {
        if (Time.time > spawnTimer)
        {
            GameManager.monsterPool.Get(null, 0).transform.position 
                = PlayerController.playerPos + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * spawnDis;
            spawnTimer = Time.time + spawnTime;
        }
    }
}
