using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForEventManager());
    }

    private IEnumerator WaitForEventManager()
    {
        while (EventManager.instance == null)
        {
            yield return null;
        }
        EventManager.instance.questEvents.onStartQuest += StartQuest;
        EventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        EventManager.instance.questEvents.onFinishQuest += FinishQuest;
    }


    private void OnDisable()
    {
        EventManager.instance.questEvents.onStartQuest -= StartQuest;
        EventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        EventManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }

    private void Start()
    {
        foreach (Quest quest in questMap.Values)
        {
            EventManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfo[] allQuests = Resources.LoadAll<QuestInfo>("Quests");
        Dictionary<string, Quest> idToquestMap = new Dictionary<string, Quest>();

        foreach (QuestInfo questInfo in allQuests)
        {
            if (idToquestMap.ContainsKey(questInfo.id))
            {
                Debug.LogError($"Quest with ID {questInfo.id} already exists. Skipping duplicate quest.");
            }
            idToquestMap.Add(questInfo.id, new Quest(questInfo));   
        }
        return idToquestMap;
    }

    private Quest GetQuestByID(string id)
    {
        if (!questMap.ContainsKey(id))
        {
            Debug.LogError($"Quest with ID {id} does not exist in the quest map.");
            return null;
        }

        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError($"Quest with ID {id} not found.");
        }
        return quest;
    }

    private void StartQuest(string id)
    {
        Debug.Log($"Starting quest with ID: {id}");
        Quest quest = GetQuestByID(id);
        quest.InstantiateCurrentStep(this.transform);
        ChangeQuestState(id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
       Debug.Log($"Advancing quest with ID: {id}");
        Quest quest = GetQuestByID(id);
        quest.MoveToNextStep();

        if(quest.CurrentStepExists())
        {
           quest.InstantiateCurrentStep(this.transform);
        }
        else
        {
            Debug.Log($"Quest {id} has no more steps. Marking as can finishZZZZZZZZZZZZZ.");
            ChangeQuestState(id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Debug.Log($"Finishing quest with ID: {id}");
        Quest quest = GetQuestByID(id);
        ClaimRewards(quest);
        ChangeQuestState(id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
      MoneyManager.instance.ChangeMoney(quest.questInfo.goldReward);
      XPManager.Instance.AddExperience(quest.questInfo.expReward);
        //TODO gérer autres types de récompenses
        //Objets? xp?
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.state = state;
        EventManager.instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;
        foreach (QuestInfo prerequisiteInfo in quest.questInfo.questRequirements)
        {
            if(GetQuestByID(prerequisiteInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }
        return meetsRequirements;
    }

    private void Update()
    {
        while(DialogueManager.Instance == null )
        {
            // Wait for DialogueManager to be initialized
            return;
        }
        // Check if any quests can be started based on their requirements
        foreach (Quest quest in questMap.Values)
        {
            if(quest.state== QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.questInfo.id, QuestState.CAN_START);
                Debug.Log($"Quest {quest.questInfo.id} is now available to start.");
            }
        }
    }
}
