using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour {

	GameObject player;
	PlayerUI scoreScript;

	Vector3 startPos;

	int points;


	void Start () {
		player = GameObject.Find ("Player");
		scoreScript = player.GetComponent<PlayerUI> ();

		startPos = transform.position;
	}
	

	void Update () {
		CoinMotion ();
	}


	void OnTriggerEnter (Collider other) {
		//Determine points value based on which type of object this is
/*Am I going to have different pickups? Do I even need to compare the gameobject's tag?
 * Maybe the instantiated Coin is just a generic imagery for "I'm going to make money from this thing"
 * Maybe add valuable paintings to the walls? But then I'd have to model/texture them
 */
		if (gameObject.tag == "Coin") {
			points = 100;
		}

		//When the player touches this object
		if (other.gameObject.CompareTag("Player")) {
			Destroy (this.gameObject);
			scoreScript.scorePoints = scoreScript.scorePoints + points;
		}
	}


	void CoinMotion () {
		//Coin rotation
		transform.Rotate (Vector3.forward * 20 * Time.deltaTime);
		//Coin movement (bobbing up and down)
		Vector3 newPos = startPos + Vector3.up * 0.2f * Mathf.Sin (Time.timeSinceLevelLoad);
		transform.position = newPos;
	}
}
