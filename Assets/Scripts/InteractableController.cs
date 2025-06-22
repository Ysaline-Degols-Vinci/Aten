using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    private InteractableObjects currentTarget;

    void Update()
    {
        if (currentTarget != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
           
                currentTarget.Hit(gameObject);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out InteractableObjects interactable))
        {
            currentTarget = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out InteractableObjects interactable) && currentTarget == interactable)
        {
            currentTarget = null;
        }
    }
}
