using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : InteractableController
{
    public GameObject itemGetCanvas;
    public float itemGetDuration = 7f / 6f;  // Sjálfgefin lengd 1 1/6 sek (70 rammar í cutscenes, sem eru 60 fps)
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator WaitThenDisable(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        // Slökkva á „item get“ fyrst ef það er til staðar
        if (itemGetCanvas)
        {
            itemGetCanvas.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    public virtual void Collect()
    {
        // Sýna „item get“ þegar notandi nær í hlutinn
        if (itemGetCanvas)
        {
            itemGetCanvas.SetActive(true);
        }
        // Slökkva á collider og renderer á meðan „item get“ er sýnt, áður en slökkt er á hlutnum, svo hann hverfi strax
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        // Slökkva á honum þegar „item get“ er búið
        StartCoroutine(WaitThenDisable(itemGetDuration));
    }
    public override void Interact()
    {
        Collect();
    }
}
