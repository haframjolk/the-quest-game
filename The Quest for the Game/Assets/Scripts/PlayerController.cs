using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ásar til að passa upp á að hreyfing sé lögleg
public enum Axis
{
    X = 1,
    Y = -1,
    None = 0
}
// Áttir fyrir animation
public enum Direction
{
    Left = -1,
    Right = 1,
    Up = 2,
    Down = 4,
    None = 0
}
// Staðsetning og átt, til að geta hreyft þræl í samræmi við hreyfingar aðalleikmanns
public class SlaveTarget
{
    public Vector3 pos;
    public Direction direction;
    
    public SlaveTarget(Vector3 pos, Direction direction)
    {
        this.pos = pos;
        this.direction = direction;
    }
}

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public LayerMask colliderLayers;
    public bool isFrozen = false;
    private Animator animator;
    private Rigidbody2D rb2d;
    private float moveX;
    private float moveY;
    private Axis currentAxis = Axis.None;
    private bool isMoving = false;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float moveStartTime;
    private float journeyLength;
    public SlavePlayerController slavePlayer;
    private List<SlaveTarget> savedSlaveTargets;  // Notað til að halda utan um fyrri staðsetningar og animator directions leikmanns svo þræll (slave) geti elt

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        savedSlaveTargets = new List<SlaveTarget>();
    }

    public void SetFrozen(bool value)
    {
        isFrozen = value;
    }

    void Update()
    {
        // Sækja hreyfiskipanir frá notanda
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        // Ef notandi ýtir á Interact-takkann, athuga hvort interactable hlutur sé innan seilingar
        if (Input.GetButton("Interact"))
        {
            // Finna út í hvaða átt leikmaður snýr
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
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDir, 1, colliderLayers);
            // Ef finnst interactable hlutur, kalla á Interact() aðferðina
            if (hit.collider != null && hit.collider.tag == "Interactable")
            {
                InteractableController interactable = hit.collider.GetComponent<InteractableController>();
                interactable.Interact();
                // Debug.Log("Player interacted with object " + interactable.ToString());
            }
        }
    }

    void FixedUpdate()
    {
        // Ekki leyfa frosnum leikmanni að hreyfa sig
        if (isFrozen)
        {
            return;
        }
        // Ef leikmaður er ekki að hreyfa sig
        if (!isMoving)
        {
            int moveSign = 0;

            Direction currentDir = Direction.None;

            // Ef leikmaður er ekki að hreyfa sig á neinum ás, finna réttan ás (x fær forgang)
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
            else if ((currentAxis == Axis.X && moveX == 0f) || (currentAxis == Axis.Y && moveY == 0f))
            {
                currentAxis = Axis.None;
            }
            
            // Stilla animator parameter fyrir átt
            else
            {
                // Er hreyfing pósitíf eða negatíf?
                if (currentAxis == Axis.X)
                {
                    moveSign = System.Math.Sign(moveX);
                }
                else if (currentAxis == Axis.Y)
                {
                    moveSign = System.Math.Sign(moveY);
                }

                // Finna í hvaða átt leikmaðurinn snýr
                if (currentAxis == Axis.X)
                {
                    if (moveSign == 1)
                    {
                        currentDir = Direction.Right;
                    }
                    else
                    {
                        currentDir = Direction.Left;
                    }
                }
                else
                {
                    if (moveSign == 1)
                    {
                        currentDir = Direction.Up;
                    }
                    else
                    {
                        currentDir = Direction.Down;
                    }
                }
                if (currentDir != Direction.None)
                {
                    animator.SetInteger("Direction", (int)currentDir);
                }
            }

            // Stilla animator parameter yfir hvort leikmaður sé að labba eða ekki
            if (moveX != 0f || moveY != 0f)
            {
                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);
            }

            // Skilgreina hreyfingu leikmanns, byggt á ási
            Vector3 playerMovement = Vector3.zero;
            if (currentAxis == Axis.X)
            {
                playerMovement = new Vector3(moveSign, 0f, 0f);
            }
            else if (currentAxis == Axis.Y)
            {
                playerMovement = new Vector3(0f, moveSign, 0f);
            }

            // Athuga hvort leikmaður geti hreyft sig eða hvort eitthvað sé í vegi hans
            // Kasta geisla frá miðju sprite-ins
            Vector3 raycastOrigin = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.5f, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, playerMovement, 1, colliderLayers);
            // Ef hlutur er í veginum
            if (hit.collider != null)
            {
                playerMovement = Vector3.zero;
                isMoving = false;
            }
            else
            {
                // Byrja að hreyfa leikmann á næstu flís
                moveStartTime = Time.time;
                startPos = transform.position;
                targetPos = transform.position + playerMovement;
                journeyLength = Vector3.Distance(startPos, targetPos);
                isMoving = true;
                
                // Ef þræll er til staðar og leikmaður er á leið á aðra flís, halda utan um þá hreyfingu
                if (slavePlayer && startPos != targetPos)
                {
                    savedSlaveTargets.Add(new SlaveTarget(targetPos, currentDir));
                    // Ef leikmaður hefur hreyft sig nógu oft hreyfir þrællinn sig í samræmi við það
                    if (savedSlaveTargets.Count > slavePlayer.stepOffset)
                    {
                        slavePlayer.MoveTo(savedSlaveTargets[0], walkSpeed);
                        savedSlaveTargets.RemoveAt(0);
                    }
                }
            }
        }
        // Ef leikmaður er enn að hreyfa sig, nota lerp til að stilla staðsetninguna
        else
        {
            float distCovered = (Time.time - moveStartTime) * walkSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            // Ef leikmaðurinn er kominn þangað sem hann ætlaði sér að fara, leyfa honum að hreyfa sig aftur
            if (transform.position == targetPos)
            {
                isMoving = false;
            }
        }
    }
}
