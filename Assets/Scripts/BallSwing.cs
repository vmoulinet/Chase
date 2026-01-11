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

    [Header("Cable Visuals")]
    public Material cableMaterial;
    public Color cableColor = Color.white;
    public float cableWidth = 0.02f;

    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.useWorldSpace = true;

        ApplyCableVisuals();
    }

    void ApplyCableVisuals()
    {
        lr.material = cableMaterial;
        lr.startColor = cableColor;
        lr.endColor = cableColor;
        lr.startWidth = cableWidth;
        lr.endWidth = cableWidth;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = new Vector3(x, -cableLength, 0f);
    }

    void LateUpdate()
    {
        lr.SetPosition(0, root.position);
        lr.SetPosition(1, transform.position);
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
