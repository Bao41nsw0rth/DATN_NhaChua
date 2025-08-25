using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light targetLight;
    public float minInterval = 0.05f; // Thời gian tắt/bật nhanh nhất
    public float maxInterval = 0.3f;  // Thời gian tắt/bật chậm nhất

    private float timer;

    void Start()
    {
        SetRandomTime();
    }

    void Update()
    {
        if (targetLight == null) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            targetLight.enabled = !targetLight.enabled; // Đảo trạng thái
            SetRandomTime();
        }
    }

    void SetRandomTime()
    {
        timer = Random.Range(minInterval, maxInterval);
    }
}
