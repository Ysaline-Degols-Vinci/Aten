using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item itemSO;

    public TMP_Text nameText;
    public TMP_Text  priceText;
    public Image itemImage;
    public TMP_Text itemCountText;
    public int itemCount;
    public GameObject itemCountUi;
    public int inventoryIndex;

    private int price;

    public void Initialize(Item newItem, int price, int itemCount)
    {
        itemSO = newItem;
        this.price = price;
        this.itemCount = itemCount;

        if (itemSO != null)
        {
            nameText.text = itemSO.Name;
            priceText.text = price.ToString();
            itemImage.sprite = itemSO.Image;
            itemImage.enabled = true;

            if (itemCount > 0)
            {
                itemCountUi.SetActive(true);
                itemCountText.text = itemCount.ToString();
            }
            else
            {
                itemCountUi.SetActive(false);
                itemCountText.text = "";
            }
        }
        else
        {
            // Slot vide
            nameText.text = "";
            priceText.text = "";
            itemImage.sprite = null;
            itemImage.enabled = false;
            itemCountUi.SetActive(false);
            itemCountText.text = "";
        }
    }

    public void ClearSlot()
    {
        itemSO = null;
        price = 0;
        itemCount = 0;

        nameText.text = "";
        priceText.text = "";
        itemImage.sprite = null;
        itemImage.enabled = false;

        itemCountUi.SetActive(false);
        itemCountText.text = "";
    }


    public void OnBuyClicked()
    {
        if (itemSO == null)
        {
            Debug.LogWarning("Item is not initialized in ShopSlot.");
            return;
        }
        Debug.Log("Attempting to buy: " + itemSO.Name + " for " + price + " coins.");
        ShopManager.instance.TryBuy(itemSO, price, 1);
    }

   
    public void OnSellClicked()
    {
        Debug.Log("OnSellClicked from object: " + gameObject.name);

        if (itemSO == null)
        {
            Debug.LogWarning("Item is not initialized in ShopSlot.");
            return;
        }
        Debug.Log("Attempting to sell: " + itemSO.Name + " for " + price + " coins.");
        ShopManager.instance.Sell(itemSO, price, 1, inventoryIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSO != null) {
            ItemTooltipController.Instance.ShowTooltip(itemSO.Name, itemSO.Description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {  
            ItemTooltipController.Instance.HideTooltip();
    }

}
