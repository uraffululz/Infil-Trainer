using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DoorScript_Keypad : MonoBehaviour {

	[SerializeField] int[] keypadDigits;
	[SerializeField] string keypadCode;
	[SerializeField] List<string> keypadGuesses;

	public List<GameObject> Buttons;
	GameObject buttonClicked;
	Text buttonClickedText;

	int guessesRemaining = 30;


	void Start () {
		KeypadSetup ();
		ButtonSetup ();
	}
	

	void Update () {
		//ClickButton ();
	}


	void KeypadSetup () {
		//Get 4 digits which comprise the keypad sequences
		//None of the digits should be repeated
		keypadDigits = new int[4];
		keypadDigits[0] = Random.Range(0, 10);
		keypadDigits[1] = Random.Range(0, 10);
		while (keypadDigits[1] == keypadDigits[0]) {
			keypadDigits [1] = Random.Range (0, 10);
		}
		keypadDigits[2] = Random.Range(0, 10);
		while (keypadDigits[2] == keypadDigits[0] || keypadDigits[2] == keypadDigits[1]) {
			keypadDigits [2] = Random.Range (0, 10);
		}
		keypadDigits[3] = (Random.Range(0, 10));
		while (keypadDigits[3] == keypadDigits[0] || keypadDigits[3] == keypadDigits[1] || keypadDigits[3] == keypadDigits[2]) {
			keypadDigits[3] = Random.Range (0, 10);
		}

		//Compile sequences using digits
		keypadGuesses.Add(string.Concat(keypadDigits[0], keypadDigits[1], keypadDigits[2], keypadDigits[3]));
		keypadGuesses.Add(string.Concat(keypadDigits[0], keypadDigits[1], keypadDigits[3], keypadDigits[2]));
		keypadGuesses.Add(string.Concat(keypadDigits[0], keypadDigits[2], keypadDigits[1], keypadDigits[3]));
		keypadGuesses.Add(string.Concat(keypadDigits[0], keypadDigits[2], keypadDigits[3], keypadDigits[1]));
		keypadGuesses.Add(string.Concat(keypadDigits[0], keypadDigits[3], keypadDigits[1], keypadDigits[2]));
		keypadGuesses.Add(string.Concat(keypadDigits[0], keypadDigits[3], keypadDigits[2], keypadDigits[1]));

		keypadGuesses.Add(string.Concat(keypadDigits[1], keypadDigits[0], keypadDigits[2], keypadDigits[3]));
		keypadGuesses.Add(string.Concat(keypadDigits[1], keypadDigits[0], keypadDigits[3], keypadDigits[2]));
		keypadGuesses.Add(string.Concat(keypadDigits[1], keypadDigits[2], keypadDigits[0], keypadDigits[3]));
		keypadGuesses.Add(string.Concat(keypadDigits[1], keypadDigits[2], keypadDigits[3], keypadDigits[0]));
		keypadGuesses.Add(string.Concat(keypadDigits[1], keypadDigits[3], keypadDigits[0], keypadDigits[2]));
		keypadGuesses.Add(string.Concat(keypadDigits[1], keypadDigits[3], keypadDigits[2], keypadDigits[0]));

		keypadGuesses.Add(string.Concat(keypadDigits[2], keypadDigits[0], keypadDigits[1], keypadDigits[3]));
		keypadGuesses.Add(string.Concat(keypadDigits[2], keypadDigits[0], keypadDigits[3], keypadDigits[1]));
		keypadGuesses.Add(string.Concat(keypadDigits[2], keypadDigits[1], keypadDigits[0], keypadDigits[3]));
		keypadGuesses.Add(string.Concat(keypadDigits[2], keypadDigits[1], keypadDigits[3], keypadDigits[0]));
		keypadGuesses.Add(string.Concat(keypadDigits[2], keypadDigits[3], keypadDigits[0], keypadDigits[1]));
		keypadGuesses.Add(string.Concat(keypadDigits[2], keypadDigits[3], keypadDigits[1], keypadDigits[0]));

		keypadGuesses.Add(string.Concat(keypadDigits[3], keypadDigits[0], keypadDigits[1], keypadDigits[2]));
		keypadGuesses.Add(string.Concat(keypadDigits[3], keypadDigits[0], keypadDigits[2], keypadDigits[1]));
		keypadGuesses.Add(string.Concat(keypadDigits[3], keypadDigits[1], keypadDigits[0], keypadDigits[2]));
		keypadGuesses.Add(string.Concat(keypadDigits[3], keypadDigits[1], keypadDigits[2], keypadDigits[0]));
		keypadGuesses.Add(string.Concat(keypadDigits[3], keypadDigits[2], keypadDigits[0], keypadDigits[1]));
		keypadGuesses.Add(string.Concat(keypadDigits[3], keypadDigits[2], keypadDigits[1], keypadDigits[0]));

		//Determine correct keypad code by randomly choosing a sequence from keypadGuesses[]
		keypadCode = keypadGuesses[Random.Range(0, keypadGuesses.Count)];
	}


	void ButtonSetup(){
		Buttons.Add(GameObject.Find ("Button1"));
		Buttons.Add(GameObject.Find ("Button2"));
		Buttons.Add(GameObject.Find ("Button3"));
		Buttons.Add(GameObject.Find ("Button4"));
		Buttons.Add(GameObject.Find ("Button5"));
		Buttons.Add(GameObject.Find ("Button6"));
		Buttons.Add(GameObject.Find ("Button7"));
		Buttons.Add(GameObject.Find ("Button8"));
		Buttons.Add(GameObject.Find ("Button9"));
		Buttons.Add(GameObject.Find ("Button10"));
		Buttons.Add(GameObject.Find ("Button11"));
		Buttons.Add(GameObject.Find ("Button12"));
		Buttons.Add(GameObject.Find ("Button13"));
		Buttons.Add(GameObject.Find ("Button14"));
		Buttons.Add(GameObject.Find ("Button15"));
		Buttons.Add(GameObject.Find ("Button16"));
		Buttons.Add(GameObject.Find ("Button17"));
		Buttons.Add(GameObject.Find ("Button18"));
		Buttons.Add(GameObject.Find ("Button19"));
		Buttons.Add(GameObject.Find ("Button20"));
		Buttons.Add(GameObject.Find ("Button21"));
		Buttons.Add(GameObject.Find ("Button22"));
		Buttons.Add(GameObject.Find ("Button23"));
		Buttons.Add(GameObject.Find ("Button24"));

		foreach (var button in Buttons) {
			int assignedGuess = (Random.Range (0, keypadGuesses.Count));
			Text buttonText = button.GetComponentInChildren<Text> ();
			buttonText.text = keypadGuesses[assignedGuess];
			keypadGuesses.RemoveAt (assignedGuess);
		}
	}


	public void ClickButton () {
		buttonClicked = EventSystem.current.currentSelectedGameObject;
		buttonClickedText = buttonClicked.GetComponent<Button> ().GetComponentInChildren<Text> ();
		if (buttonClickedText.text == keypadCode) {
			buttonClickedText.text = "<color=green>" + buttonClickedText.text + "</color>";
			Debug.Log ("You guessed right. YOU WIN");
		} else {
			Debug.Log ("You guessed wrong");
			guessesRemaining--;
			if (guessesRemaining <= 0) {
				Debug.Log ("You lose");
			} else {
				CompareDigits ();
			}
		}
	}

	void CompareDigits () {
		int correctlyPlacedDigits = 0;

		string guessDigit1 = buttonClickedText.text.Substring (0, 1);
		string guessDigit2 = buttonClickedText.text.Substring (1, 1);
		string guessDigit3 = buttonClickedText.text.Substring (2, 1);
		string guessDigit4 = buttonClickedText.text.Substring (3, 1);


		if (guessDigit1 == keypadCode.Substring(0, 1)) {
			correctlyPlacedDigits++;
			guessDigit1 = "<color=green>"+guessDigit1+"</color>";
		}
		if (guessDigit2 == keypadCode.Substring(1, 1)) {
			correctlyPlacedDigits++;
			guessDigit2 = "<color=green>"+guessDigit2+"</color>";
		}
		if (guessDigit3 == keypadCode.Substring (2, 1)) {
			correctlyPlacedDigits++;
			guessDigit3 = "<color=green>"+guessDigit3+"</color>";
		}
		if (guessDigit4 == keypadCode.Substring(3, 1)) {
			correctlyPlacedDigits++;
			guessDigit4 = "<color=green>"+guessDigit4+"</color>";
		}
		buttonClickedText.text = guessDigit1 + guessDigit2 + guessDigit3 + guessDigit4;

		/*Debug.Log ("Digit1: " + buttonClickedText.text.Substring (0, 1) + ", " + keypadCode.Substring (0, 1));
		Debug.Log ("Digit2: " + buttonClickedText.text.Substring (1, 1) + ", " + keypadCode.Substring (1, 1));
		Debug.Log ("Digit3: " + buttonClickedText.text.Substring (2, 1) + ", " + keypadCode.Substring (2, 1));
		Debug.Log ("Digit4: " + buttonClickedText.text.Substring (3, 1) + ", " + keypadCode.Substring (3, 1));
*/
		Debug.Log (correctlyPlacedDigits);
	}

}
