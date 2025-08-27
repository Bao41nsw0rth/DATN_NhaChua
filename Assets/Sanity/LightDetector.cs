using UnityEngine;

public class LightDetector : MonoBehaviour
{
    public float lightThreshold = 10000f;
    private SanityManager playerEffects;

    void Start()
    {
        playerEffects = GetComponent<SanityManager>();
    }

    void Update()
    {
        float totalLight = CalculateLightAtPosition(transform.position);

        if (totalLight < lightThreshold)
        {
            playerEffects.SetLoseSanity(true);
        }
        else
        {
            playerEffects.SetLoseSanity(false);
        }
    }

    float CalculateLightAtPosition(Vector3 pos)
    {
        Light[] lights = FindObjectsOfType<Light>();
        float total = 0f;

        foreach (Light l in lights)
        {
            if (!l.enabled) continue;

            switch (l.type)
            {
                case LightType.Point:
                    float dist = Vector3.Distance(pos, l.transform.position);
                    if (dist < l.range)
                    {
                        total += l.intensity * (1f - dist / l.range);
                    }

                    break;

                case LightType.Spot:
                    Vector3 dirToPlayer = (pos - l.transform.position).normalized;
                    float angle = Vector3.Angle(l.transform.forward, dirToPlayer);

                    if (angle < l.spotAngle / 2f)
                    {
                        float spotDist = Vector3.Distance(pos, l.transform.position);
                        if (spotDist < l.range)
                        {
                            total += l.intensity * (1f - spotDist / l.range);
                        }
                    }

                    break;

                case LightType.Directional:
                    total += l.intensity; // ánh sáng hướng không phụ thuộc khoảng cách
                    break;
            }
        }

        return total;
    }
}