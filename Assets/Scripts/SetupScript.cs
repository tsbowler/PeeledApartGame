using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetupScript : MonoBehaviour
{
    public static SetupScript instance; // Singleton reference
    public GameObject AudioManager;

    public TextMeshProUGUI currentDifficultyText; 

    // References to the arrow buttons
    public Button leftArrowButton;
    public Button rightArrowButton;

    // Variable to hold the lion speed, default to Normal difficulty (1.85)
    private float lionSpeed = 1.8f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        // Initialize the difficulty text
        UpdateDifficultyText();

        // Set up the arrow button event listeners
        if (leftArrowButton != null && rightArrowButton != null) {
            leftArrowButton.onClick.AddListener(LowerDifficulty);
            rightArrowButton.onClick.AddListener(RaiseDifficulty);
        }

        // Check button availability
        UpdateButtonInteractivity();
    }

    // Method to lower the difficulty
    public void LowerDifficulty()
    {
        if (lionSpeed == 2.4f)
        {
            lionSpeed = 2.2f;
        }
        else if (lionSpeed == 2.2f)
        {
            lionSpeed = 2f;
        }
        else if (lionSpeed == 2f)
        {
            lionSpeed = 1.8f;
        }
        else if (lionSpeed == 1.8f)
        {
            lionSpeed = 1.5f;
        }

        UpdateDifficultyText();
        UpdateButtonInteractivity();
    }

    // Method to raise the difficulty
    public void RaiseDifficulty()
    {
        if (lionSpeed == 1.5f)
        {
            lionSpeed = 1.8f;
        }
        else if (lionSpeed == 1.8f)
        {
            lionSpeed = 2f;
        }
        else if (lionSpeed == 2f)
        {
            lionSpeed = 2.2f;
        }
        else if (lionSpeed == 2.2f)
        {
            lionSpeed = 2.4f;
        }

        UpdateDifficultyText();
        UpdateButtonInteractivity();
    }

    // Method to update the difficulty text based on current speed
    private void UpdateDifficultyText()
    {
        if (currentDifficultyText == null) return;

        if (lionSpeed == 1.5f)
        {
            currentDifficultyText.text = "Easy";
        }
        else if (lionSpeed == 1.8f)
        {
            currentDifficultyText.text = "Normal";
        }
        else if (lionSpeed == 2f)
        {
            currentDifficultyText.text = "Hard";
        }
        else if (lionSpeed == 2.2f)
        {
            currentDifficultyText.text = "Impossible";
        }
        else if (lionSpeed == 2.4f)
        {
            currentDifficultyText.text = "! Lion's Feast !";
        }
    }

    // Method to enable or disable arrow buttons based on current difficulty
    private void UpdateButtonInteractivity()
    {
        if (leftArrowButton != null) 
            leftArrowButton.interactable = lionSpeed > 1.5f;

        if (rightArrowButton != null) 
            rightArrowButton.interactable = lionSpeed < 2.4f;
    }

    // Method to get the lion speed (for use elsewhere in the game)
    public float GetLionSpeed()
    {
        return lionSpeed;
    }
}