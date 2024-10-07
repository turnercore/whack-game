using System.Collections.Generic;
using UnityEngine;

public class PickupSoundEffectsManager : MonoBehaviour
{
    [SerializeField]
    private int audioSourcesCount = 3;
    private List<AudioSource> audioSources = new();

    private Dictionary<int, float> gameObjectsInTimeout = new();

    private Dictionary<AudioClip, float> sfxInTimeout = new();

    [SerializeField]
    private float objectTimeoutTime = 0.1f;

    [SerializeField]
    private float sfxTimeoutTime = 0.1f;

    private void Start()
    {
        // Initialize the audio sources array
        for (int i = 0; i < audioSourcesCount; i++)
        {
            // Make a new game object and add it as a child
            GameObject audioSourceObject = new("AudioSource" + i);

            // Set the parent to this object
            audioSourceObject.transform.parent = transform;

            // Add an audio source component to the game object
            AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();

            // Add the audio source to the list
            audioSources.Add(audioSource);
        }

        // Subscribe to EventBus audio sfx event played
        EventBus.Instance.OnAudioSFXPlayed += PlaySound;
    }

    private void OnDestroy()
    {
        // Unsubscribe from EventBus audio sfx event played
        EventBus.Instance.OnAudioSFXPlayed -= PlaySound;
    }

    private void Update()
    {
        // Collect keys to be removed after their timeout expires
        List<int> toRemove = new();
        List<AudioClip> sfxToRemove = new();

        // Iterate over a copy of the keys to safely modify the dictionary
        foreach (var key in new List<int>(gameObjectsInTimeout.Keys))
        {
            gameObjectsInTimeout[key] -= Time.deltaTime;

            // If the timer has expired, mark the object for removal
            if (gameObjectsInTimeout[key] <= 0)
            {
                toRemove.Add(key);
            }
        }

        // Iterate over a copy of the keys to safely modify the dictionary
        foreach (var key in new List<AudioClip>(sfxInTimeout.Keys))
        {
            sfxInTimeout[key] -= Time.deltaTime;

            // If the timer has expired, mark the object for removal
            if (sfxInTimeout[key] <= 0)
            {
                sfxToRemove.Add(key);
            }
        }

        // Remove all expired GameObjects from the dictionary
        foreach (int id in toRemove)
        {
            gameObjectsInTimeout.Remove(id);
        }

        // Remove all expired sfx from the dictionary
        foreach (AudioClip sfx in sfxToRemove)
        {
            sfxInTimeout.Remove(sfx);
        }
    }

    private void PlaySound(AudioClip sfx, int objectTriggerId)
    {
        // If the object is already in the banned list, ignore its sound
        if (gameObjectsInTimeout.ContainsKey(objectTriggerId))
        {
            return;
        }

        // If the sfx is already in the banned list, ignore its sound
        if (sfxInTimeout.ContainsKey(sfx))
        {
            return;
        }

        // Add the object to the banned list with the initial timeout
        gameObjectsInTimeout[objectTriggerId] = objectTimeoutTime;
        // Add the sfx to the banned list with the initial timeout
        sfxInTimeout[sfx] = sfxTimeoutTime;

        // Loop through all audio sources to find one that is not playing
        AudioSource bestAudioSource = null;

        // Find an idle audio source
        foreach (var audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                bestAudioSource = audioSource;
                break;
            }
        }

        // If no idle AudioSource was found, pick the one closest to finishing
        if (bestAudioSource == null && audioSources.Count > 0)
        {
            bestAudioSource = audioSources[0];
            float bestTime = bestAudioSource.time;

            for (int i = 1; i < audioSources.Count; i++)
            {
                if (audioSources[i].time < bestTime)
                {
                    bestAudioSource = audioSources[i];
                    bestTime = audioSources[i].time;
                }
            }
        }

        // If a suitable audio source is found, play the sound
        if (bestAudioSource != null)
        {
            bestAudioSource.clip = sfx;
            bestAudioSource.Play();
        }
    }
}
