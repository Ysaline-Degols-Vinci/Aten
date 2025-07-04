using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isCompleted = false;
    private string questID;
    public void initializeQuestStep(string questID)
    {
        this.questID = questID;
    }

    protected void CompleteStep()
    {
        if (!isCompleted)
        {
            isCompleted = true;
            EventManager.instance.questEvents.AdvanceQuest(questID);
            Destroy(this.gameObject);
        }
    }

}
