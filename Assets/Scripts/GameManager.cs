using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score;
    public float highscore;
    
    public bool gameStarted;
    public bool isFalling;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    private void Awake()
    {
        score = 0;
        scoreText.text = score.ToString();
        highscore = PlayerPrefs.GetFloat("Highscore", 0f);
        highscoreText.text = "Highscore: " + highscore.ToString();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }
    
    public void StartGame()
    {
        gameStarted = true;
        isFalling = false;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
    
    public void IncreaseScore()
    {
        score++;
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat("Highscore", highscore);
        }
        scoreText.text = score.ToString();
        highscoreText.text = "Highscore: " + highscore.ToString();
    }
}
