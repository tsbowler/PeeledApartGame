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
        SceneManager.LoadScene("Gameplay");
    }

    public void Celebrate()
    {
        soundPlayer.PlayWinner();
    }

    public void OnReturnButtonClick()
    {
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
