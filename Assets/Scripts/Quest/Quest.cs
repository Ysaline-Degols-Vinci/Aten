using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest 
{
   public QuestInfo questInfo;

    public QuestState state;
    private int currentQuestStepIndex;
    private GameObject currentStepInstance;

    public Quest(QuestInfo questInfo)
    {
        this.questInfo = questInfo;
        state = QuestState.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        if (currentStepInstance != null)
        {
            Object.Destroy(currentStepInstance);
            currentStepInstance = null;
        }

        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < questInfo.questStepPrefab.Length);
    }

    public void InstantiateCurrentStep(Transform parent)
    {
        GameObject questPrefab = GetCurrentQuestStepPrefab();
        if (questPrefab != null)
        {
            currentStepInstance = Object.Instantiate<GameObject>(questPrefab, parent);
            QuestStep questStep = currentStepInstance.GetComponent<QuestStep>();
            questStep.initializeQuestStep(questInfo.id);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject prefab = null;

        if (CurrentStepExists())
        {
            prefab = questInfo.questStepPrefab[currentQuestStepIndex];
        }
        return prefab;
    }
    }
