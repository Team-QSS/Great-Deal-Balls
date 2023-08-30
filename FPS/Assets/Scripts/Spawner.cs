using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPointTransforms;
    
    [SerializeField] private int   count;
    [SerializeField] private float spawnTime;
    
    private                  float spawnTimer;
    private void Update()
    {
        if (Time.time > spawnTimer)
        {
            GameManager.MonsterPool.Get(null, 0).transform.position
                = spawnPointTransforms[Random.Range(0, spawnPointTransforms.Length)].position;
            spawnTimer = Time.time + spawnTime;
        }
    }
}
