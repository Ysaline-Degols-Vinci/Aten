using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = 0.3f;

    private Vector3 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Camera.main == null)
            return;

        Vector2 mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 offsetFromCenter = mouseViewportPos - new Vector2(0.5f, 0.5f);

        Vector3 targetPosition = startPosition + new Vector3(offsetFromCenter.x, offsetFromCenter.y, 0f) * offsetMultiplier;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
