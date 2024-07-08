using System;
using UnityEngine;

[Serializable]
public class WeaponData
{
    [SerializeField]
    private GameObject weaponPrefab;

    [SerializeField]
    private Weapons weapon;

    public Weapons GetWeapon() { return weapon; }

    public void SetWeapon(Weapons _weapon) {  weapon = _weapon; }

    public GameObject GetWeaponPrefab() { return weaponPrefab; }

    public void SetWeaponPrefab(GameObject _newPrefab) { this.weaponPrefab = _newPrefab;  }
}

public enum Weapons
{
    pistol
}