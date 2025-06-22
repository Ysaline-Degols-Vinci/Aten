using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    public InteractableObject data;
    private int currentHealth;

    private void Start() => currentHealth = data.maxHealth;

    public void Hit(GameObject attacker)
    {
        Item selectedItem = InventoryManager.Instance.getSelectedItem(false);

        if (data.requiredTool != null && selectedItem != data.requiredTool)
        {
            Debug.Log("Outil incorrect pour interagir avec cet objet.");
            return;
        }

        currentHealth--;
        if (currentHealth <= 0)
        {
            DropResources();
            Destroy(transform.root.gameObject);
        }
    }

    void DropResources()
    {
        foreach (var drop in data.drops)
        {
            for (int i = 0; i < drop.amount; i++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));
                GameObject dropObj = Instantiate(drop.dropPrefab, transform.position + randomOffset, Quaternion.identity);
                dropObj.GetComponent<ResourcePickup>().Initialize(drop.itemData);
            }
        }
    }
}