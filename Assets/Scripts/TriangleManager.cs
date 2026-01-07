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

    void Start()
    {
        if (agents == null || agents.Length < 3)
        {
            Debug.LogError("TriangleManager : il faut au moins 3 agents.");
            return;
        }

        AssignPairs();
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
