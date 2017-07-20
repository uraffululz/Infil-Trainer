using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_PressurePlate : MonoBehaviour {

	GameObject pressurePlate;
	GameObject treasure;
	GameObject counterWeight;


	void Start () {
		pressurePlate = GameObject.Find ("PressurePlate");
		treasure = GameObject.Find ("Treasure");
		counterWeight = GameObject.Find ("CounterWeight");
	}
	

	void FixedUpdate () {
		Ray grabber = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit ableToGrab;

//Is this doing anything? Turn off and on again to find out, or try increasing/decreasing
		pressurePlate.GetComponent<Rigidbody> ().maxDepenetrationVelocity = 0.1f;

		if (Input.GetMouseButton(0) && Physics.Raycast(grabber, out ableToGrab, 5)) {
			if (ableToGrab.collider.gameObject == treasure || ableToGrab.collider.gameObject == counterWeight) {
				ableToGrab.rigidbody.MovePosition(
					Vector2.Lerp(ableToGrab.transform.position, ableToGrab.point, 0.3f));
			}
		}
			
		/*If the pressurePlate remains half-cocked,
		and the treasure is placed into the Player's bag (or whatever),
		the player WINS
		*/
	}

}
