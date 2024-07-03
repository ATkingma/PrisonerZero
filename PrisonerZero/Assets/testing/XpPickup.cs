using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class XpPickup : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetlayer;

    [SerializeField]
    private float xpValue;

    private void OnTriggerEnter2D(Collider2D _hitInfo)
    {
        if (_hitInfo.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            XpManager.Instance.AddXp(xpValue);
            this.gameObject.SetActive(false);
        }
    }
}
