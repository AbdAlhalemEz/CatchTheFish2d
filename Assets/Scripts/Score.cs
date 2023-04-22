using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {

    // References to UI elements and game objects
    public Text scoreText;
    public Text HScoreText;
    public int ballValue;
    public GameObject gameOverText;
    public GameObject restartButton;
    public Text timerText;
    public Text winText;

    // Player's current score
    private int score;

    // To save a high score:
    // Use PlayerPrefs to save and load the highest score achieved so far

    // Use this for initialization
    void Start () {
     
        // Set the initial score to zero
        score = 0;
        // Update the score text on the UI
        UpdateScore ();
    }

    // Update the score when the player collects a ball
    void OnTriggerEnter2D () {
		if (!gameOverText.activeSelf) {
        // Add the ball value to the player's score
        score += ballValue;
        // Update the score text on the UI
        UpdateScore ();
        // Get the previous highest score from PlayerPrefs
        int Hscore = PlayerPrefs.GetInt("score", 0);
        // Check if the player's current score is higher than the previous highest score
        if (score >= Hscore){
            // If so, set the new high score and save it using PlayerPrefs
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();
            // Update the high score text on the UI
            HScoreText.text = "Your Highest score is\n" + score;
        }
    }}

    // When the player collides with a bomb, show game over text and restart button
    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.tag == "Bomb") {
            gameOverText.SetActive (true);
            restartButton.SetActive (true);
            // Disable the timer text on the UI
            timerText.enabled = false;
        }
    }

    // Update the score text on the UI
    void UpdateScore () {
        scoreText.text = "Total Score\n" + score;
    }
}
