using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Simple_Enemy : MonoBehaviour
{
    private GameObject target;

    [SerializeField]
    private float movespeed = 5;

    public UnityAction OnDeath;

    private void Start()
    {
        target = FindAnyObjectByType<SpawnManager>().gameObject;

        OnDeath += () => Destroy(gameObject);
    }

    private void Update()
    {
        if (target)
        {
            LookAtTarget();
            Move();
        }
    }

    private void Move()
    {
        // funny movement
        //transform.Translate(transform.up * movespeed * Time.deltaTime);
        transform.Translate(movespeed * Time.deltaTime * new Vector3(0,1,0));

        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget < 1)
            Die();
    }

    private void LookAtTarget()
    {
        transform.up = target.transform.position - transform.position;
    }

    private void Die() => OnDeath.Invoke();
}
