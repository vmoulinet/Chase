using UnityEngine;

public class TriangleAgent : MonoBehaviour
{
    [Header("Triangle Targets")]
    public Transform agentA;
    public Transform agentB;

    [Header("References")]
    public TriangleManager manager;

    [Header("Movement")]
    public float mass = 1f;
    public float attractionStrength = 1.5f;
    public float damping = 0.99f;

    [Header("Noise")]
    public float noiseStrength = 0.1f;
    public float noiseFrequency = 1f;

    [Header("Rotation")]
    public float rotationSpeed = 8f;

    [Header("Visual Links")]
    public float lineWidth = 0.05f;

    Vector3 velocity;
    bool satisfied;

    LineRenderer lineToA;
    LineRenderer lineToB;

    void Start()
    {
        velocity = Random.insideUnitSphere * 0.5f;
        velocity.y = 0f;

        lineToA = CreateLineRenderer("LineToA");
        lineToB = CreateLineRenderer("LineToB");
    }

    void Update()
    {
        if (!agentA || !agentB || !manager)
            return;

        Vector3 desiredPosition = ComputeDesiredPosition();
        float distanceToTarget = Vector3.Distance(transform.position, desiredPosition);

        satisfied = (distanceToTarget <= manager.toleranceRadius && velocity.magnitude < 0.1f);

        if (!satisfied)
            ApplyMovement(desiredPosition);

        velocity *= damping;
        transform.position += velocity * Time.deltaTime;

        UpdateRotation();
        UpdateLines();
    }

    Vector3 ComputeDesiredPosition()
    {
        Vector3 posA = agentA.position;
        Vector3 posB = agentB.position;

        posA.y = transform.position.y;
        posB.y = transform.position.y;

        Vector3 mid = (posA + posB) * 0.5f;
        Vector3 axis = posB - posA;

        if (axis.sqrMagnitude < 0.0001f)
            return mid;

        Vector3 dir = axis.normalized;
        Vector3 normal = new Vector3(-dir.z, 0f, dir.x);

        Vector3 toMe = transform.position - mid;
        float signedDist = Vector3.Dot(toMe, normal);

        float absDist = Mathf.Clamp(
            Mathf.Abs(signedDist),
            manager.minDistance,
            manager.maxDistance
        );

        float finalDist = signedDist < 0f ? -absDist : absDist;
        return mid + normal * finalDist;
    }

    void ApplyMovement(Vector3 desiredPosition)
    {
        Vector3 force = (desiredPosition - transform.position) * attractionStrength;

        float noise = Mathf.PerlinNoise(Time.time * noiseFrequency, GetInstanceID()) - 0.5f;
        Vector3 noiseVector = new Vector3(noise, 0f, -noise) * noiseStrength;

        force += noiseVector;

        Vector3 acceleration = force / Mathf.Max(0.0001f, mass);
        velocity += acceleration * Time.deltaTime;

        velocity = Vector3.ClampMagnitude(velocity, manager.maxSpeed);
    }

    void UpdateRotation()
    {
        Vector3 dir = velocity;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

    LineRenderer CreateLineRenderer(string name)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = transform;

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.useWorldSpace = true;
        lr.material = new Material(Shader.Find("Sprites/Default"));

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.red, 0f),
                new GradientColorKey(Color.white, 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f)
            }
        );

        lr.colorGradient = gradient;
        return lr;
    }

    void UpdateLines()
    {
        lineToA.SetPosition(0, transform.position);
        lineToA.SetPosition(1, agentA.position);

        lineToB.SetPosition(0, transform.position);
        lineToB.SetPosition(1, agentB.position);
    }

    void OnDrawGizmos()
    {
        if (!agentA || !agentB || !manager) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(ComputeDesiredPosition(), manager.toleranceRadius);
    }
}
