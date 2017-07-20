using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript_Rotary : MonoBehaviour {

	//GameObject References
	GameObject Dial;

	//Dial Rotation Variables
	int dialRotSpeed = 20;
	[SerializeField]
	float dialAngle;

	//Combo Variables
	public int comboNum1;
	public int comboNum2;
	public int comboNum3;
	bool comboNum1Reached = false;
	bool comboNum2Reached = false;
	bool comboNum3Reached = false;
	public float comboTimer = 1.0f;
	int attemptsRemaining = 4;

	//State Machine Variables
	private enum comboStates {traversing_level, seeking_combo1, seeking_combo2, seeking_combo3, door_open, attempt_failed};
	private comboStates myState;


	void Start () {
		Dial = GameObject.Find ("RotaryDial");
		comboNum1 = Random.Range (0, 360);
		comboNum2 = Random.Range (0, 360);
		comboNum3 = Random.Range (0, 360);
		//myState = comboStates.traversing_level;
		myState = comboStates.seeking_combo1;
	}
	

	void Update () {
/*While the player is the in the "traversing_level" state, dodging lasers...
		if(myState = comboStates.traversing_level){
		//When the player activates the door, he switches from "traversing_level" to "seeking_combo1"
		//When this switch happens, disable player movement/crouching/(and look rotation?)
		}
*/

		/*else*/
		if (myState == comboStates.seeking_combo1) {
			RotateDial ();
			SeekingCombo1 ();

		} else if (myState == comboStates.seeking_combo2) {
			RotateDial ();
			SeekingCombo2 ();

		} else if (myState == comboStates.seeking_combo3) {
			RotateDial ();
			SeekingCombo3 ();

		} else if (myState == comboStates.door_open) {
			DoorOpen ();

		} else if (myState == comboStates.attempt_failed) {
			AttemptFailed ();
		}

//Dial Notes
//Is there a margin for error for touch devices?
//What happens if the player fails the challenge entirely?
	}


	void RotateDial () {
		//Use input to rotate Dial
		Dial.transform.Rotate (Vector3.back, Input.GetAxis ("Horizontal") * dialRotSpeed * Time.deltaTime);
		//Get Dial rotation angle #(between 0 and 360)
		dialAngle = Dial.transform.rotation.eulerAngles.z;
	}

	void SeekingCombo1 () {
/*Put in an arrow prompt to let Player know which direction to rotate Dial
 */
		//Determine if the Dial is being rotated in the wrong direction
		if (Input.GetAxis("Horizontal") < 0.0f) {
			Debug.Log ("Wrong Direction. Attempt FAILED");
		//If so, then the attempt at unlocking the door is failed
			attemptsRemaining--;
			myState = comboStates.attempt_failed;
		}
		if (comboNum1Reached == false && (int)dialAngle <= comboNum1 + 5 && (int)dialAngle >= comboNum1) {
			//When in range of first combo number, notify player via vibration/sound/something
			Debug.Log ("Getting closer");
			if ((int)dialAngle <= comboNum1 + 1 && (int)dialAngle >= comboNum1 - 1) {
				/*Then, when they reach the proximity of the exact combo number,
*signify proximity to success with an appropriate increase in sound/vibration/whatever
*Then, if the player maintains the Dial's rotation at the correct angle for just a second(measured by a simple timer),
*the current combo# will unlock
*/
				if (comboTimer > 0.0f) {
					comboTimer = comboTimer - 0.5f * Time.deltaTime;
				} else if (comboTimer <= 0.0f) {
					Debug.Log ("*Click*");
					comboNum1Reached = true;
					myState = comboStates.seeking_combo2;
				}
			}
		} else if ((int)dialAngle < comboNum1 - 1 && (int)dialAngle >= comboNum1 - 2) {
			//If the Player rotates the Dial too far, then the attempt at unlocking the door is failed
			Debug.Log ("You rotated too far. Attempt FAILED");
			attemptsRemaining--;
			myState = comboStates.attempt_failed;
		} else {
			comboTimer = 1.0f;
		}
	}

	void SeekingCombo2 () {
/*Put in an arrow prompt to let Player know which direction to rotate Dial
 */
		//Determine if the Dial is being rotated in the wrong direction
		if (Input.GetAxis("Horizontal") > 0.0f) {
			Debug.Log ("Wrong Direction. Attempt FAILED");
		//If so, then the attempt at unlocking the door is failed
			attemptsRemaining--;
			myState = comboStates.attempt_failed;
		}
		if (comboNum2Reached == false  && (int)dialAngle >= comboNum2 - 5 && (int)dialAngle <= comboNum2) {
			//When in range of first combo number, notify player via vibration/sound/something
			Debug.Log ("Getting closer");
			if ((int)dialAngle >= comboNum2 - 1 && (int)dialAngle <= comboNum2 + 1) {
				/*Then, when they reach the proximity of the exact combo number,
*signify proximity to success with an appropriate increase in sound/vibration/whatever
*Then, if the player maintains the Dial's rotation at the correct angle for just a second(measured by a simple timer),
*the current combo# will unlock
*/
				if (comboTimer > 0.0f) {
					comboTimer = comboTimer - 0.5f * Time.deltaTime;
				} else if (comboTimer <= 0.0f) {
					Debug.Log ("*Click*");
					comboNum2Reached = true;
					myState = comboStates.seeking_combo3;
				}
			}
		} else if ((int)dialAngle > comboNum2 + 1 && dialAngle <= comboNum2 + 2) {
			//If the Player rotates the Dial too far, then the attempt at unlocking the door is failed
			Debug.Log ("You rotated too far. Attempt FAILED");
			attemptsRemaining--;
			myState = comboStates.attempt_failed;
		} else {
			comboTimer = 1.0f;
		}
	}

	void SeekingCombo3 () {
/*Put in an arrow prompt to let Player know which direction to rotate Dial
 */
		//Determine if the Dial is being rotated in the wrong direction
		if (Input.GetAxis("Horizontal") < 0.0f) {
			Debug.Log ("Wrong Direction. Attempt FAILED");
		//If so, then the attempt at unlocking the door is failed
			attemptsRemaining--;
			myState = comboStates.attempt_failed;
		}
		if (comboNum3Reached == false && (int)dialAngle <= comboNum3 + 5 && (int)dialAngle >= comboNum3) {
			//When in range of first combo number, notify player via vibration/sound/something
			Debug.Log ("Getting closer");
			if ((int)dialAngle <= comboNum3 + 1 && (int)dialAngle >= comboNum3 - 1) {
				/*Then, when they reach the proximity of the exact combo number,
*signify proximity to success with an appropriate increase in sound/vibration/whatever
*Then, if the player maintains the Dial's rotation at the correct angle for just a second(measured by a simple timer),
*the current combo# will unlock
*/
				if (comboTimer > 0.0f) {
					comboTimer = comboTimer - 0.5f * Time.deltaTime;
				} else if (comboTimer <= 0.0f) {
					Debug.Log ("*Click*");
					comboNum3Reached = true;
					myState = comboStates.door_open;
				}
			}
		} else if ((int)dialAngle < comboNum3 - 1 && dialAngle >= comboNum3 - 2) {
			//If the Player rotates the Dial too far, then the attempt at unlocking the door is failed
			Debug.Log ("You rotated too far. Attempt FAILED");
			attemptsRemaining--;
			myState = comboStates.attempt_failed;
		} else {
			comboTimer = 1.0f;
		}
	}

	void DoorOpen () {
		Debug.Log ("Door open!");
	}

	void AttemptFailed () {
		if (attemptsRemaining > 0) {
			Debug.Log ("Attempts Remaining: " + attemptsRemaining + ". Press R to Retry");
			if (Input.GetKeyDown(KeyCode.R)) {
				Dial.transform.Rotate(0.0f, 0.0f, -dialAngle);
				dialAngle = 0.0f;
				myState = comboStates.seeking_combo1;
			}
		} else {
			Debug.Log ("No attempts Left. YOU LOSE");
			//GameOver();
		}
	}
}
