using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : InteractableController
{
    public AudioClip collectClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator WaitThenDestroy()
    {
        float timeToWait = 0f;
        if (collectClip)
        {
            timeToWait = collectClip.length;
        }
        yield return new WaitForSeconds(timeToWait);
        Destroy(gameObject);
    }

    public virtual void Collect()
    {
        float timeToWait = 0f;
        // Spila hljóð þegar notandi nær í hlutinn
        if (collectClip && !audioSource.isPlaying)
        {
            timeToWait = collectClip.length;
            audioSource.PlayOneShot(collectClip);
        }
        // Slökkva á collider og renderer á meðan hljóðið spilast, áður en hlutnum er eytt, svo hann hverfi strax
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        // Eyða honum þegar hljóðið er búið að spilast
        StartCoroutine("WaitThenDestroy");
    }
    public override void Interact()
    {
        Collect();
    }
}
