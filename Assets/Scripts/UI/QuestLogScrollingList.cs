using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;

    [Header("QuestLogButton")]
    [SerializeField] private GameObject questLogButtonPrefab;

    private Dictionary<string, QuestLogButton> questButtons = new Dictionary<string, QuestLogButton>();

    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
    {
        QuestLogButton button = Instantiate(questLogButtonPrefab, contentParent.transform).GetComponent<QuestLogButton>();
        button.name = quest.questInfo.id + "_button";
        button.Initialize(selectAction, quest.questInfo.displayName);
        questButtons[quest.questInfo.id] = button;
        return button;
    }

    public QuestLogButton CreateButtonIfNotExist(Quest quest, UnityAction selectAction)
    {
        QuestLogButton button = null;

        if (!questButtons.ContainsKey(quest.questInfo.id))
        {
           button = InstantiateQuestLogButton(quest, selectAction);

        }
        else
        {
            button = questButtons[quest.questInfo.id];
        }


        return button;
    }

}
