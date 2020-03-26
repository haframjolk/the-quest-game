using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    private PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();    
    }

    public void PauseTimeline()
    {
        // Minnkar hraðann á tímalínunni í 0 svo hún hætti að hreyfast en hætti þó ekki að vera „evaluated“
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void PlayTimeline()
    {
        // Stillir hraðann aftur á 1 svo tímalínan haldi venjulega áfram
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}
