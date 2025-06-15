using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    public InventorySlot[] inventorySlots;
    public GameObject InventoryItemPrefab;

    public int maxStackItems = 4;

    int selectedSlot = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        changeSelectedSlot(0);
    }
    public void changeSelectedSlot(int slotIndex)
    {
        if(selectedSlot >= 0) inventorySlots[selectedSlot].deselect();
        selectedSlot = slotIndex;
        inventorySlots[selectedSlot].select();
    }

    private void Update()
    {
        /*  if (Input.inputString != null)
          {
              bool isNumber = int.TryParse(Input.inputString, out int number);
              if (isNumber && number >= 1 && number <= inventorySlots.Length)
              {
                  changeSelectedSlot(number - 1);
              }
          }*/
        if (Input.inputString != null)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
                {
                    changeSelectedSlot(i);
                }
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        int newSlot = -1;
        if (scroll != 0f)
        {
            if (scroll > 0f)
            {
                newSlot = (selectedSlot + 1) % inventorySlots.Length;
            }
            else if (scroll < 0f)
            {
                newSlot = (selectedSlot - 1 + inventorySlots.Length) % inventorySlots.Length;
            }

            changeSelectedSlot(newSlot);
        }
    }


    public bool AddItem(Item item)
    {

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

          
            maxStackItems = item.MaxStackSize;
            if (inventoryItem != null && inventoryItem.item == item && inventoryItem.itemCount < maxStackItems  && inventoryItem.item.IsStackable)
            {
                inventoryItem.itemCount++;
                inventoryItem.refreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

            if(inventoryItem == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    } 

    public void SpawnNewItem(Item item, InventorySlot slot) { 
        GameObject newItem = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.initializeItem(item);
    }

    public Item getSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
        if (inventoryItem != null)
        {
            Item item =  inventoryItem.item;

            if(use == true)
            {
                inventoryItem.itemCount--;

                if (inventoryItem.itemCount <= 0)
                {
                    Destroy(inventoryItem.gameObject);
                }
                else
                {
                    inventoryItem.refreshCount();

                }
            }



            return item;
        }
        return null;
    }
}
