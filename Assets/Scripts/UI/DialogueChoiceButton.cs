using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour, ISelectHandler
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;
    private int choiceIndex = -1;

    public void setChoiceText(string choiceText)
    {
        text.text = choiceText;
    }

    public void setChoiceIndex(int index)
    {
        this.choiceIndex = index;
    }

    public void selectButton()
    {
        button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameEventsManager.Instance.DialogueEvent.UpdateChoiceIndex(choiceIndex);
    }
}
