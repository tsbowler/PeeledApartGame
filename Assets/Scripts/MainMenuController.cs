using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button gameSetupButton;
    public Button howToPlayButton;
    public Button quitButton;

    public GameObject gameSetupPanel;
    public GameObject howToPlayPanel;
    public GameObject quitConfirmationPanel;

    public TextMeshProUGUI mapText; 
    public Button leftMap;
    public Button rightMap;
    private int mapChoice = 1;

    void Start()
    {  
        playButton.onClick.AddListener(OnPlayButtonClick);
        gameSetupButton.onClick.AddListener(OnGameSetupButtonClick);
        howToPlayButton.onClick.AddListener(OnHowToPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);

        gameSetupPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        quitConfirmationPanel.SetActive(false);
    }

    // load scene based on map setting chosen
    void OnPlayButtonClick()
    {
        if (mapChoice == 1)
        {
            SceneManager.LoadScene("Map1");
        }
        else if (mapChoice == 2)
        {
            SceneManager.LoadScene("Map2");
        }
        else 
        {
            SceneManager.LoadScene("Map3");
        }
    }

    void OnGameSetupButtonClick()
    {
        UpdateButtonInteractivity();
        gameSetupPanel.SetActive(true);
    }

    void OnHowToPlayButtonClick()
    {
        howToPlayPanel.SetActive(true);
    }

    void OnQuitButtonClick()
    {
        quitConfirmationPanel.SetActive(true);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        gameSetupPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        quitConfirmationPanel.SetActive(false);
    }

    public void increaseMap()
    {
        mapChoice++;
        UpdateButtonInteractivity();
    }

    public void decreaseMap()
    {
        mapChoice--;
        UpdateButtonInteractivity();
    }

    // player can't click buttons beyond possible options
    private void UpdateButtonInteractivity()
    {
        if (leftMap!= null) 
            leftMap.interactable = mapChoice > 1;

        if (rightMap != null) 
            rightMap.interactable = mapChoice < 3;

        if (mapChoice == 1)
        {
            mapText.text = "Classic Jungle";
        }
        else if (mapChoice == 2)
        {
            mapText.text = "The Cat Cave";
        }
        else if (mapChoice == 3)
        {
            mapText.text = "The Potasseum";
        }
    }
}
