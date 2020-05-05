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

    IEnumerator WaitThenDisable()
    {
        float timeToWait = 0f;
        if (collectClip)
        {
            timeToWait = collectClip.length;
        }
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
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
        // Slökkva á collider og renderer á meðan hljóðið spilast, áður en slökkt er á hlutnum, svo hann hverfi strax
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        // Slökkva á honum þegar hljóðið er búið að spilast
        StartCoroutine(WaitThenDisable());
    }
    public override void Interact()
    {
        Collect();
    }
}
