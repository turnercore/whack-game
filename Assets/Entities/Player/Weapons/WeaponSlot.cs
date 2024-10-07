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

    public void SetWeapon(GameObject weaponPrefab)
    {
        // Delete the current child weapon object
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Check if null
        if (weaponPrefab == null)
        {
            return;
        }

        // Instantiate the weapon prefab
        GameObject weaponObject = Instantiate(weaponPrefab, transform);

        // Set weapon to the Weapon component
        weapon = weaponObject.GetComponent<Weapon>();

        // Set the weapon as child
        weaponObject.transform.parent = transform;

        // Set the weapon's location to the offset
        weaponObject.transform.localPosition = weapon.offset;
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
