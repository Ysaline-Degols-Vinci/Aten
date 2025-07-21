using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    private TextMeshProUGUI buttonText;
    private UnityAction onSelectAction;

    public Button button
    {
        get; private set;
    }
    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction();
    }

    public void Initialize(UnityAction selectAction, string displayName)
    {
        this.button = this.GetComponent<Button>();
        this.onSelectAction = selectAction;
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();
        this.buttonText.text = displayName;
        
    }
    

    public void SetState(QuestState state)
    {
        switch (state)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
            case QuestState.CAN_START:
               // buttonText.color =Color.red;
                button.GetComponent<Image>().color = ColorFromRgba(255, 0, 0, 0.8f);
                break;
            case QuestState.IN_PROGRESS:
                //buttonText.color = new Color(0.929f, 0.6f, 0.157f, 0.8f);
                //buttonText.color = ColorFromRgba(181, 119, 0, 0.8f);
                button.GetComponent<Image>().color = ColorFromRgba(218, 180, 72, 0.8f); 
                break; 
            case QuestState.CAN_FINISH:
                //buttonText.color = new Color(0.5098f, 1f, 0.596f, 0.8f);
                //buttonText.color = ColorFromRgba(0, 159, 2, 0.8f);
                button.GetComponent<Image>().color = ColorFromRgba(0, 159, 2, 0.8f);
                break;
            case QuestState.FINISHED:
                //buttonText.color = new Color(0.5098f, 1f, 0.596f, 0.8f);
                button.GetComponent<Image>().color = ColorFromRgba(236, 196, 217, 0.8f);
                //buttonText.color = Color.gray;
                break;
            default: Debug.LogError("Invalid quest state for button: " + state); break;

        }
    }

    public static Color ColorFromRgba(int r, int g, int b, float a = 1f)
    {
        return new Color(r / 255f, g / 255f, b / 255f, Mathf.Clamp01(a));
    }

}
