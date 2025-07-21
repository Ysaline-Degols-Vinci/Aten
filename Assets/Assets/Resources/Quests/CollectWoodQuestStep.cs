using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectWoodQuestStep : QuestStep
{
    private int woodToCollect = 1;

    private void OnEnable()
    {
        InventoryManager.Instance.onItemChanged += CheckProgress;
        StartCoroutine(DeferredInitialCheck());
    }

    private void OnDisable()
    {
        InventoryManager.Instance.onItemChanged -= CheckProgress;
    }

    private IEnumerator DeferredInitialCheck()
    {
        yield return null;
        CheckProgress();
    }

    public void CheckProgress()
    {
        int woodCollected = InventoryManager.Instance.ItemPresence("Wood");
        if (woodCollected >= woodToCollect)
        {
            CompleteStep();
        }
    }

    public override void RemoveRessources()
    {
        InventoryManager.Instance.removeItem("Wood", woodToCollect);
    }
}
