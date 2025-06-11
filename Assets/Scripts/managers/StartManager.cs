using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI coinText;

    void Start()
    {
        //TODO je garde le fichier mais ça c'est géré autre part tkt
        dialogueBox.SetActive(true);
        coinText.alignment = TextAlignmentOptions.Center;

    }
}
