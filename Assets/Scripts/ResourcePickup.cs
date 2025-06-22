using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    public float magnetDistance = 3f;
    public float moveSpeed = 5f;

    public float floatAmplitude = 0.25f;
    public float floatSpeed = 2f;
    public float heightOffset = 0.5f;

    private Item resourceItem;
    private Transform player;
    private Vector3 floatOrigin;

    void Start()
    {
        // Positionne juste au-dessus du sol
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 20, Vector3.down, out hit, 0.5f))
        {
            transform.position = hit.point + Vector3.up * heightOffset;
        }

        floatOrigin = transform.position;
    }

    public void Initialize(Item item)
    {
        resourceItem = item;
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < magnetDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            if (distance < 1f)
            {
                InventoryManager.Instance.AddItem(resourceItem);
                Destroy(gameObject);
            }
        }
        else
        {
            // Flottement uniquement quand pas attiré
            float newY = floatOrigin.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(floatOrigin.x, newY, floatOrigin.z);
        }
    }
}
