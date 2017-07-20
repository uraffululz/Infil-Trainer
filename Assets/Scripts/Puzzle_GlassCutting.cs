using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_GlassCutting : MonoBehaviour {

	[SerializeField] GameObject DottedLinePrefab;
	GameObject dottedLineDot;
	[SerializeField] List<GameObject> dottedLineSegments;

	[SerializeField] GameObject CutLinePrefab;
	GameObject cutLineDot;
	[SerializeField] List<GameObject> cutLineSegments;

	float angleOnCircle = 0.0f;

	[SerializeField] float cutTimer = 0.5f;
	[SerializeField] float crackTimer = 1.0f; 


	void Start () {
		dottedLineSegments = new List<GameObject>();
		cutLineSegments = new List<GameObject>();

		for (int i = 0; i < 24; i++) {
			dottedLineDot = Instantiate (DottedLinePrefab, gameObject.transform.position + (Vector3.up * 0.2f), Quaternion.identity, gameObject.transform);
			dottedLineSegments.Add (dottedLineDot);
			dottedLineDot.transform.RotateAround (gameObject.transform.position, Vector3.forward, angleOnCircle);
			angleOnCircle = angleOnCircle + 15.0f;
		}
	}
	

	void Update () {
		Cutting ();
	}


	void Cutting () {
		Ray cutter = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (cutter, out hit, 5)) {
			if (hit.collider.gameObject.name == "DottedLinePrefab(Clone)") {
				Debug.DrawLine (cutter.origin, hit.point);
				if (cutTimer > 0.0f) {
					cutTimer = cutTimer - 1.0f * Time.deltaTime;
				} else if (cutTimer <= 0.0f) {
					hit.collider.gameObject.SetActive (false);
					cutLineDot = Instantiate (CutLinePrefab, hit.collider.gameObject.transform.position,
						hit.collider.gameObject.transform.rotation, gameObject.transform);
					dottedLineSegments.Remove (hit.collider.gameObject);
					cutLineSegments.Add (cutLineDot);
					if (dottedLineSegments.Count == 0) {
						Debug.Log ("You cut through! Claim your treasure!");
					}
				}

			} else if (hit.collider.gameObject.name == "CutLinePrefab(Clone)") {
				Debug.DrawLine (cutter.origin, hit.point);
				if (crackTimer > 0.0f) {
					crackTimer = crackTimer - 1.0f * Time.deltaTime;
				} else if (crackTimer <= 0.0f) {
					hit.collider.gameObject.SetActive (false);
//Instantiate a "crackedLinePrefab" or a "Shattering Glass" model/prefab
					//cutLineSegments.Remove (hit.collider.gameObject);
					Debug.Log ("You cracked the glass.");
					GameOver ();
				}

			} else {
				cutTimer = 0.5f;
				crackTimer = 1.0f;
			}
		}
	}

	void GameOver () {
		Debug.Log ("Game over");
	}

}
