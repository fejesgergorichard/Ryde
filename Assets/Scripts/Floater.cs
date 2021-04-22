using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private float angle = 0;
    private float sinValue;
    private int frequency = 0;
    private float initialYPosition;

    public float FrequencyModifier = 1.0f;
    public float AmplitudeModifier = 0.2f;
    public float RotationSpeed = 1.0f;
    public bool Rotate = false;


    void Start()
    {
        initialYPosition = transform.position.y;
    }

    void Update()
    {
        // Move the object up and down in a sine wave
        frequency++;
        angle = (float)(Mathf.PI * frequency * FrequencyModifier / 180.0);
        sinValue = Mathf.Sin(angle) * AmplitudeModifier;
        transform.position = new Vector3(transform.position.x, initialYPosition + sinValue, transform.position.z);

        // Rotate if asked
        if (Rotate)
        {
            transform.Rotate(0.0f, RotationSpeed, 0.0f);
        }
    }
}
