using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class FoucaultPendulumBall : MonoBehaviour
{
    [Header("References")]
    public Transform root;

    [Header("Swing")]
    public float amplitude = 7f;
    public float speed = 1f;
    public float cableLength = 48.5f;
    public float swingTimeScale = 1f;

    [Header("Cable")]
    public bool enableCable = true;
    public Material cableMaterial;
    public Color cableColor = Color.white;
    public float cableWidth = 0.02f;

    LineRenderer lr;
    Rigidbody rb;

    float phase;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();

        // 🔒 Rigidbody = collision only
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;

        // Line
        lr.useWorldSpace = true;
        lr.positionCount = 2;
        ApplyCableVisuals();
        lr.enabled = false;
    }

    void Update()
    {
        phase += Time.deltaTime * speed * swingTimeScale;

        float x = Mathf.Sin(phase) * amplitude;

        Vector3 worldPos =
            root.position +
            root.right * x +
            root.up * (-cableLength);

        transform.position = worldPos;
    }

    void LateUpdate()
    {
        if (!Application.isPlaying)
        {
            lr.enabled = false;
            return;
        }

        lr.enabled = enableCable;
        if (!enableCable) return;

        lr.SetPosition(0, root.position);
        lr.SetPosition(1, transform.position);
    }

    void ApplyCableVisuals()
    {
        if (cableMaterial)
            lr.material = cableMaterial;

        lr.startColor = cableColor;
        lr.endColor = cableColor;
        lr.startWidth = cableWidth;
        lr.endWidth = cableWidth;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!lr) lr = GetComponent<LineRenderer>();
        ApplyCableVisuals();
        if (!Application.isPlaying)
            lr.enabled = false;
    }
#endif
}
