using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatDroppedItem : MonoBehaviour
{
    public float floatAmplitude = 0.25f;
    public float floatSpeed = 2f;
    public float heightOffset = 0.5f;

    private Vector3 startPos;

    void Start()
    {
        RaycastHit hit;

        // Lance un raycast vers le bas pour positionner l'objet juste au-dessus du sol
        if (Physics.Raycast(transform.position + Vector3.up * 2, Vector3.down, out hit, 5f))
        {
            transform.position = hit.point + Vector3.up * heightOffset;
        }

        startPos = transform.position;
    }

    void Update()
    {
        // Mouvement de flottement vertical
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
