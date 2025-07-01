using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager Instance { get; private set; }
    public DialogueEvent DialogueEvent;
    public InputEventContext InputEventContext = InputEventContext.DEFAULT;

    public void setToInventory()
    {
        InputEventContext = InputEventContext.INVENTORY;
    }

    public void setToDefault()
    {
        InputEventContext = InputEventContext.DEFAULT;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one");

        }
        Instance = this;

        DialogueEvent = new DialogueEvent();
        Debug.Log("input :" + InputEventContext);
    }
}



