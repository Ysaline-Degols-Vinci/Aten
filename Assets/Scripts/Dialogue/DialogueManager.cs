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

    public GameObject Inventory;

    private Dictionary<string, object> inkVariables = new Dictionary<string, object>()
{
    { "coins", 0 },
    { "TrashCoin", false },
    {"TossedCoinWell", false }
};



    private void Awake() {
        

        story = new Story(inkJSON.text);
        Bind(story);
    }

    private void OnDestroy()
    {
        Unbind(story);
    }

    private void Bind(Story story) {
        story.BindExternalFunction("gainCoin", (int amount) => {
            MoneyManager.instance.ChangeMoney(amount);
            UpdateVariable("coins", MoneyManager.instance.GetMoney());
        });

        story.BindExternalFunction("setTrashCoin", (bool value) => {
            UpdateVariable("TrashCoin", value);
        });

        story.BindExternalFunction("setTossedCoinWell", (bool value) => {
            UpdateVariable("TossedCoinWell", value);
        });

        story.BindExternalFunction("StartQuest", (string questID) => StartQuest(questID));
        story.BindExternalFunction("AdvanceQuest", (string questID) => AdvanceQuest(questID));
        story.BindExternalFunction("FinishQuest", (string questID) => FinishQuest(questID));
        story.BindExternalFunction("AddItemInventory", (string item) => AddToInventory(item));


        SyncVariablesToInk();
    }

    private void Unbind(Story story)
    {
        story.UnbindExternalFunction("gainCoin");
        story.UnbindExternalFunction("setTrashCoin");
        story.UnbindExternalFunction("setTossedCoinWell");
        story.UnbindExternalFunction("StartQuest");
        story.UnbindExternalFunction("AdvanceQuest");
        story.UnbindExternalFunction("FinishQuest");
        story.UnbindExternalFunction("AddItemInventory");
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
        Debug.Log("DialogueManager: GameEventsManager and EventManager are ready.");
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
            SyncAllVariablesToInk();
            GameEventsManager.Instance.DialogueEvent.DialogueStarted();
            dialoguePlaying = true;
            GameEventsManager.Instance.InputEventContext = InputEventContext.DIALOGUE;
        }
        if (!knotName.Equals(""))
        {

            story.ChoosePathString(knotName);
        }

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
            Debug.Log(tagValue + " is the tag value for " + tagKey);
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

    private void SyncVariablesToInk()
    {
        foreach (var kvp in inkVariables)
        {
            if (kvp.Value is int intValue)
            {
                story.variablesState[kvp.Key] = intValue;
            }
            else if (kvp.Value is bool boolValue)
            {
                story.variablesState[kvp.Key] = boolValue;
            }
            // ajouter d'autres types si besoin
        }
    }

    public void UpdateVariable(string key, object value)
    {
        if (inkVariables.ContainsKey(key))
        {
            inkVariables[key] = value;
        }
        else
        {
            inkVariables.Add(key, value);
        }
    }

    private void UpdateVariableInInk(string key, object value)
    {
        if (value is int intValue)
            story.variablesState[key] = intValue;
        else if (value is bool boolValue)
            story.variablesState[key] = boolValue;
        else if (value is float floatValue)
            story.variablesState[key] = floatValue;
        else if (value is string strValue)
            story.variablesState[key] = strValue;
        else
            Debug.LogWarning($"UpdateVariableInInk: type of {key} not supported");
    }

    public void SyncAllVariablesToInk()
    {
        foreach (var kvp in inkVariables)
        {
            UpdateVariableInInk(kvp.Key, kvp.Value);
        }
    }

    private void OnMoneyChanged(int newAmount)
    {
        UpdateVariable("coins", newAmount);
    }

    private void StartQuest(string questID)
    {
        EventManager.instance.questEvents.StartQuest(questID);
    }

    private void AdvanceQuest(string questID)
    {
        EventManager.instance.questEvents.AdvanceQuest(questID);
    }

    private void FinishQuest(string questID)
    {
        EventManager.instance.questEvents.FinishQuest(questID);
    }

    private void AddToInventory(string item)
    {
        InventoryManager.Instance.AddItemByName(item);
    }

    private void QuestStateChange(Quest quest)
    {
        Debug.Log($"Quest state changed: {quest.questInfo.id} - {quest.state}");

        GameEventsManager.Instance.DialogueEvent.UpdateInkDialogueVariable(
            quest.questInfo.id + "State",
            new StringValue(quest.state.ToString())
        );
        UpdateVariableInInk(quest.questInfo.id + "State", new StringValue(quest.state.ToString()));
    }

    private void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        UpdateVariableState(name, value);
    }

    public void UpdateVariableState(string name, Ink.Runtime.Object value)
    {
        // only maintain variables that were initialized from the globals ink file
        if (!inkVariables.ContainsKey(name))
        {
            return;
        }
        inkVariables[name] = value;
        Debug.Log("Updated dialogue variable: " + name + " = " + value);
    }
}
