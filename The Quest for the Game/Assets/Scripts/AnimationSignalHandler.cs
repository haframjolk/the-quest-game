using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSignalHandler : MonoBehaviour
{
    private Animator animator;

    // Stilla hvert leikmaður snýr (fyrir timeline events)
    public void SetAnimatorDirection(int direction)
    {
        animator.SetInteger("Direction", direction);
    }
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
}
