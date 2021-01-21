using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    public bool isInteractable = true;
    public bool hasSecondary = false;

    public virtual void OnPrimaryInteract() {

    }
    public virtual void OnSecondaryInteract() {

    }

    public virtual string GetPrimaryInteractText() {
        return "Primary";
    }
    public virtual string GetSecondaryInteractText() {
        return "Secondary";
    }

}