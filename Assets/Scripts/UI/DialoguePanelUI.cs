using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DialoguePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private UnityEngine.UI.Image portraitImage;
    [SerializeField] private DialogueChoiceButton[] choiceButtons;



    private void Awake()
    {
        contentParent.SetActive(false);

        ResetPanel();
    }

    private void DialogueStarted()
    {
        contentParent.SetActive(true);
    }

    private void DialogueFinished()
    {
        contentParent.SetActive(false);
        ResetPanel();   
    }

    private void DisplayDialogue(string text, string speaker, string portrait, List<Choice> dialogueChoices)
    {
        speakerText.alignment = TextAlignmentOptions.Center;

        dialogueText.text = text;
        speakerText.text = speaker;

        // Charge le portrait depuis le dossier Resources/Portraits
        Sprite loadedPortrait = Resources.Load<Sprite>($"Portraits/{portrait}");
        if (loadedPortrait != null)
        {
            portraitImage.sprite = loadedPortrait;
        }
        else
        {
            Debug.LogWarning($"Portrait sprite not found for: {portrait}");
            portraitImage.sprite = null; // ou un sprite par défaut
        }

        foreach (DialogueChoiceButton button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
        int choiceButtonIndex = dialogueChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = choiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.setChoiceText(dialogueChoice.text);
            choiceButton.setChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
            {
                choiceButton.selectButton();
                GameEventsManager.Instance.DialogueEvent.UpdateChoiceIndex(inkChoiceIndex);
            }

            choiceButtonIndex--;
        }


    }

    private void ResetPanel()
    {
        dialogueText.text = "";
        speakerText.text = "";
    }

    private void OnEnable()
    {
        // Subscribe to the dialogue events
        // This is where we will receive the events from the DialogueEvent class
        GameEventsManager.Instance.DialogueEvent.onDialogueStarted += DialogueStarted;
        GameEventsManager.Instance.DialogueEvent.onDialogueFinished += DialogueFinished;
        GameEventsManager.Instance.DialogueEvent.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.DialogueEvent.onDialogueStarted -= DialogueStarted;
        GameEventsManager.Instance.DialogueEvent.onDialogueFinished -= DialogueFinished;
        GameEventsManager.Instance.DialogueEvent.onDisplayDialogue -= DisplayDialogue;
    }
}
