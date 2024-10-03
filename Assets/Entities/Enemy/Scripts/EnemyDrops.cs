using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    public GameObject[] itemDrops; // Array of drop prefabs
    public float dropChance; // Chance of dropping an item
    public GameObject xpDrop; // Array of XP drop prefabs
    public int xpAmount; // Amount of XP to drop

    // The enemy will always drop xp depending on the xpAmount, it has a chance to drop an item out of the itemDrops array

    public void DropLoot(){
        // Drop XP
        for (int i = 0; i < xpAmount; i++)
        {
            // Instantiate the XP drop prefab at the enemy's position
            Instantiate(xpDrop, transform.position, Quaternion.identity);
        }

        // Drop item
        if (Random.value <= dropChance)
        {
            // Randomly select an item drop prefab from the itemDrops array
            GameObject itemDrop = itemDrops[Random.Range(0, itemDrops.Length)];
            // Instantiate the item drop prefab at the enemy's position
            Instantiate(itemDrop, transform.position, Quaternion.identity);
        }
    }
}
