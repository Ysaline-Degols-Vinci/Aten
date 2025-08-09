using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDataBase : MonoBehaviour
{
    public static ShopDataBase Instance;

    private Dictionary<string, VendorData> vendors = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            VendorData[] allVendors = FindObjectsOfType<VendorData>();
            foreach (var vendor in allVendors)
            {
                vendors[vendor.vendorName] = vendor;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public VendorData GetVendor(string shopName)
    {
        vendors.TryGetValue(shopName, out var vendor);
        return vendor;
    }
}
