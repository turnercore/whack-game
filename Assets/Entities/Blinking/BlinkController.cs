using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    private const int BLINK_ANIMATIONS = 9;

    // Start is called before the first frame update
    void Start()
    {
        // Pick a random blink animation
        ChooseBlink();
        // Subscribe to the OnDeath event in the Health script on the parent object (if it exists)
        Health health = GetComponentInParent<Health>();
        if (health != null)
        {
            health.OnDeath += Die;
        }
    }
    public void ChooseBlink()
    {
        // Choose blink animation from 0 to BlinkAnimations - 1
        int blinkIndex = Random.Range(0, BLINK_ANIMATIONS);
        // Set Animator param BlinkIndex to blinkIndex
        GetComponent<Animator>().SetInteger("BlinkIndex", blinkIndex);
    }
    public void Die()
    {
        // Play the Die animation
        GetComponent<Animator>().SetBool("isDead", true);
    }
}
