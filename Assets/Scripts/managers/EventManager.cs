using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
 public static EventManager instance { get; private set; }

    public QuestEvents questEvents;


    private void Awake()
    {
        if (instance != null)
        {
          Debug.LogError("EventManager instance already exists.");
        }
        instance = this;

        questEvents = new QuestEvents();
    }
}
