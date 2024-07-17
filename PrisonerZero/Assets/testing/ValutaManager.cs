using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValutaManager : MonoBehaviour
{
    public static ValutaManager Instance { get; private set; }

    private Valuta currentValuta;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;    

        if(DataManager.Instance != null)
        {

        }
    }
    public void AddSoulFragements(int amount)
    {

    }
}
