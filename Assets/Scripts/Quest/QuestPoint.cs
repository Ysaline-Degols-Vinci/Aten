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

    private void Awake()
    {
        questID = questInfo.id;
    }

    private void OnEnable()
    {
        if(EventManager.instance == null)
        {
            Debug.LogError("EventManager instance is null. Make sure it is initialized before QuestPoint. AAAAAAAAAAAAAAAAAA");
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
        //Cliquer e pour commencer la quete
        if (Input.GetKeyDown(KeyCode.E)){
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                EventManager.instance.questEvents.StartQuest(questInfo.id);
                Debug.Log("Quest started: " + questInfo.id);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
               // EventManager.instance.questEvents.FinishQuest(questInfo.id);
               // Debug.Log("Quest finished: " + questInfo.id);
               
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
