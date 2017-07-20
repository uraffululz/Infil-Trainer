using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_PressurePlateWeights : MonoBehaviour {

	GameObject treasure;
	GameObject counterWeight;
	GameObject plateBase;

	Rigidbody treasureRB;
	Rigidbody cWeightRB;


	void Start () {
		//treasure = GameObject.Find ("Treasure");
		counterWeight = GameObject.Find ("CounterWeight");
		plateBase = GameObject.Find ("PlateBase");

		//treasureRB = treasure.GetComponent<Rigidbody> ();
		cWeightRB = counterWeight.GetComponent<Rigidbody> ();
	}
	

	void Update () {
		
	}

	void LateUpdate () {
		//If the pressurePlate rises too high, game over
		if (gameObject.transform.position.y > 0.53f && Time.timeSinceLevelLoad > 0.5f) {
			Debug.Log ("Plate too high");
			GameOver ();
		}
	}

	void OnCollisionStay (Collision col) {
		//When the counterWeight is placed on the pressurePlate, enable its useGravity parameter
		if (col.gameObject == counterWeight) {
			cWeightRB.useGravity = true;
			cWeightRB.constraints = RigidbodyConstraints.FreezeRotation;
		}
		//If the pressurePlate touches its base, game over
		if (col.gameObject == plateBase) {
			Debug.Log ("Plate touched base");
			GameOver ();
		}
	}

	void OnCollisionExit (Collision colStop) {
		//When the counterWeight is taken off the pressurePlate, disable its useGravity parameter
		if (colStop.gameObject == counterWeight) {
				cWeightRB.useGravity = false;
			}
		}

	void GameOver () {
		Debug.Log ("Game Over");
	}

}
