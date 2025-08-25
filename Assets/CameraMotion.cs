using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    public float speedY = 2f;       // Tốc độ dao động theo trục Y
    public float amplitudeY = 0.5f; // Biên độ dao động Y

    public float speedX = 1.5f;     // Tốc độ dao động theo trục X
    public float amplitudeX = 0.5f; // Biên độ dao động X

    private Vector3 startPos; // Vị trí ban đầu

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speedY) * amplitudeY;
        float offsetX = Mathf.Cos(Time.time * speedX) * amplitudeX;

        transform.position = new Vector3(
            startPos.x + offsetX,
            startPos.y + offsetY,
            startPos.z
        );
    }
}
