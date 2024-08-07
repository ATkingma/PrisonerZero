using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hallway : MonoBehaviour
{
    [SerializeField]
    private List<SpawnInfo> spawnLocations;

    [SerializeField]
    private List<Transform> boundries;

    public List<SpawnInfo> Positions => spawnLocations;

    public List<Transform> Boundries => boundries;

    public Transform startCheck; 
    public Transform exitCheck; 
}

[Serializable]
public class SpawnInfo
{
    public Vector2 newSpawnLocation;
    public List<SpawnDirection> direction;
}

public enum SpawnDirection
{
    None,
    Up,
    Down,
    Left, 
    Right,
}
