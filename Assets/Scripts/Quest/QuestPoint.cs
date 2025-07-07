using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestPoint : MonoBehaviour
{
    private bool PlayerIsNear = false;
    [SerializeField] private QuestInfo questInfo;
    private string questID;
    private QuestState currentQuestState;
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;
    Quest questDone;

    private void Awake()
    {
        questID = questInfo.id;
        questDone = QuestManager.instance.GetQuestByID(questID);
    }

    private void OnEnable()
    {
        if(EventManager.instance == null)
        {
            Debug.LogError("EventManager instance is null. Make sure it is initialized before QuestPoint.");
            return;
        }
        EventManager.instance.questEvents.onQuestStateChange += QuestChangeState;
    }

    private void OnDisable()
    {
        EventManager.instance.questEvents.onQuestStateChange -= QuestChangeState;
    }

    private void Update()
    {
        if (!PlayerIsNear) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("================CLICKING E NEAR QUEST=========");

            if (currentQuestState.Equals(QuestState.IN_PROGRESS))
            {
                QuestStep currentStep = questDone.currentSteps[questDone.currentQuestStepIndex];
                if (currentStep == null)
                {
                    Debug.LogError("Current step is null for quest: " + questInfo.id);
                    return;
                }
                else
                {
                    Debug.Log("Current step: " + currentStep.name + " for quest: " + questInfo.id + currentStep.conditionComplete);
                }
                Debug.Log("VALIDATING THE STEP?");
                currentStep.ValidateStep();
                Debug.Log("DONE VALIDATING");


                Debug.Log("SOMETHING DONE HERE??");
                Debug.Log("Current quest state: " + questDone.state);
                Debug.Log("one taken into account" + currentQuestState);

                if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
                {
                    EventManager.instance.questEvents.StartQuest(questInfo.id);
                    Debug.Log("Quest started: " + questInfo.id);
                }
                else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
                {
                    EventManager.instance.questEvents.FinishQuest(questInfo.id);
                    Debug.Log("Quest finished: " + questInfo.id);
                }

            }
        }

        //Cliquer e pour commencer la quete
        if (Input.GetKeyDown(KeyCode.E)){
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                EventManager.instance.questEvents.StartQuest(questInfo.id);
                Debug.Log("Quest started: " + questInfo.id);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                EventManager.instance.questEvents.FinishQuest(questInfo.id);
                Debug.Log("Quest finished: " + questInfo.id);
            } 
            }


    }

    private void QuestChangeState(Quest quest) { 
    if(quest.questInfo.id == questID)
        {
            currentQuestState = quest.state;
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerIsNear = true;            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerIsNear = false;
        }
    }
}
