using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }

    public AudioClip musicClip;
    [Range(0f, 1f)] public float volume = 0.5f;

    AudioSource source;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = gameObject.AddComponent<AudioSource>();
        source.loop = true;
        source.spatialBlend = 0f;
        source.volume = volume;
        source.playOnAwake = false;

        StartCoroutine(PlayWhenReady());
    }

    IEnumerator PlayWhenReady()
    {
        if (musicClip == null) yield break;

        while (musicClip.loadState == AudioDataLoadState.Loading)
        {
            yield return null;
        }

        if (musicClip.loadState == AudioDataLoadState.Loaded)
        {
            source.clip = musicClip;
            source.Play();
        }
    }
}