using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Object Data")]

public class InteractableObject : ScriptableObject
{
    public string objectName;
    public int maxHealth;
    public List<ResourceDrop> drops;
    public Item requiredTool;
}
