using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[CreateAssetMenu(fileName = "MoonShape", menuName = "Planets/MoonShape")]
public class MoonShape : PlanetShape
{
    public override Vector3 ModifyVertex(Vector3 v)
    {
        Vector3 center = Vector3.zero;


        float arc = CalcArcLength(v, CraterLocation.normalized, 1f);

        if(arc < RimWidth)
        {
            float offset = SmoothMax(SmoothMin(Mathf.Pow((arc - 1 - RimWidth), 2) * RimSteepness, arc * arc, SmoothVal), 1 - CraterDepth, SmoothVal);
            return v - v.normalized*offset*CraterSize;
        }

        return v;
    }

    public float SmoothVal = 0f;

    public Vector3 CraterLocation = new Vector3(0, 1, 0);
    public float CraterSize = 0.86f;
    public float CraterDepth = 0.9f;

    [Range(0, 0.007f)]
    public float RimWidth = 0.05f;
    public float RimSteepness = 0.01f;

    private float CalcArcLength(Vector3 a, Vector3 b, float radius)
    {
        float theta = (Mathf.Acos(Vector3.Dot(a, b) / a.magnitude / b.magnitude)) / 360f;
        return Mathf.Abs(2f * 3.14159f * radius * theta);
    }

    float SmoothMax(float a, float b, float k)
    {
        return Mathf.Max(a, b);
    }

    float SmoothMin(float a, float b, float k)
    {
        /*a = Mathf.Pow(a, k);
        b = Mathf.Pow(b, k);
        return Mathf.Pow((a * b) / (a + b), 1f / k);*/
        return Mathf.Min(a, b);
    }
}
