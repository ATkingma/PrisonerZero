using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D _hitInfo)
    {
        if (_hitInfo.transform.CompareTag("Player"))
        {
            GameManager.Instance.BossBeaten();
        }
    }
}
