using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public void SetInteractable(bool value)
    {
        if (value)
        {
            gameObject.tag = "Interactable";
        }
        else
        {
            gameObject.tag = "Untagged";
        }
    }
    public virtual void Interact()
    {

    }
}
