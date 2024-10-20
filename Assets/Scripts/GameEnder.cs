using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    public GameObject quitConfirmationPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public SoundPlayer soundPlayer;

    public void OnReplayButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Celebrate()
    {
        soundPlayer.PlayWinner();
    }

    public void OnReturnButtonClick()
    {
        GameObject setupScriptObject = SetupScript.instance.gameObject;

        if (setupScriptObject != null)
        {
            Destroy(setupScriptObject);
        }

        GameObject audioObject = AudioManager.instance.gameObject;

        if (audioObject != null)
        {
            Destroy(audioObject);
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitButtonClick()
    {
        quitConfirmationPanel.SetActive(true);
    }

    public void CancelQuit()
    {
        quitConfirmationPanel.SetActive(false);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    } 
}
