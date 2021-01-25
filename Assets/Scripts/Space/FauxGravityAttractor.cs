using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{

    public float surfaceAcceleration = 10f;
    public float surfaceRadius = 100f;

    public float rotationSpeed = 1f;

    public float mass {get {
        return (surfaceAcceleration * surfaceRadius) / (float)Universe.BIG_G;
    }}

    private void Awake() {
        Universe.Instance.FauxGravityAttractors.Add(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);
        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, surfaceRadius);
    }
}
