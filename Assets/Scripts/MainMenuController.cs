using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    // Assign these buttons in the inspector or programmatically
    public Button playButton;
    public Button gameSetupButton;
    public Button howToPlayButton;
    public Button quitButton;

    // Panels or sub-menus for Game Setup, How to Play, and Quit Confirmation
    public GameObject gameSetupPanel;
    public GameObject howToPlayPanel;
    public GameObject quitConfirmationPanel;

    public TextMeshProUGUI mapText; 
    public Button leftMap;
    public Button rightMap;
    private int mapChoice = 1;

    void Start()
    {  
        // Hook up button click events
        playButton.onClick.AddListener(OnPlayButtonClick);
        gameSetupButton.onClick.AddListener(OnGameSetupButtonClick);
        howToPlayButton.onClick.AddListener(OnHowToPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);

        // Ensure sub-menus and confirmation panels are hidden at the start
        gameSetupPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        quitConfirmationPanel.SetActive(false);
    }

    // Play button logic
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

    // Game Setup button logic
    void OnGameSetupButtonClick()
    {
        // Show the Game Setup panel and hide the main menu buttons
        UpdateButtonInteractivity();
        gameSetupPanel.SetActive(true);
    }

    // How to Play button logic
    void OnHowToPlayButtonClick()
    {
        // Show the How to Play panel
        howToPlayPanel.SetActive(true);
    }

    // Quit button logic
    void OnQuitButtonClick()
    {
        // Show the quit confirmation panel
        quitConfirmationPanel.SetActive(true);
    }

    // Called when user confirms they want to quit the game
    public void ConfirmQuit()
    {
        Application.Quit();
    }

    // Return to main menu from panels
    public void ReturnToMainMenu()
    {
        // Hide all sub-menus and return to the main menu
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
