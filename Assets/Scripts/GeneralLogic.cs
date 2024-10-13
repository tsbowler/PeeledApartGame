using UnityEngine;
using TMPro;  // Add this to use TextMeshProUGUI

public class GeneralLogic : MonoBehaviour
{
    public TextMeshProUGUI scoreText;    // Reference to the TextMeshProUGUI element for score
    public TextMeshProUGUI timeText;     // Reference to the TextMeshProUGUI element for time
    
    private int score = 0;               // Tracks the player's score
    private float elapsedTime = 0f;      // Tracks the elapsed time since the start of the game

    void Update()
    {
        // Update the elapsed time
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);  // Calculate total minutes
        int seconds = Mathf.FloorToInt(elapsedTime % 60);  // Calculate remaining seconds

        timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    // Method to increase the score when a banana is collected
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Bananas: " + score;
    }
}
