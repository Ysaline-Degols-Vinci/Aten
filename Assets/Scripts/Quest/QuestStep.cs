using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    public bool isCompleted = false;
    public bool conditionComplete=false;
    private string questID;
    

    public QuestStep initializeQuestStep(string questID)
    {
        this.questID = questID;
        return this;
    }

    protected void CompleteStep()
    {
        Debug.Log("CompleteStep called for: " + name + " for questEEEEE: " + questID);
        if (!isCompleted)
        {
            isCompleted = true;
            EventManager.instance.questEvents.AdvanceQuest(questID);
            gameObject.SetActive(false); // on ne détruit pas
            Debug.Log("Quest step completed: " + name + " for questAAAAAAAAAAAAA: " + questID);
        }
    }

    public virtual bool CanBeCompleted()
    {
        return conditionComplete;
    }

    public virtual void ValidateStep()
    {
        bool comp = CanBeCompleted();
        Debug.Log("CAN BE COMPLETED? " + comp + " for step: " + name + " for quest: " + questID);
        if (CanBeCompleted())
        {
            CompleteStep();
        }
    }


    public virtual void removeElements()
    {

    }

}
