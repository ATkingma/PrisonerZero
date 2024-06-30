using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveEnemy
{
    public GameObject enemy;
    public int amount;
    public float waitTime = .1f;
    public float waitTimeNextEnemy = 1;

}
