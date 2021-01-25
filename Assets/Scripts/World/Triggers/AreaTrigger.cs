using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public abstract class AreaTrigger : MonoBehaviour
{
    public enum AreaTriggerBehaviour {
        Once,
        Always
    }

    public AreaTriggerBehaviour Behaviour;

    private Collider areaCol;

    private void Awake() {
        areaCol = GetComponent<Collider>();
        areaCol.isTrigger = true;
    }

    public UnityAction<Collider> EntryActions;
    private void OnTriggerEnter(Collider other) {
        EntryActions.Invoke(other);
        if(Behaviour == AreaTriggerBehaviour.Once) {
            this.enabled = false;
            Destroy(this.gameObject);
            Debug.Log("Destroying Trigger");
        }
    }
}
