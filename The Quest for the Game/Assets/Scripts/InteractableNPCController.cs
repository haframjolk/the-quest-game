using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InteractableNPCController : InteractableController
{
    public PlayableDirector timeline;

    public override void Interact()
    {
        timeline.Play();
    }
}
