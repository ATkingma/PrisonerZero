using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [SerializeField]
    private float health;   
    public virtual void DoDamage(float _damage)
    {
        if(health-_damage <= 0)
        {
            Death();
            return;
        }

        health -= _damage;  
    }

    public virtual void Death()
    {
        Destroy(gameObject);    
    }
}
