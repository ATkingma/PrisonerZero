using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UpgradeCard : MonoBehaviour
{
    [SerializeField]
    private float chance;

    public float Chance => chance;

    [SerializeField]
    private UnityEvent upgradeEvent;

    public void Upgrade()
    {
        upgradeEvent.Invoke();
    }
}