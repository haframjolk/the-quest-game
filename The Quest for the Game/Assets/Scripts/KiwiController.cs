using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiwiController : InteractableNPCController
{
    public bool letsPlayerPass;
    public GameObject borderWall;

    public void SetPlayerPassStatus(bool value)
    {
        letsPlayerPass = value;
    }

    public override void Interact()
    {
        // Ef leikmaður má komast í gegn er ósýnilega landamæraveggnum eytt og Interact() aðferðin keyrð
        if (letsPlayerPass)
        {
            Destroy(borderWall);
            base.Interact();
        }
    }
}
