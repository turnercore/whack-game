using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup
{
    public int value = 1;
    public float health = 1f;
    // Start is called before the first frame update
    protected override void OnReachPlayer(GameObject player)
    {
        if (player.CompareTag("Player"))
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            // Add the coins to the player
            playerController.AddCoins(value);
            // Add health to the player
            playerController.Heal(health);
            // Destroy the pickup object
            Destroy(gameObject);
        }
    }
}
