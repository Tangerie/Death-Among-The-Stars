using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FauxGravityBody : MonoBehaviour
{

    private Rigidbody rigidbody;

    public const float ROTATION_SPEED = 100.0f;

    public bool doesAlignWithGravity = true;
    public float alignMinAcceleration = 1f;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        Universe.Instance.FauxGravityBodies.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        FauxGravityAttractor closestBody = null;
        Vector3 largestAcc = Vector3.zero;
        foreach(FauxGravityAttractor attractor in Universe.Instance.FauxGravityAttractors) {
            Vector3 acc = calcAcceleration(attractor);
            rigidbody.AddForce(acc * rigidbody.mass);
            if(acc.sqrMagnitude > largestAcc.sqrMagnitude) {
                closestBody = attractor;
                largestAcc = acc;
            }
        }   

        if(closestBody != null && doesAlignWithGravity && largestAcc.sqrMagnitude > alignMinAcceleration * alignMinAcceleration) {
            //transform.up = -largestAcc.normalized;
            Quaternion newRot = Quaternion.FromToRotation(transform.up, -largestAcc.normalized) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, ROTATION_SPEED * Time.deltaTime);
        }
    }

    private Vector3 calcAcceleration(FauxGravityAttractor attractor) {
        Vector3 heading = attractor.transform.position - transform.position;
        return heading.normalized * (float)(Universe.BIG_G * (attractor.mass / heading.sqrMagnitude));
    } 
}
