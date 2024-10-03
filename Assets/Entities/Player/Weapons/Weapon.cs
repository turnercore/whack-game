using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1.0f;
    public float addedForce = 1.0f;
    public Array hitSounds;
    public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        // Set the weapon's location to the offset
        transform.localPosition = offset;
    }
}
