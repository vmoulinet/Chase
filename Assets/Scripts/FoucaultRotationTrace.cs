using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class FoucaultBallTrail : MonoBehaviour
{
    [Header("Reference")]
    public Transform ball;

    [Header("Sampling")]
    public float minDistance = 0.05f;
    public int maxPoints = 5000;

    [Header("Line Visuals")]
    public Material lineMaterial;
    public Color lineColor = Color.cyan;
    public float lineWidth = 0.02f;

    LineRenderer lr;
    List<Vector3> points = new List<Vector3>();

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;

        ApplyLineVisuals();
        lr.positionCount = 0;
    }

    void ApplyLineVisuals()
    {
        if (lineMaterial) lr.material = lineMaterial;

        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
    }

    void Update()
    {
        Vector3 currentPos = ball.position;

        if (points.Count == 0 ||
            Vector3.Distance(points[points.Count - 1], currentPos) > minDistance)
        {
            points.Add(currentPos);

            if (points.Count > maxPoints)
                points.RemoveAt(0);

            lr.positionCount = points.Count;
            lr.SetPositions(points.ToArray());
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!lr) lr = GetComponent<LineRenderer>();
        ApplyLineVisuals();
    }
#endif
}
