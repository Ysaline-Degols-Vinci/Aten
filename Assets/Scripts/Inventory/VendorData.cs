using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorData : MonoBehaviour
{
    public string vendorName;
    [SerializeField] public List<ShopItems> shopItems;
    public string inkKnotBackToDiscussion;
}

[System.Serializable]
public class ShopItems
{
    public Item itemSo;
    public int price;
}
