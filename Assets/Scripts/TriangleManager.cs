using UnityEngine;
using System.Collections.Generic;

public class TriangleManager : MonoBehaviour
{
    public TriangleAgent[] agents;

    [Header("Global Triangle Parameters")]
    public float minDistance = 1.0f;
    public float maxDistance = 2.0f;
    public float maxSpeed = 2f;
    public float toleranceRadius = 1f;

    [Header("Spiral Mode")]
    public float spiralInterval = 30f;
    public float spiralDuration = 6f;
    public float spiralStrength = 2f;

    [HideInInspector] public bool spiralActive = false;
    [HideInInspector] public Vector3 spiralCenter;

    float intervalTimer = 0f;
    float spiralTimer = 0f;

    void Start()
    {
        if (agents == null || agents.Length < 3)
        {
            Debug.LogError("TriangleManager : il faut au moins 3 agents.");
            return;
        }

        AssignPairs();
    }

    void Update()
    {
        intervalTimer += Time.deltaTime;

        if (!spiralActive && intervalTimer >= spiralInterval)
            StartSpiral();

        if (spiralActive)
        {
            spiralTimer += Time.deltaTime;
            if (spiralTimer >= spiralDuration)
                StopSpiral();
        }
    }

    void StartSpiral()
    {
        Debug.Log("=== SPIRAL MODE ===");
        intervalTimer = 0f;
        spiralTimer = 0f;
        spiralActive = true;

        spiralCenter = Vector3.zero;
        foreach (var agent in agents)
            spiralCenter += agent.transform.position;

        spiralCenter /= agents.Length;
    }

    void StopSpiral()
    {
        Debug.Log("=== TRIANGLE MODE ===");
        spiralActive = false;
    }

    void AssignPairs()
    {
        foreach (TriangleAgent agent in agents)
        {
            List<TriangleAgent> others = new List<TriangleAgent>(agents);
            others.Remove(agent);

            TriangleAgent a = others[Random.Range(0, others.Count)];
            others.Remove(a);
            TriangleAgent b = others[Random.Range(0, others.Count)];

            agent.agentA = a.transform;
            agent.agentB = b.transform;
            agent.manager = this;
        }
    }
}
