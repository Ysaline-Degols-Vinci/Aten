using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    public InventorySlot[] inventorySlots;
    public InventorySlot[] activeInventorySlots;
    public GameObject InventoryItemPrefab;

    public GameObject InternInventory;
    public GameObject ShowInventory;
    public GameObject HideInventory;
    public Button SelectedOption;

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
        if(selectedSlot >= 0) activeInventorySlots[selectedSlot].deselect();
        selectedSlot = slotIndex;
        activeInventorySlots[selectedSlot].select();
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

        if (GameEventsManager.Instance.InputEventContext == InputEventContext.DIALOGUE) return;
        

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
                newSlot = (selectedSlot + 1) % activeInventorySlots.Length;
            }
            else if (scroll < 0f)
            {
                newSlot = (selectedSlot - 1 + activeInventorySlots.Length) % activeInventorySlots.Length;
            }

            changeSelectedSlot(newSlot);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Toggle la visibilité de l'inventaire
            if (InternInventory.activeSelf) {
                //on cache l'inventaire
                InternInventory.SetActive(false);
                ShowInventory.SetActive(true);
                HideInventory.SetActive(false);
                GameEventsManager.Instance.InputEventContext = InputEventContext.DEFAULT;

            }
            else
            {
                //on affiche l'inventaire
                InternInventory.SetActive(true);
                ShowInventory.SetActive(false);
                HideInventory.SetActive(true);
                SelectedOption.Select();
                GameEventsManager.Instance.InputEventContext = InputEventContext.INVENTORY;
            }
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
        InventorySlot slot = activeInventorySlots[selectedSlot];
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
