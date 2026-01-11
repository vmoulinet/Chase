using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FoucaultPendulumBall : MonoBehaviour
{
    [Header("References")]
    public Transform root;

    [Header("Swing")]
    public float amplitude = 5f;
    public float speed = 1f;
    public float cableLength = 10f;

    [Header("Time")]
    public float swingTimeScale = 1f;

    [Header("Cable Visuals")]
    public Material cableMaterial;
    public Color cableColor = Color.white;
    public float cableWidth = 0.02f;

    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.positionCount = 2;

        ApplyCableVisuals();
        lr.enabled = false; // IMPORTANT : pas de preview en Editor
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            lr.enabled = false;
            return;
        }

        lr.enabled = true;

        float t = Time.time * swingTimeScale;
        float x = Mathf.Sin(t * speed) * amplitude;

        transform.localPosition =
            new Vector3(x, -cableLength, 0f);
    }

    void LateUpdate()
    {
        if (!Application.isPlaying) return;

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
    }
#endif

    void OnCollisionEnter(Collision c)
    {
        Destroy(c.gameObject);
    }
}
