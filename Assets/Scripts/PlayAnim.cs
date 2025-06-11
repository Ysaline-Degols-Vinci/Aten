using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour
{

    [SerializeField] private AnimManager monAnim;


    Boolean playerIsNear = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }


    private void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.E))
        {

            monAnim.JouerAnimation();
        }
    }
}
