using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    public static Vector3 PlayerPos;

    private void Update()
    {
        PlayerPos = transform.position;
    }
}
