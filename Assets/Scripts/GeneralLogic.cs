using UnityEngine;
using TMPro;

public class GeneralLogic : MonoBehaviour
{
    public TextMeshProUGUI scoreText;    
    public TextMeshProUGUI timeText;    
    public GameObject monkey;
    public GameObject lion;
    public SoundPlayer soundPlayer;
    public GameObject winPanel;
    public GameObject losePanel;

    private float winScore = 10;
    private int score = 0;               // Tracks the player's score
    private float elapsedTime = 300f;      // Tracks the elapsed time since the start of the game
    private bool isGameOver = false;

    void Start()
    {
        //AudioManager.instance.PlayBackgroundMusic();
    }

    void Update()
    {
        // Update the elapsed time
        elapsedTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);  // Calculate total minutes
        int seconds = Mathf.FloorToInt(elapsedTime % 60);  // Calculate remaining seconds

        timeText.text = string.Format("Time: {0:0}:{1:00}", minutes, seconds);

        if ((elapsedTime <= 0 || ((Vector3.Distance(lion.transform.position, monkey.transform.position) < 0.3f) && elapsedTime<293)) && !isGameOver)
        {
            soundPlayer.PlayChomp();
            soundPlayer.PlayLoser();
            losePanel.SetActive(true);
            monkey.SetActive(false);
            isGameOver = true;
        }
    }

    // Method to increase the score when a banana is collected
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Bananas: " + score;
        if (score >= winScore && !isGameOver)
        {
            soundPlayer.PlayWinner();
            winPanel.SetActive(true);
            lion.SetActive(false);
            isGameOver = true;
        }
    }
}
