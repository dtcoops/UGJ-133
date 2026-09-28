using UnityEngine;

public class UIClickSound : MonoBehaviour
{
    public static UIClickSound Instance { get; private set; }

    public AudioClip clickSound;
    [Range(0f, 1f)] public float volume = 1f;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void PlayClick()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound, volume);
        }
    }
}