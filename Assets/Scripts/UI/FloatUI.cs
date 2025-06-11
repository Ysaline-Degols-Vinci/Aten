using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUI : MonoBehaviour
{
    public float floatAmplitude = 0.5f;  // Hauteur du flottement
    public float floatFrequency = 1f;    // Vitesse du flottement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // On fait varier la position Y avec une sinusoïde
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
