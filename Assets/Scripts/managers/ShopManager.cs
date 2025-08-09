using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopSlot[] shopSlots;
    public static ShopManager instance { get; private set; }
    public GameObject shopUi;
    private string currentVendorInkNode;
    public TextMeshProUGUI shopTitleText;

    [SerializeField] private ShopSlot[] shopSlotsSell;
    public GameObject sellUi;



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("ShopManager instance already exists.");
        }
        instance = this;
    }


    public void OpenShopWithItems(List<ShopItems> items, string vendorInkNode, string shopName)
    {
        GameEventsManager.Instance.InputEventContext = InputEventContext.SHOP;
        InventoryManager.Instance.hideExternInventory();

        currentVendorInkNode = vendorInkNode;
        shopUi.SetActive(true);
        shopTitleText.text = shopName;
        for (int i = 0; i < items.Count && i < shopSlots.Length; i++)
        {
            var item = items[i];
            shopSlots[i].Initialize(item.itemSo, item.price, 0);//TODO a voir si on met une quantité
            shopSlots[i].gameObject.SetActive(true);
        }

        for (int i = items.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }

        DisplayPlayerShop();
    }

    public void DisplayPlayerShop()
    {
        sellUi.SetActive(true);

        var inventorySlots = InventoryManager.Instance.inventorySlots;


        for (int i = 0; i < shopSlotsSell.Length; i++)
        {
            // Récupère le slot du joueur
            InventorySlot playerSlot = inventorySlots[i];
            InventoryItem inventoryItem = playerSlot.GetComponentInChildren<InventoryItem>();

            if (inventoryItem != null && inventoryItem.itemCount!=0)
            {
                int sellPrice = inventoryItem.item.basePrice / 4 * 3;
                sellPrice = Mathf.Max(sellPrice, 1); 
                sellPrice = Mathf.RoundToInt(sellPrice);

                shopSlotsSell[i].Initialize(inventoryItem.item, sellPrice, inventoryItem.itemCount);
                shopSlotsSell[i].inventoryIndex = i;
            }
            else
            {
                shopSlotsSell[i].ClearSlot();
            }

            shopSlotsSell[i].gameObject.SetActive(true);
        }

    }

    public void CloseShop()
    {
        shopUi.SetActive(false);
        sellUi.SetActive(false);
        if (!string.IsNullOrEmpty(currentVendorInkNode))
        {
            GameEventsManager.Instance.InputEventContext = InputEventContext.DEFAULT;
            GameEventsManager.Instance.DialogueEvent.EnterDialogie(currentVendorInkNode, true, true);
        }
    }

    public void TryBuy(Item item, int price, int quantity)
    {
        int totalPrice = price * quantity;

        if(MoneyManager.instance.GetMoney()<totalPrice) return;

        if (InventoryManager.Instance.AddItem(item, quantity))
        {
            MoneyManager.instance.ChangeMoney(-totalPrice);
            Debug.Log($"Bought {quantity} of {item.Name} for {totalPrice} coins.");
            DisplayPlayerShop();
        }

    }

    public void Sell(Item item, int price, int quantity, int inventoryIndex)
    {
        int totalPrice = price * quantity;
        InventoryManager.Instance.removeItem(inventoryIndex, quantity);
        MoneyManager.instance.ChangeMoney(totalPrice);
        DisplayPlayerShop();
    }
}

