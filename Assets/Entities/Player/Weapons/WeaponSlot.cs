using System;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    private GameObject _weaponPrefab;
    public GameObject WeaponPrefab
    {
        get { return _weaponPrefab; }
        set
        {
            _weaponPrefab = value;
            SetWeapon(_weaponPrefab);
        }
    }

    // Weapon script
    public Weapon weapon;

    // Event on weapon set
    public event Action<Weapon> OnWeaponSet;

    public Weapon SetWeapon(GameObject weaponPrefab)
    {
        // Delete the current child weapon object
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Check if null
        if (weaponPrefab == null)
        {
            return null;
        }

        // Instantiate the weapon prefab
        GameObject weaponObject = Instantiate(weaponPrefab, transform);

        // Set weapon to the Weapon component
        weapon = weaponObject.GetComponent<Weapon>();

        // Trigger the event
        Debug.Log("Weapon set: " + weapon.name);
        OnWeaponSet?.Invoke(weapon);

        // Return the weapon
        return weapon;
    }

    Weapon GetWeapon()
    {
        // Get the weapon prefab from the weapon slot if it exists
        return weapon;
    }

    GameObject GetWeaponPrefab()
    {
        // Get the weapon prefab from the weapon slot if it exists
        return _weaponPrefab;
    }
}
