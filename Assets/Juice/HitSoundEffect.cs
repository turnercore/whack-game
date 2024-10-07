using UnityEngine;

public class HitSoundEffect : Juice
{
    public AudioClip[] hitSounds; // Array of AudioClips to play

    [SerializeField]
    private bool onlyOnWeaponHit = false;

    // 2D collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignore edge colliders, inactive states, or low-velocity collisions
        if (
            collision.collider is EdgeCollider2D
            || !isActive
            || collision.relativeVelocity.magnitude < 0.1f
        )
            return;

        // Get instance IDs of both colliding objects
        int instanceId = gameObject.GetInstanceID();
        int otherInstanceId = collision.gameObject.GetInstanceID();

        // Check if the other collider has a HitSoundEffect component
        HitSoundEffect otherHitSoundEffect = collision.gameObject.GetComponent<HitSoundEffect>();

        // If the other object has a HitSoundEffect and is active, only play the sound if this object's ID is greater
        if (
            otherHitSoundEffect != null
            && otherHitSoundEffect.isActive
            && instanceId < otherInstanceId
        )
        {
            return;
        }

        // Play sound if hit by a weapon or if not limited to weapon hits
        if (!onlyOnWeaponHit || collision.collider.CompareTag("Weapon"))
        {
            // Play a random sound from the array
            if (hitSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, hitSounds.Length);
                AudioClip selectedSound = hitSounds[randomIndex];
                EventBus.Instance.TriggerAudioSFXPlayed(selectedSound, instanceId);
            }
        }
    }
}
