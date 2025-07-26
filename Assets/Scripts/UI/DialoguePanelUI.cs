using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DialoguePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject contentParentDialogue;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private UnityEngine.UI.Image portraitImageDialogue;
    [SerializeField] private DialogueChoiceButton[] choiceButtonsDialogues;

    [SerializeField] private GameObject contentParentShop;
    [SerializeField] private TextMeshProUGUI shopDialogueText;
    [SerializeField] private TextMeshProUGUI shopSpeakerText;
    [SerializeField] private UnityEngine.UI.Image shopPortraitImageDialogue;
    [SerializeField] private DialogueChoiceButton[] choiceButtonsShop;

    [SerializeField] private DialogueChoiceButton choiceButtonPrefab; 
    [SerializeField] private Transform choiceButtonContainerShop;
    private List<DialogueChoiceButton> currentShopButtons = new List<DialogueChoiceButton>();

    private void Awake()
    {
        contentParentDialogue.SetActive(false);
        contentParentShop.SetActive(false);

        ResetPanel();
    }

    private void DialogueStarted()
    {
        if(GameEventsManager.Instance.InputEventContext == InputEventContext.SHOPDIALOGUE)
        {
            contentParentShop.SetActive(true);
            contentParentDialogue.SetActive(false);
        }
        else if (GameEventsManager.Instance.InputEventContext == InputEventContext.DIALOGUE)
        {
            contentParentShop.SetActive(false);
            contentParentDialogue.SetActive(true);
        }
    }

    private void DialogueFinished()
    {
        contentParentDialogue.SetActive(false);
        contentParentShop.SetActive(false);
        ResetPanel();

        foreach (var btn in currentShopButtons)
        {
            Destroy(btn.gameObject);
        }
        currentShopButtons.Clear();
    }

    private void DisplayDialogue(string text, string speaker, string portrait, List<Choice> dialogueChoices)
    {
        if(GameEventsManager.Instance.InputEventContext == InputEventContext.SHOPDIALOGUE)
        {
            DisplayShopDialogue(text, speaker, portrait, dialogueChoices);
        }
        else
        {
            DisplayDialoguePanel(text, speaker, portrait, dialogueChoices);
        }
        

    }

    private void DisplayShopDialogue(string text, string speaker, string portrait, List<Choice> dialogueChoices)
    {
        contentParentShop.SetActive(true);
        shopSpeakerText.alignment = TextAlignmentOptions.Center;

        shopDialogueText.text = text;
        shopSpeakerText.text = speaker;

        // Charge le portrait depuis le dossier Resources/Portraits
        Sprite loadedPortrait = Resources.Load<Sprite>($"Portraits/{portrait}");
        if (loadedPortrait != null)
        {
            shopPortraitImageDialogue.sprite = loadedPortrait;
        }
        else
        {
            Debug.LogWarning($"Portrait sprite not found for: {portrait}");
            shopPortraitImageDialogue.sprite = null; // ou un sprite par défaut
        }

        // Supprime les anciens boutons de choix s'ils existent
        foreach (var btn in currentShopButtons)
        {
            Destroy(btn.gameObject);
        }
        currentShopButtons.Clear();

        // Crée dynamiquement les nouveaux boutons dans le conteneur
        for (int i = 0; i < dialogueChoices.Count; i++)
        {
            Choice dialogueChoice = dialogueChoices[i];
            DialogueChoiceButton newButton = Instantiate(choiceButtonPrefab, choiceButtonContainerShop);

            newButton.setChoiceText(dialogueChoice.text);
            newButton.setChoiceIndex(i);

            if (i == 0)
            {
                newButton.selectButton();
                GameEventsManager.Instance.DialogueEvent.UpdateChoiceIndex(i);
            }

            currentShopButtons.Add(newButton);
        }

    }

    private void DisplayDialoguePanel(string text, string speaker, string portrait, List<Choice> dialogueChoices)
    {
        contentParentDialogue.SetActive(true);
        speakerText.alignment = TextAlignmentOptions.Center;

        dialogueText.text = text;
        speakerText.text = speaker;

        // Charge le portrait depuis le dossier Resources/Portraits
        Sprite loadedPortrait = Resources.Load<Sprite>($"Portraits/{portrait}");
        if (loadedPortrait != null)
        {
            portraitImageDialogue.sprite = loadedPortrait;
        }
        else
        {
            Debug.LogWarning($"Portrait sprite not found for: {portrait}");
            portraitImageDialogue.sprite = null; // ou un sprite par défaut
        }

        foreach (DialogueChoiceButton button in choiceButtonsDialogues)
        {
            button.gameObject.SetActive(false);
        }
        int choiceButtonIndex = dialogueChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = choiceButtonsDialogues[choiceButtonIndex];

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
