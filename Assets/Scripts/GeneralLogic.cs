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
    private int score = 0;           
    private float elapsedTime = 300f;      
    private bool isGameOver = false;
    public bool canMonkeyDie = true;

    void Update()
    {
        elapsedTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);  
        int seconds = Mathf.FloorToInt(elapsedTime % 60);  

        timeText.text = string.Format("Time: {0:0}:{1:00}", minutes, seconds);

     // player loses if times is up, or lion reaches them, unless monkey is currently invulnerable
        if ((elapsedTime <= 0 || ((Vector3.Distance(lion.transform.position, monkey.transform.position) < 0.3f) && elapsedTime<293)) && !isGameOver && canMonkeyDie)
        {
            soundPlayer.PlayChomp();
            soundPlayer.PlayLoser();
            losePanel.SetActive(true);
            monkey.SetActive(false);
            isGameOver = true;
        }
    }

    // increment score and activate win screen on winning score
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
