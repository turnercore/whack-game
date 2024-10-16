using UnityEngine;

public class WeaponPickup : Pickup
{
    // Weapon Prefab
    [SerializeField]
    private GameObject weapon;

    [SerializeField]
    private SpriteRenderer sprite;

    // On start set sprite to the weapon prefabs sprite
    protected override void Start()
    {
        base.Start();
        sprite.sprite = weapon.GetComponentInChildren<SpriteRenderer>().sprite;
        sprite.transform.localScale = weapon
            .GetComponentInChildren<SpriteRenderer>()
            .transform.localScale;
    }

    // on reach player, add weapon to player
    protected override void OnReachPlayer(GameObject player)
    {
        // Play Sound Effect
        PlayPickupSound();
        // Add the weapon to the player
        player.GetComponent<PlayerController>().AddWeapon(weapon);
        // Destroy the pickup object
        Destroy(gameObject);
    }
}
