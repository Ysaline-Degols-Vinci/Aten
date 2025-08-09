using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public TileBase Tile;
    public Sprite Image;
    public string Name;
    public itemType Type;
    public bool IsStackable;
    public int basePrice;
    public string Description;

    public int MaxStackSize;
}


public enum itemType
{
    Tool,
    Equipment,
    Consumable,
    QuestItem,
    Ressources
}


