using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private PromptAnimator promptAnimator;
    [SerializeField] private string dialogueKnotName;
    Boolean playerIsNear = false;
    [SerializeField] private bool isShop;

    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {

            if(!dialogueKnotName.Equals(string.Empty))
            {
                GameEventsManager.Instance.DialogueEvent.EnterDialogie(dialogueKnotName, isShop, false);
            }
           

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && promptAnimator != null)
        {
            promptAnimator.Show();
            playerIsNear = true;
        }else if(other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && promptAnimator != null)
        {
            promptAnimator.Hide();
            playerIsNear = false;
        }else if(other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
