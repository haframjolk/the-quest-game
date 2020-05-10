using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector timeline;
    
    public void Trigger()
    {
        timeline.Play();
        // Debug.Log("Cutscene triggered!");
    }
}
