using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestInfo questInfo;
    public QuestState state;
    public int currentQuestStepIndex;
    public List<QuestStep> currentSteps = new List<QuestStep>();

    public Quest(QuestInfo questInfo)
    {
        this.questInfo = questInfo;
        state = QuestState.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        if (!CurrentStepExists()) return;

        QuestStep currentStep = currentSteps.Count > currentQuestStepIndex ? currentSteps[currentQuestStepIndex] : null;

        if (currentStep != null && currentStep.isCompleted)
        {
            // remove or handle logic related to this step

            // destroy the actual instantiated object, not the prefab!
            Object.Destroy(currentStep.gameObject);

            // go to next step
            currentQuestStepIndex++;
        }
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < questInfo.questStepPrefab.Length);
    }

    public void InstantiateCurrentStep(Transform parent)
    {
        Debug.Log("Instantiating current step for quest: " + questInfo.id + " at index: " + currentQuestStepIndex);
        GameObject questPrefab = GetCurrentQuestStepPrefab();
        if (questPrefab != null)
        {
            GameObject instance = Object.Instantiate<GameObject>(questPrefab, parent);
            QuestStep questStep = instance.GetComponent<QuestStep>();
            QuestStep newStep = questStep.initializeQuestStep(questInfo.id);
            currentSteps.Add(newStep);
        }
        else
        {
            Debug.LogError("No current quest step prefab found for quest: " + questInfo.id + " at index: " + currentQuestStepIndex);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        if (CurrentStepExists())
        {
            return questInfo.questStepPrefab[currentQuestStepIndex];
        }
        return null;
    }
}
