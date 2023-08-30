using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterPool : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;

    private Queue<GameObject>[] pools;

    private void Awake()
    {
        GameManager.MonsterPool = this;
        pools = new Queue<GameObject>[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    private void Creat(int i)
    {
        GameObject instance = Instantiate(prefabs[i]);
        pools[i].Enqueue(instance);
    }

    public GameObject Get(Transform parent, int i)
    {
        if (pools[i].Count < 1) Creat(i);
        GameObject instance = pools[i].Dequeue();
        instance.SetActive(true);
        instance.transform.SetParent(parent);
        return instance;
    }

    public void ReturnObject(GameObject instance, int i)
    {
        instance.SetActive(false);
        instance.transform.SetParent(transform);
        pools[i].Enqueue(instance);
    }

    private void Destroy(int i)
    {
        Destroy(pools[i].Dequeue());
    }
}
