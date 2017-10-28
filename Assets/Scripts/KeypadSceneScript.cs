using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadSceneScript : MonoBehaviour {

	GameObject door;


		void Start () {
		door = GameObject.Find ("ExitDoor");
		door.GetComponent<DoorScript_Keypad> ().enabled = true;
	}
	

	void Update () {
		
	}
}
