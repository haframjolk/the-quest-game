using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float lerpSpeed = 1.0f;
    public bool isFrozen = false;
    private Vector3 offset;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float lerpStartTime;
    private float journeyLength;
    private bool isLerping;

    // Stilla hvort myndavél sé frosin
    public void SetFrozen(bool value)
    {
        isFrozen = value;
    }

    // Stilla hvaða leikmann myndavélin á að elta
    public void SetMainPlayer(Transform mainPlayer)
    {
        player = mainPlayer;
    }

    // Færa myndavél yfir að leikmanni með lerp
    public void SetMainPlayerLerp(Transform mainPlayer)
    {
        SetMainPlayer(mainPlayer);

        // Undirbúa að færa myndavél yfir á nýja leikmanninn
        lerpStartTime = Time.time;
        startPos = transform.position;
        targetPos = mainPlayer.transform.position + offset;
        journeyLength = Vector3.Distance(startPos, targetPos);
        isLerping = true;
    }

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // Ef myndavélin er frosin, ekki gera neitt
        if (isFrozen)
        {
            return;
        }
        // Ef myndavélin er að hreyfa sig að öðrum leikmanni, færa hana nær
        if (isLerping)
        {
            targetPos = player.transform.position + offset;
            float distCovered = (Time.time - lerpStartTime) * lerpSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            // Ef myndavélin er komin á áfangastað er hún ekki lengur að hreyfa sig
            if (transform.position == targetPos)
            {
                isLerping = false;
            }
        }
        else
        {
            // Hreyfa myndavél með leikmanni og halda honum alltaf í miðjunni
            transform.position = player.transform.position + offset;
        }
    }
}
