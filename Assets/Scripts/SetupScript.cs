using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetupScript : MonoBehaviour
{
    public static SetupScript instance; 
    public GameObject AudioManager;

    public TextMeshProUGUI currentDifficultyText; 

    public Button leftArrowButton;
    public Button rightArrowButton;


    private float lionSpeed = 1.8f;

    void Awake() // persist settings from menu to gameplay
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
        }
    }


    void Start()
    {
        UpdateDifficultyText();

        if (leftArrowButton != null && rightArrowButton != null) {
            leftArrowButton.onClick.AddListener(LowerDifficulty);
            rightArrowButton.onClick.AddListener(RaiseDifficulty);
        }

        UpdateButtonInteractivity();
    }

    public void LowerDifficulty() // decrease lion speed at predetermined intervals
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

    public void RaiseDifficulty() // increase lion speed
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

    private void UpdateDifficultyText() // display text according to lion speed
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
            currentDifficultyText.text = "Lion's Feast";
        }
    }

    private void UpdateButtonInteractivity() // disable button use after min/max values reached
    {
        if (leftArrowButton != null) 
            leftArrowButton.interactable = lionSpeed > 1.5f;

        if (rightArrowButton != null) 
            rightArrowButton.interactable = lionSpeed < 2.4f;
    }

    // called in map scenes
    public float GetLionSpeed()
    {
        return lionSpeed;
    }
}
