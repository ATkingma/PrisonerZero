using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{   public static WeaponManager Instance => instance;

    private static WeaponManager instance;

    [SerializeField]
    private List<WeaponData> weapons;

    private WeaponData selectedWeapon;

    public WeaponData SelectedWeapon =>selectedWeapon;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject); 
    }

    public void SetWeapon(int x)
    {
        for (int y = 0; y < weapons.Count; y++)
        {
            if ((int)weapons[y].GetWeapon() == x)
            {
                selectedWeapon = weapons[y];    
            }
        }
    }

    public void SetWeapon(Weapons x)
    {
        for (int y = 0; y < weapons.Count; y++)
        {
            if (weapons[y].GetWeapon() == x)
            {
                selectedWeapon = weapons[y];
            }
        }
    }

    public void SetWeapon(WeaponData data)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            selectedWeapon = data;
        }
    }

    public WeaponData GetWeapon() { return selectedWeapon; }
}