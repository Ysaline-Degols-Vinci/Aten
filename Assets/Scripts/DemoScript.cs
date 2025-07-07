using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
   public InventoryManager inventoryManager;
    public Item[] itemsToPickUp;

public void pickUpItem(int id)
{

       bool result = inventoryManager.AddItem(itemsToPickUp[id]);

        if(result)
        {
        }
        else
        {
            Debug.Log("Inventory full, could not add item: " + itemsToPickUp[id].name);
        }
    }

}