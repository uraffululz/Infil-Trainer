using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
	
	[SerializeField] GameObject roomGenerator;

	[SerializeField] Canvas scoreCanvas;
	Canvas spawnCanvas;
	public List<Text> UITexts;
	public Text scoreText;
	public Text TimerText;

	public int scorePoints;

	public float detectedTimer;
	public bool timerActive;


	void Start () {
		timerActive = false;
		detectedTimer = 0.0f;

		roomGenerator = GameObject.Find ("RoomEmpty");
		spawnCanvas = Instantiate (scoreCanvas, roomGenerator.transform);
		spawnCanvas.GetComponentsInChildren<Text> (true, UITexts);
		scoreText = UITexts [0];
		scoreText.color = Color.yellow;
		TimerText = UITexts [1];
		TimerText.color = Color.red;

		scorePoints = 0;
	}
	

	void Update () {
		TouchedLaser ();
		KeepScore ();

		if (Input.GetKeyDown(KeyCode.P)) {
			PauseMenu ();
		}
	}


	void TouchedLaser () {
		if (detectedTimer > 0.0f) {
			detectedTimer = detectedTimer - Time.deltaTime;
			TimerText.text = (detectedTimer.ToString());
			//If detectedTimer runs out, initiate Game Over and Pause/stop time
			if (detectedTimer <= 0.0f) {
				TimerText.text = ("GAME OVER");
				//timerActive = false;
				//Time.timeScale = 0.0f;
				//Make sure Time.timeScale goes back to 1.0f when game is not paused
			}/* else {
				Time.timeScale = 1.0f;
			}*/
		}
	}


	void KeepScore () {
		scoreText.text = ("Score: " + scorePoints);
	}


	void PauseMenu () {

	}


	void OnTriggerEnter (Collider other) {
		//When a Player touches the Laser, start a countdown timer

//I don't want to have to keep checking the Time.timeSinceLevelLoad here
//Fix the bug that causes him to be hit by a laser when the level loads
		if (other.CompareTag("Laser") && Time.timeSinceLevelLoad > 0.1f) {
			if (!timerActive && detectedTimer <= 0.0f) {
				timerActive = true;
				detectedTimer = 30.0f;
			}
		}
	}
}
