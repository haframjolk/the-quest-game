using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RasmusenChaseController : MonoBehaviour
{
    public LayerMask playerLayer;
    public bool isActive = false;
    public TimelineController timeline;
    public PlayableDirector caughtMessageTimeline;
    private Animator animator;
    private Vector3 startPos;
    private Direction startDir = Direction.None;

    public void ResetRasmusen()
    {
        timeline.StopTimeline();
        transform.position = startPos;
        animator.SetInteger("Direction", (int)startDir);
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    void Update()
    {
        if (isActive)
        {
            Direction currentDir = (Direction)animator.GetInteger("Direction");
            Vector3 raycastDir = Vector3.zero;
            if (currentDir == Direction.Left)
            {
                raycastDir = Vector3.left;
            }
            else if (currentDir == Direction.Right)
            {
                raycastDir = Vector3.right;
            }
            else if (currentDir == Direction.Up)
            {
                raycastDir = Vector3.up;
            }
            else if (currentDir == Direction.Down)
            {
                raycastDir = Vector3.down;
            }
            // Raycast
            Vector3 raycastOrigin = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.5f, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDir, 1, playerLayer);
            Debug.DrawRay(raycastOrigin, raycastDir, Color.green);
            // Ef Rasmusen er að fara að rekast á leikmann, stoppa hann og núllstilla
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                timeline.PauseTimeline();
                caughtMessageTimeline.Play();
                isActive = false;
            }
        }
        else
        {
            // Geyma síðustu átt sem notandinn sneri í áður en eltingaleikur byrjaði
            startDir = (Direction)animator.GetInteger("Direction");
        }
    }
}
