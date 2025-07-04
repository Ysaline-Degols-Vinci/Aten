using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectWoodQuestStep : QuestStep
{
    private int woodCollected = 0;
    private int woodToCollect = 1;

    
    private void OnEnable()
    {
        InventoryManager.Instance.onItemChanged += WoodCollected;
        StartCoroutine(DeferredInitialCheck());
    }

    private IEnumerator DeferredInitialCheck()
    {
        yield return null; // attend un frame
        WoodCollected();   // fait la vérification réelle
    }
    private void Update()
    {
        if (woodCollected >= woodToCollect)
        {
            CompleteStep();
        }
    }

    private void OnDisable()
    {
        InventoryManager.Instance.onItemChanged -= WoodCollected;
    }

    private void WoodCollected()
    {
        woodCollected = InventoryManager.Instance.ItemPresence("Wood", woodToCollect);
        Debug.Log("Wood collected: " + woodCollected);
        if (woodCollected >= woodToCollect)
        {
            CompleteStep();
        }
    }
}
