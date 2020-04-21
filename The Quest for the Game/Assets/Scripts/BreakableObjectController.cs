using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectController : InteractableController
{
    public bool isBreakable = true;

    public void SetBreakableStatus(bool value)
    {
        isBreakable = value;
    }

    public void BreakObject()
    {
        if (isBreakable)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Interact()
    {
        Debug.Log("Player interacted with breakable object.");
        BreakObject();
    }
}
