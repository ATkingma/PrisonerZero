using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public List<WaveEnemy> enemies;
    public float waitWaveTime = 5;
}
