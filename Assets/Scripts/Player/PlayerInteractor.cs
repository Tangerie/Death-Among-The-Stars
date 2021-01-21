using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;

public class PlayerInteractor : MonoBehaviour
{
    public Transform headTransform;
    private GameObject currentHit = null;

    private InteractableBase currentInteractable = null;
    public Animator CursorAnimator;

    public TextMeshProUGUI interactText;

    [SerializeField]
    private GameObject toolTipBack;
    [SerializeField]
    private GameObject outerCursor;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        MoveOffInteract();
         player = Universe.Instance.RewiredPlayer;
    }
    
    private void Update() {
        if(currentInteractable != null && currentInteractable.isInteractable && player.GetButtonDown("InteractPrimary")) {
            currentInteractable.OnPrimaryInteract();
            MoveOnInteract();
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(headTransform.position, headTransform.TransformDirection(Vector3.forward), out hit, 5)) {
            //Don't do anything if its already got it
            if(currentHit != hit.collider.gameObject) {
                currentHit = hit.collider.gameObject;

                //Get parent if current does not have it
                InteractableBase inter = currentHit.GetComponent<InteractableBase>();
                inter = inter == null ? currentHit.GetComponentInParent<InteractableBase>() : inter;
                
                if(currentInteractable != null) {
                    MoveOffInteract();
                    currentInteractable = null;
                }

                if(inter != null) {
                    currentInteractable = inter;
                    MoveOnInteract();
                }
            }
        } else {
            if(currentInteractable != null) {
                MoveOffInteract();
            }
            currentHit = null;
            currentInteractable = null;
        }
    }

    private void MoveOnInteract() {
        if(currentInteractable.isInteractable) {
            string txt = currentInteractable.GetPrimaryInteractText();
            
            if(txt == null) return;

            interactText.SetText(txt);
            LeanTween.scaleY(interactText.gameObject, 1, 0.1f);
            LeanTween.scale(outerCursor, Vector3.one * 1.5f, 0.1f);
            LeanTween.scale(toolTipBack, Vector3.one, 0.1f);
        }
    }

    private void MoveOffInteract() {
        LeanTween.scaleY(interactText.gameObject, 0, 0.1f);
        LeanTween.scale(outerCursor, Vector3.one, 0.1f);
        LeanTween.scale(toolTipBack, Vector3.zero, 0.1f);
    }
}
