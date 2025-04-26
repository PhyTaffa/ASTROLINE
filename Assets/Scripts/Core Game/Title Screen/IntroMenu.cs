using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMenu : MonoBehaviour {
    
    //[SerializeField] private Fading fading; // Reference to the Fading script

    public void PlayGame() {
        
        SceneManager.LoadScene("ShipMenu");
       // fading.FadeOutAndChangeScene(1); // Trigger fade-out and load scene
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame triggered!");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
