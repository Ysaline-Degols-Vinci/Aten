using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager Instance { get; private set; }

    private bool dialoguePlaying = false;
    [SerializeField] private TextAsset inkJSON;
    private Story story;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private int currentChoiceIndex = -1;
    private string speaker;
    private string portrait;

    private InkExternalFunctions inkExternalFunctions;
    public InkDialogVariables inkDialogueVariables;

    public GameObject Inventory;

    private void Awake() {
        

        story = new Story(inkJSON.text);
        inkExternalFunctions = new InkExternalFunctions();
        inkExternalFunctions.Bind(story);
        inkDialogueVariables = new InkDialogVariables(story);
    }

    private void OnDestroy()
    {
        inkExternalFunctions.Unbind(story);
    }



    private void Update()
    {
        if(!dialoguePlaying) return;

        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            continueOrExitStory();
        }
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForGameEventsManager());
    }

    private IEnumerator WaitForGameEventsManager()
    {
        while (GameEventsManager.Instance == null || EventManager.instance == null)
        {
            yield return null; 
        }

        GameEventsManager.Instance.DialogueEvent.onEnterDialogue += EnterDialogue;
        GameEventsManager.Instance.DialogueEvent.onUpdateChoiceIndex += updateChoiceIndex;
        GameEventsManager.Instance.onMoneyChanged += OnMoneyChanged;
        GameEventsManager.Instance.DialogueEvent.onUpdateInkDialogueVariable += UpdateInkDialogueVariable;
        EventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        Instance = this;

    }

    private void OnDisable()
    {
        GameEventsManager.Instance.DialogueEvent.onEnterDialogue -= EnterDialogue;
        GameEventsManager.Instance.DialogueEvent.onUpdateChoiceIndex -= updateChoiceIndex;
        GameEventsManager.Instance.onMoneyChanged -= OnMoneyChanged;
        GameEventsManager.Instance.DialogueEvent.onUpdateInkDialogueVariable -= UpdateInkDialogueVariable;
        EventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;

    }
    private void EnterDialogue(string knotName)
    {
        if (dialoguePlaying) { 
            return;
        }
        else
        {
            Inventory.SetActive(false);
            GameEventsManager.Instance.DialogueEvent.DialogueStarted();
            dialoguePlaying = true;
            GameEventsManager.Instance.InputEventContext = InputEventContext.DIALOGUE;
        }
        if (!knotName.Equals(""))
        {

            story.ChoosePathString(knotName);
        }
        inkDialogueVariables.SyncVariablesAndStartListening(story);

        // continueOrExitStory();
    }

    private void continueOrExitStory()
    {
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            currentChoiceIndex = -1; // reset choice index after choosing
        }

        while (story.canContinue)
        {
            string text = story.Continue();

            if (!string.IsNullOrWhiteSpace(text))
            {
                HandleTags(story.currentTags);
                GameEventsManager.Instance.DialogueEvent.DisplayDialogue(text, speaker, portrait, story.currentChoices);
                return; // on sort dès qu'on a un texte valide à afficher
            }
            // Si texte vide, on continue la boucle pour sauter les passages vides
        }

        // Si on arrive ici, plus rien à afficher
        if (story.currentChoices.Count == 0)
        {
            exitDialogue();
        }
    }


    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            // handle the tag

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    speaker = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portrait = tagValue;
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void exitDialogue()
    {
        dialoguePlaying = false;
        Inventory.SetActive(true);
        GameEventsManager.Instance.InputEventContext = InputEventContext.DEFAULT;
        GameEventsManager.Instance.DialogueEvent.DialogueFinished();
        story.ResetState();
    }

    private void updateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    public void updateInk(int money) {
        story.variablesState["coins"] = money;
    }
    private void QuestStateChange(Quest quest)
    {
        Debug.Log("yeahhhhhhhhh");
        GameEventsManager.Instance.DialogueEvent.UpdateInkDialogueVariable(
            quest.questInfo.id + "State",
            new StringValue(quest.state.ToString())
        );
    }

    private void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        inkDialogueVariables.UpdateVariableState(name, value);
    }



    private void OnMoneyChanged(int newAmount)
    {
        GameEventsManager.Instance.DialogueEvent.UpdateInkDialogueVariable("coins", new Ink.Runtime.IntValue(newAmount));
    }








}
