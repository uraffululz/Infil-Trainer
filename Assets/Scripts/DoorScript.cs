using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

	GameObject Player;
	public float distToPlayer;


	void Start () {
		Player = GameObject.Find ("Player");
	}
	

	void Update () {
		distToPlayer = Vector3.Distance (gameObject.transform.position, Player.transform.position);
		if (distToPlayer <= 2.0f) {
			Debug.Log ("Open the door");
			if (Input.GetKeyDown(KeyCode.E)) {
				Debug.Log ("Success");
			}
		}
	}
}
