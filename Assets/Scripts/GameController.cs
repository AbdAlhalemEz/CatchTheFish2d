using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public Camera cam;
	public GameObject[] FishBombs;
	public float timeLeft;
	public Text timerText;
	public GameObject gameOverText;
	public Text winText;
	public GameObject restartButton;
	public GameObject splashScreen;
	public GameObject startButton;
	public NetController net_Controller;

	private float maxWidth;
	private bool playing;
	private int ballCount;
	public Text HScoreText;

	// Use this for initialization
	void Start () {

		// If no camera is assigned, set the main camera as the camera
		if (cam == null) {
			cam = Camera.main;
		}

		playing = false;

		// Calculate the maximum width for ball spawning
		Vector3 upperCorner = new Vector3 (Screen.width, Screen.height, 0.0f);
		Vector3 targetWidth = cam.ScreenToWorldPoint(upperCorner);
		float ballWidth = FishBombs[0].GetComponent<Renderer>().bounds.extents.x;
		maxWidth = targetWidth.x-ballWidth;

		// Update the UI
		UpdateText ();
	}

	void FixedUpdate () {
		if (playing) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				timeLeft = 0;
			}
			UpdateText ();		
		}
	}

	// Called when the "Start" button is clicked
	public void StartGame () {
		splashScreen.SetActive (false);
		startButton.SetActive (false);
		playing = true;
		net_Controller.toggledControl (true);
		StartCoroutine (Spawn ());

		// Update the high score UI
		int HScore = PlayerPrefs.GetInt("score", 0);
		HScoreText.text= "Your Highest score is\n"+ HScore;
	}

	public void BallCountUpdate () {
		ballCount--;
	}

	// Coroutine for spawning balls
	IEnumerator Spawn () {
		
		while (!gameOverText.activeSelf && timeLeft > 0) {
			
			// Spawn a random number of balls each time
			int rand = Random.Range (1, 10);
			while (rand > 0) {
				GameObject ball = FishBombs[Random.Range (0, FishBombs.Length)];
				Vector3 spawnPosition = new Vector3 (
					Random.Range (-maxWidth, maxWidth), 
					transform.position.y, 
					0.0f
				);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (ball, spawnPosition, spawnRotation);
				ballCount++;
				rand--;
				yield return new WaitForSeconds (Random.Range (0.5f, 0.7f));
			}
			yield return new WaitForSeconds (Random.Range (0.7f, 1.3f)); // Wait for 1 or 2 seconds & go for the loop again
		}

		// Show win text if time is over and player is still alive
		if (!gameOverText.activeSelf) {
			yield return new WaitForSeconds(1.0f);
			winText.gameObject.SetActive(true);	
		}

		// Show the restart button
		yield return new WaitForSeconds(.5f);
		restartButton.SetActive (true);
	} 

	// Update the timer UI
	void UpdateText () {
		timerText.text = "Time Left\n" + Mathf.RoundToInt (timeLeft);
	}
}
