using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Add array of hit sounds
    [SerializeField]
    private AudioClip[] hitSounds;

    [SerializeField]
    private ParticleSystem hitParticles;
    public float damage = 1.0f;
    public float addedForce = 1.0f;
    public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        // Set the weapon's location to the offset
        transform.localPosition = offset;
    }

    public void PlayHitEffects()
    {
        PlayHitSound();
        if (hitParticles != null)
            PlayHitParticles();
    }

    // Play a random hit sound from the array, but only if one is not already playing
    void PlayHitSound()
    {
        if (hitSounds.Length > 0)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (!audioSource.isPlaying)
            {
                audioSource.clip = hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)];
                audioSource.Play();
            }
        }
    }

    void PlayHitParticles()
    {
        hitParticles.Play();
    }
}
