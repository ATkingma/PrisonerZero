using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRigid : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    void Update()
    {
        if(rb != null)
        {
            Debug.Log(rb.velocity);
        }
    }
}
