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
        woodCollected = InventoryManager.Instance.ItemPresence("Wood", woodToCollect);
        conditionComplete = (woodCollected >= woodToCollect);

    }

    private void OnDisable()
    {
        InventoryManager.Instance.onItemChanged -= WoodCollected;
    }

    private void WoodCollected()
    {
        woodCollected = InventoryManager.Instance.ItemPresence("Wood", woodToCollect);
        conditionComplete = (woodCollected >= woodToCollect);

    }

    public override void removeElements()
    {
        InventoryManager.Instance.removeItem("Wood", woodToCollect);
    }
    public override void ValidateStep()
    {
        bool comp = CanBeCompleted();
        Debug.Log("Can be completedDDDDDDDDDDDD: " + comp);
        if (CanBeCompleted())
        {
            removeElements();
            CompleteStep();
        }
    }

    public override bool CanBeCompleted()
    {
        return conditionComplete;
    }


}
