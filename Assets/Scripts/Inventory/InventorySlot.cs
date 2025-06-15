using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Sprite selectedImage;
    public Sprite notSelectedImage;

    private void Awake()
    {
        deselect();
    }
    public void select()
    {
        image.sprite = selectedImage;
    }

    public void deselect()
    {
        image.sprite = notSelectedImage;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem item = dropped.GetComponent<InventoryItem>();
            item.parentAfterDrag = transform;
        }
    }
}
