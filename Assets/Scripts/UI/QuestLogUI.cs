using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI questDisplayNameText;
    [SerializeField] private TextMeshProUGUI questStatusText;
    [SerializeField] private TextMeshProUGUI questRewardsText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;
    [SerializeField] private QuestLogScrollingList scrollingList;

    private Button firstSelectedButton;


    private void SetQuestLogInfo(Quest quest)
    {
        questDisplayNameText.text = quest.questInfo.displayName;

        questDescriptionText.text = quest.questInfo.description;

        if(quest.state == QuestState.REQUIREMENTS_NOT_MET)
        {
            questStatusText.text = "Quest Status: Requirements Not Met";
        }
        else if (quest.state == QuestState.IN_PROGRESS)
        {
            questStatusText.text = "Quest Status: In Progress";
        }
        else if (quest.state == QuestState.CAN_START)
        {
            questStatusText.text = "Quest Status: Can start";
        }
        else if (quest.state == QuestState.FINISHED)
        {
            questStatusText.text = "Quest Status: Finished";
        }
        else
        {
            questStatusText.text = "Quest Status: Unknown";
        }





        questRewardsText.text = $"Gold Reward: {quest.questInfo.goldReward}\n" +
            $"Exp Reward: {quest.questInfo.expReward}";

    }

    private void QuestStateChange(Quest quest)
    {
        if(quest.state != QuestState.REQUIREMENTS_NOT_MET && quest.state != QuestState.CAN_START) {
            QuestLogButton button = scrollingList.CreateButtonIfNotExist(quest, () => { SetQuestLogInfo(quest); });

            if (firstSelectedButton == null)
            {
                firstSelectedButton = button.GetComponent<Button>();
                firstSelectedButton.Select();
            }
            button.SetState(quest.state);
        }
    }

    public void selectFirst()
    {
        if(firstSelectedButton!= null)
        {
            firstSelectedButton.Select();
        }
    }

    private void OnEnable()
    {

        StartCoroutine(WaitForGameEventsManager());
    }
    private IEnumerator WaitForGameEventsManager()
    {
        while (EventManager.instance == null || EventManager.instance.questEvents == null)
        {
            yield return null;
        }
        EventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }



        private void OnDisable()
    {
        EventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }
}
