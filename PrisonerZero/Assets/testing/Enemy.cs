using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private NavMeshAgent agent;

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false; 
    }

    private void Update()
    {
        agent.SetDestination(target.position);
    }
}
