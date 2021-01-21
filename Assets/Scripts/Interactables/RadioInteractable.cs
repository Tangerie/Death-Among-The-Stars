using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioInteractable : InteractableBase
{
    private bool isOn = true;

    AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        isOn = !audioSource.mute;
    }

    public override void OnPrimaryInteract() {
        audioSource.mute = isOn;
        isOn = !isOn;
    }
    public override void OnSecondaryInteract() {

    }

    public override string GetPrimaryInteractText() {
        if(isOn) {
            return "Turn Off";
        } else {
            return "Turn On";
        }
    }
}
