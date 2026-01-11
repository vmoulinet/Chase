using UnityEngine;
using System;

public class FoucaultEarthRotation : MonoBehaviour
{
    [Range(-90f, 90f)]
    public float latitude = 48.8566f; // Paris par défaut

    void Update()
    {
        DateTime now = DateTime.UtcNow;
        float secondsToday =
            now.Hour * 3600f +
            now.Minute * 60f +
            now.Second +
            now.Millisecond / 1000f;

        float dayRatio = secondsToday / 86400f;

        float angle =
            360f * Mathf.Sin(latitude * Mathf.Deg2Rad) * dayRatio;

        transform.localRotation = Quaternion.Euler(0f, angle, 0f);
    }
}
