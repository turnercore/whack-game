using UnityEngine;

public class XpPickup : Pickup
{
    // The amount of XP to give to the player
    public int xpAmount = 1;

    // This function is called when the pickup reaches the player
    protected override void OnReachPlayer(GameObject player)
    {
        // Add the XP to the player
        player.GetComponent<PlayerController>().AddXP(xpAmount);

        // Destroy the pickup object
        Destroy(gameObject);
    }
}
