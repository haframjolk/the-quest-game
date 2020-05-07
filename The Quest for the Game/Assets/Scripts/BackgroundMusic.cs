using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void StopSmooth()
    {
        FadeToEnd(0.25f);
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
    }

    public void FadeToEnd(float duration)
    {
        StartCoroutine(FadeOut(duration));
    }
}
