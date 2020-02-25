using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    X = 1,
    Y = -1,
    None = 0
}
public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public LayerMask colliderLayers;
    private Rigidbody2D rb2d;
    private Axis currentAxis = Axis.None;
    private bool isMoving = false;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float moveStartTime;
    private float journeyLength;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    void FixedUpdate()
    {
        // Ef leikmaður er ekki að hreyfa sig
        // If player is not currently in the process of moving
        if (!isMoving)
        {
            // Sækja skipanir frá notanda
            // Get user input
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            // Ef leikmaður er ekki að hreyfa sig á neinum ás, finna réttan ás (x fær forgang)
            // If player is currently not moving along any axis, find the correct axis (prefer x if both are active)
            if (currentAxis == Axis.None)
            {
                if (moveX != 0f)
                {
                    currentAxis = Axis.X;
                }
                else if (moveY != 0f)
                {
                    currentAxis = Axis.Y;
                }
            }
            // Ef leikmaður var að hreyfa sig á X/Y ásnum en er nú stopp
            // If player is currently walking around X/Y axis but has stopped
            else if ((currentAxis == Axis.X && moveX == 0f) || (currentAxis == Axis.Y && moveY == 0f))
            {
                currentAxis = Axis.None;
            }

            // Skilgreina hreyfingu leikmanns, byggt á ási
            // Define player movement based on axis
            Vector3 playerMovement = Vector3.zero;
            if (currentAxis == Axis.X)
            {
                playerMovement = new Vector3(System.Math.Sign(moveX), 0f, 0f);
            }
            else if (currentAxis == Axis.Y)
            {
                playerMovement = new Vector3(0f, System.Math.Sign(moveY), 0f);
            }

            // Athuga hvort leikmaður geti hreyft sig eða hvort eitthvað sé í vegi hans
            // Determine if player is able to move or if an obstacle is in their way
            
            // Kasta geisla frá miðju sprite-ins
            // Cast ray from center of sprite
            Vector3 raycastOrigin = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.5f, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, playerMovement, 1, colliderLayers);
            // Ef hlutur er í veginum
            // If an object is in the way
            if (hit.collider != null)
            {
                playerMovement = Vector3.zero;
                isMoving = false;
            }
            else
            {
                // Byrja að hreyfa leikmann á næstu flís
                // Start player position animation (move to next tile)
                moveStartTime = Time.time;
                startPos = transform.position;
                targetPos = transform.position + playerMovement;
                journeyLength = Vector3.Distance(startPos, targetPos);
                isMoving = true;
            }
        }
        // Ef leikmaður er enn að hreyfa sig, nota lerp til að stilla staðsetninguna
        // If player is still moving, lerp their position
        else
        {
            float distCovered = (Time.time - moveStartTime) * walkSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            // Ef leikmaðurinn er kominn þangað sem hann ætlaði sér að fara, leyfa honum að hreyfa sig aftur
            // If player has reached target position, let them move again
            if (transform.position == targetPos)
            {
                isMoving = false;
            }
        }
    }
}
