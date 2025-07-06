using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Ink.Runtime;
public class DialogueEvent
{
    public event Action<String> onEnterDialogue;

    public void EnterDialogie(String KnotName)
    {
       onEnterDialogue?.Invoke(KnotName);
    }

    public event Action onDialogueStarted;

    public void DialogueStarted()
    {
        if (onDialogueStarted != null)
            onDialogueStarted();    
    }

    public event Action onDialogueFinished;

    public void DialogueFinished()
    {
        if (onDialogueFinished != null)
            onDialogueFinished();
    }

    public event Action<String, String, String, List<Choice>> onDisplayDialogue;
    public void DisplayDialogue(String dialogue, string speaker, string portrait, List<Choice> dialogueChoices)
    {
        if (onDisplayDialogue != null)
            onDisplayDialogue(dialogue, speaker, portrait, dialogueChoices);
    }

    public event Action<int> onUpdateChoiceIndex;
    public void UpdateChoiceIndex(int choiceIndex)
    {
        if (onUpdateChoiceIndex != null)
            onUpdateChoiceIndex(choiceIndex);
    }


    public event Action<string, Ink.Runtime.Object> onUpdateInkDialogueVariable;
    public void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        if (onUpdateInkDialogueVariable != null)
        {
            onUpdateInkDialogueVariable(name, value);
            Debug.Log("DialogueEvent: Updated Ink variable " + name + " with value: " + value + "AAAAAAAAAAAAAAAAAAAAAAAA");
        }
    }
}
