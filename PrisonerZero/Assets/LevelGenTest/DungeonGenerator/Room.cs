using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private List<Transform> boundries;

    public List<Transform> Boundries => boundries;
}
