using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CampfireInteractable : InteractableBase
{
    private bool isCampfireActive;

    private Light fireLight;
    private AudioSource fireCrackle;
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private void Awake() {
        fireLight = GetComponentInChildren<Light>();
        fireLight.enabled = false;

        fireCrackle = GetComponent<AudioSource>();
        fireCrackle.Stop();

        foreach(var part in particleSystems) {
            part.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public override void OnPrimaryInteract() {
        if(isCampfireActive) {

        } else {
            isCampfireActive = true;
            fireLight.enabled = true;

            float lightIntensity = fireLight.intensity;

            LeanTween.value(fireLight.gameObject, (intensity) => {
                fireLight.intensity = intensity;
            }, 0, fireLight.intensity, 1);

            fireCrackle.Play();
            foreach(var part in particleSystems) {
                part.Clear();
                part.Play();
            }
        }
    }
    public override void OnSecondaryInteract() {

    }

    public override string GetPrimaryInteractText() {
        if(isCampfireActive) {
            return null;
        } else {
            return "Light Campfire";
        }
    }
}
