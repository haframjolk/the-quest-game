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

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        // If player is not currently in the process of moving
        if (!isMoving)
        {
            // Get user input
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

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
            // If player is currently walking around X/Y axis but has stopped
            else if ((currentAxis == Axis.X && moveX == 0f) || (currentAxis == Axis.Y && moveY == 0f))
            {
                currentAxis = Axis.None;
            }

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

            // Determine if player is able to move or if an obstacle is in their way
            // Cast ray from center of sprite
            Vector3 raycastOrigin = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.5f, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, playerMovement, 1, colliderLayers);
            // If an object is in the way
            if (hit.collider != null)
            {
                playerMovement = Vector3.zero;
                isMoving = false;
            }
            else
            {
                // Start player position animation (move to next tile)
                moveStartTime = Time.time;
                startPos = transform.position;
                targetPos = transform.position + playerMovement;
                journeyLength = Vector3.Distance(startPos, targetPos);
                isMoving = true;
            }
        }
        // If player is still moving, lerp their position
        else
        {
            float distCovered = (Time.time - moveStartTime) * walkSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            // If player has reached target position, let them move again
            if (transform.position == targetPos)
            {
                isMoving = false;
            }
        }
    }
}
