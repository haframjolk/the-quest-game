using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public float fadeoutDuration = 0.1f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    // Stoppa afspilun hljóðs með stuttu fade-out
    public void StopSmooth()
    {
        FadeToEnd(fadeoutDuration);
    }

    // Láta hljóð fjara út
    public IEnumerator FadeOut(float duration)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        Stop();
        audioSource.volume = startVolume;
    }

    public void FadeToEnd(float duration)
    {
        StartCoroutine(FadeOut(duration));
    }
}
