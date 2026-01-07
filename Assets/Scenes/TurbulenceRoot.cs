using UnityEngine;

public class TurbulenceRoot : MonoBehaviour
{
    public float posAmp = 0.2f;
    public float rotAmp = 30f;
    public float speed = 0.5f;

    Vector3 basePos;
    Quaternion baseRot;
    float seed;

    public void Init()
    {
        basePos = transform.position;
        baseRot = transform.rotation;
        seed = Random.value * 1000f;
    }

    void Update()
    {
        float t = Time.time * speed;

        Vector3 nPos = new Vector3(
            Mathf.PerlinNoise(seed, t) - 0.5f,
            Mathf.PerlinNoise(seed + 10, t) - 0.5f,
            Mathf.PerlinNoise(seed + 20, t) - 0.5f
        );

        Vector3 nRot = new Vector3(
            Mathf.PerlinNoise(seed + 30, t),
            Mathf.PerlinNoise(seed + 40, t),
            Mathf.PerlinNoise(seed + 50, t)
        );

        transform.position = basePos + nPos * posAmp;
        transform.rotation = baseRot * Quaternion.Euler((nRot - Vector3.one * 0.5f) * rotAmp);
    }
}
