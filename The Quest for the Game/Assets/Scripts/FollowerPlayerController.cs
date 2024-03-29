﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerPlayerController : MonoBehaviour
{
    public int stepOffset = 2;
    private float walkSpeed;
    private float moveStartTime;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float journeyLength;
    private bool isMoving = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Ef leikmaður hefur hreyft sig nóg til að fylgjandi eigi að hreyfa sig er kallað í þetta fall, sem undirbýr hreyfinguna
    public void MoveTo(FollowerTarget target, float speed)
    {
        // Stilla allar þær upplýsingar sem þarf til að færa fylgjanda
        this.walkSpeed = speed;
        this.moveStartTime = Time.time;
        this.startPos = transform.position;
        this.targetPos = target.pos;
        this.journeyLength = Vector3.Distance(startPos, targetPos);
        this.isMoving = true;

        // Stilla animator-eigindi
        animator.SetInteger("Direction", (int)target.direction);
        animator.SetBool("Walking", true);
    }

    void FixedUpdate()
    {
        // Ef fylgjandinn er að hreyfa sig, færa hann áfram nær næstu flís
        if (isMoving)
        {
            float distCovered = (Time.time - moveStartTime) * walkSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            // Ef leikmaðurinn er kominn þangað sem hann ætlaði sér að fara er hann ekki lengur að hreyfa sig
            if (transform.position == targetPos)
            {
                isMoving = false;
            }
        }
        // Ef hann er ekki að hreyfa sig, slökkva á Walking í animator
        else
        {
            animator.SetBool("Walking", false);
        }
    }
}
