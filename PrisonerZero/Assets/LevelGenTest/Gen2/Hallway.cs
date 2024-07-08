using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour
{
    [SerializeField]
    private List<Vector2> newSpawnLocations;

    [SerializeField]
    private List<Transform> boundries;

    public List<Vector2> Positions => newSpawnLocations;

    public List<Transform> Boundries => boundries;
}
