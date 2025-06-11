using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

  
    //bouton play

    public void PlayGame()
    {
        Debug.Log("Play Game button clicked");
        SceneManager.LoadSceneAsync(2);

    }

    //bouton options
    public void OpenOptions()
    {
        Debug.Log("Open Options button clicked");
    }

    //bouton quit
    public void QuitGame()
    {
        Debug.Log("Quit Game button clicked");
        Application.Quit();

        // If running in the editor, stop playing
//#if UNITY_EDITOR
  //      UnityEditor.EditorApplication.isPlaying = false;
//#endif
    }
}
