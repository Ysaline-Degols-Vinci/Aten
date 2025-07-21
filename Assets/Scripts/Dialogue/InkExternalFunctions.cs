using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions 
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
        story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
        story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
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
        story.BindExternalFunction("AddItemInventory", (string item) => AddToInventory(item));
        story.BindExternalFunction("IsItemQuantityPresent", (string itemName) =>
        {
            return IsItemQuantityPresent(itemName);
        });
        story.BindExternalFunction("RemoveFromInventory", (string itemName, int quantity) => RemoveFromInventory(itemName, quantity));



    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("gainCoin");
        story.UnbindExternalFunction("setTrashCoin");
        story.UnbindExternalFunction("setTossedCoinWell");
        story.UnbindExternalFunction("StartQuest");
        story.UnbindExternalFunction("AdvanceQuest");
        story.UnbindExternalFunction("FinishQuest");
        story.UnbindExternalFunction("AddItemInventory");
        story.UnbindExternalFunction("IsItemQuantityPresent");
        story.UnbindExternalFunction("RemoveFromInventory");
    }

    private void StartQuest(string questId)
    {
        EventManager.instance.questEvents.StartQuest(questId);
    }

    private void AdvanceQuest(string questId)
    {
        EventManager.instance.questEvents.AdvanceQuest(questId);
    }

    private void FinishQuest(string questId)
    {
        EventManager.instance.questEvents.FinishQuest(questId);
    }

    private void AddToInventory(string item)
    {
        InventoryManager.Instance.AddItemByName(item);
    }

    private int IsItemQuantityPresent(string itemName)
    {
        return InventoryManager.Instance.ItemPresence(itemName);
    }

    private void UpdateVariable(string variableName, object value)
    {
        Ink.Runtime.Object inkValue = null;

        if (value is int intValue)
            inkValue = new Ink.Runtime.IntValue(intValue);
        else if (value is bool boolValue)
            inkValue = new Ink.Runtime.BoolValue(boolValue);
        else if (value is float floatValue)
            inkValue = new Ink.Runtime.FloatValue(floatValue);
        else if (value is string strValue)
            inkValue = new Ink.Runtime.StringValue(strValue);
        else
        {
            Debug.LogWarning($"[UpdateVariable] Type {value.GetType()} not supported for Ink variable: {variableName}");
            return;
        }
        DialogueManager.Instance.inkDialogueVariables.UpdateVariableState(variableName, inkValue);
    }

    public void RemoveFromInventory(string itemName, int quantity)
    {
        InventoryManager.Instance.removeItem(itemName, quantity);
    }
}
