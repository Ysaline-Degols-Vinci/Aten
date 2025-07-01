using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item item;
    [HideInInspector] public int itemCount=1;
    public Text countText;
    public Image backgroundImage; // Optional: to change the background color or image of the item



    public void initializeItem(Item newItem) { 
        item= newItem;
        image.sprite = newItem.Image;
        itemCount = 1;
        refreshCount();
    }

    public void refreshCount()
    {
        countText.text = itemCount.ToString();
        bool textVisible = itemCount > 1;
        countText.gameObject.SetActive(textVisible);
        backgroundImage.gameObject.SetActive(textVisible);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false; 
        parentAfterDrag = transform.parent; 
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        Debug.Log("OnBeginDrag: " + item.name + " - Count: " + itemCount);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true; 
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;

    }
}
