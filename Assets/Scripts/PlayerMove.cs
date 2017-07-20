using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour {

	//Global References
	Vector3 gravity;

	//GameObject References
	Camera playerView;

	//Component Variables
	Rigidbody rb;
	//MeshRenderer mRenderer;
	//CapsuleCollider capCol;

	//Movement/Rotation Variables
	int moveSpeed;
	int rotSpeed;
	enum Stances {standing, crawling};
	Stances myStance;


	void Start () {
		gravity = Physics.gravity;
		playerView = Camera.main;
		rb = GetComponent<Rigidbody> ();

		rotSpeed = 40;
		myStance = Stances.standing;
	}

	void FixedUpdate () {
		//Player movement, based on State
//Adjust scale, rotation, collider, mesh, movement, movespeed, etc. for each State
		if (myStance == Stances.standing) {
			Vector3 playerScale = Vector3.one;
			gameObject.transform.localScale = playerScale;
			UprightMove (3);
			UprightRotation ();
			ChangeIncline ();
		} else if (myStance == Stances.crawling) {
			Vector3 playerScale = new Vector3(1.0f, 0.5f, 1.0f);
			gameObject.transform.localScale = playerScale;
			CrawlingMove (2);
			CrawlingRotation ();
			ChangeIncline ();
		}
	}
	

	void Update () {

	}


	void UprightMove (int moveSpeed) {
//Fix to allow Vertical and Horizontal movement at the same time
		if (Input.GetAxis ("Vertical") != 0.0f) {
			rb.MovePosition (transform.position + (transform.forward * Input.GetAxis ("Vertical") * moveSpeed * Time.fixedDeltaTime));
		}
		if (Input.GetAxis("Horizontal") != 0.0f) {
			rb.MovePosition (transform.position + (transform.right * Input.GetAxis ("Horizontal") * moveSpeed * Time.fixedDeltaTime));
		}
	}


	void CrawlingMove (int moveSpeed) {
		if (Input.GetAxis ("Vertical") != 0.0f) {
			rb.MovePosition (transform.position + (transform.up * Input.GetAxis ("Vertical") * moveSpeed * Time.fixedDeltaTime));
		}
		if (Input.GetAxis("Horizontal") != 0.0f) {
			rb.MovePosition (transform.position + (transform.right * Input.GetAxis ("Horizontal") * moveSpeed * Time.fixedDeltaTime));
		}
	}


	void UprightRotation () {
//Clamp vertical rotation angles, so Player is not bending backwards or folding in half
		if (Input.GetKey(KeyCode.Keypad8)/* && playerView.transform.rotation.x >= -90.0f*/) {
			playerView.transform.Rotate (Vector3.left * rotSpeed * Time.fixedDeltaTime);		
		} else if (Input.GetKey(KeyCode.Keypad2)/* && playerView.transform.rotation.x <= 90.0f*/) {
			playerView.transform.Rotate (Vector3.right * rotSpeed * Time.fixedDeltaTime);		
		}
		if (Input.GetKey(KeyCode.Keypad4)) {
			transform.Rotate (Vector3.down * rotSpeed * Time.fixedDeltaTime);
		} else if (Input.GetKey(KeyCode.Keypad6)) {
			transform.Rotate (Vector3.up * rotSpeed * Time.fixedDeltaTime);
		}
	}


	void CrawlingRotation () {
		if (Input.GetKey(KeyCode.Keypad8)/* && playerView.transform.rotation.x >= -90.0f*/) {
			playerView.transform.Rotate (Vector3.left * rotSpeed * Time.fixedDeltaTime);		
		} else if (Input.GetKey(KeyCode.Keypad2)/* && playerView.transform.rotation.x <= 90.0f*/) {
			playerView.transform.Rotate (Vector3.right * rotSpeed * Time.fixedDeltaTime);		
		}
		if (Input.GetKey(KeyCode.Keypad4)) {
			transform.Rotate (Vector3.forward * rotSpeed * Time.fixedDeltaTime);
		} else if (Input.GetKey(KeyCode.Keypad6)) {
			transform.Rotate (Vector3.back * rotSpeed * Time.fixedDeltaTime);
		}
	}


	void ChangeIncline () {
		if (myStance == Stances.standing) {
			Ray reachForWall = new Ray (transform.position, transform.forward);
			RaycastHit reachedWall;
			Debug.DrawRay (transform.position, transform.forward);

			if (Physics.Raycast (reachForWall, out reachedWall, 1.0f)) {
				//Climbing from floor(standing) to wall(crawling)
				if (reachedWall.collider.CompareTag("Wall")) {
					Debug.Log ("Press G to crawl onto the wall");
					if (Input.GetKeyDown (KeyCode.G)) {
						StickToSurface (-reachedWall.normal*9.8f);
						StartCoroutine (ChangingSurface (reachedWall, Vector3.Normalize (rb.transform.forward)));
						myStance = Stances.crawling;
					}
				} else if (reachedWall.collider.CompareTag("ExitDoor")) {
					Debug.Log ("Press R to open the door");
					if (Input.GetKeyDown(KeyCode.R)) {
/* Position player in front of door
 */
						StartCoroutine(MoveToDoor (reachedWall));
						StickToSurface (Vector3.down*9.8f);


/* Disable player movement/rotation Input and enable door Input
 * 
 * Offer a button input to allow player to step back away from door and resume movement/rotation/etc. in current room
 * 		Will this wipe away any failed attempts on the door, or are they persistent?
 */

					}
				}
			}

			Ray reachForFloor = new Ray (transform.position, -transform.up);
			RaycastHit reachedFloor;
			Debug.DrawRay (transform.position, -transform.up);

			if (Physics.Raycast (reachForFloor, out reachedFloor, 1.0f)) {
				//Input to Crawl
				if (Input.GetKeyDown (KeyCode.LeftControl)) {
					StartCoroutine (ChangingSurface (reachedFloor, Vector3.Normalize (rb.transform.forward)));
					myStance = Stances.crawling;
					Debug.Log ("Crawling");
				}
			}

		} else if (myStance == Stances.crawling) {
			Ray reachForSky = new Ray (transform.position, transform.up);
			RaycastHit reachedSky;
			Debug.DrawRay (transform.position, transform.up);

			if (Physics.Raycast (reachForSky, out reachedSky, 1.0f)) {
				if (reachedSky.collider.CompareTag("Wall") ||
					reachedSky.collider.CompareTag("Floor") ||
					reachedSky.collider.CompareTag("Ceiling")) {
					Debug.Log ("Press G to crawl onto the wall");
					if (Input.GetKeyDown (KeyCode.G)) {
						StickToSurface (-reachedSky.normal*9.8f);
						StartCoroutine (ChangingSurface (reachedSky, Vector3.Normalize (rb.transform.forward)));
					}
				} else if (reachedSky.collider.CompareTag("ExitDoor")) {
					Debug.Log ("Press R to open the door");
					if (Input.GetKeyDown(KeyCode.R)) {
						myStance = Stances.standing;
						StartCoroutine(MoveToDoor(reachedSky));
						StickToSurface (Vector3.down*9.8f);
					}
				}
			}

			Ray reachDown = new Ray (transform.position, transform.forward);
			RaycastHit reachedDown;
			Debug.DrawRay (transform.position, transform.forward);

			if (Physics.Raycast (reachDown, out reachedDown, 1.0f)) {
				//Input to Stand
				if (Input.GetKeyDown (KeyCode.LeftControl)) {
					if (reachedDown.collider.CompareTag ("Floor")) {
						StartCoroutine (ChangingSurface (reachedDown, Vector3.Normalize (-rb.transform.up)));
						myStance = Stances.standing;
						Debug.Log ("Standing");
					} else if (reachedDown.collider.CompareTag ("Ceiling")) {
						StickToSurface (reachedDown.normal * 9.8f);
						StartCoroutine (ChangingSurface (reachedDown, Vector3.Normalize (rb.transform.up)));
						myStance = Stances.standing;
						Debug.Log ("Dropping from ceiling");
					}
				}
			}
		}
	}


//Merge with ChangingSurface (Make sure to pass in RaycastHit)
	void StickToSurface (Vector3 gravChange) {
		gravity = gravChange;
		Physics.gravity = gravity;
	}


	IEnumerator ChangingSurface(RaycastHit reachedSurface, Vector3 normalBody) {
		//for (float t = 0; t < 1; t += Time.fixedDeltaTime) {
			Quaternion lerpEnd = Quaternion.FromToRotation (normalBody, -reachedSurface.normal) * rb.rotation;

//Change t from 1.0f to the magnitude of the combined angles of rotation, multiplied by Time.fixedDeltaTime
//Only once I find the solution do I reinstate this for-loop
//It's not Vector3.magnitude or Vector3.sqrMagnitude, at least as far as I can tell
		rb.MoveRotation (Quaternion.Slerp (rb.rotation, lerpEnd, 1.0f));

//Rotate (lerp) playerView toward the relative forward angle for ease of control
		/*Vector3 povNormal = Vector3.Normalize(playerView.transform.forward);
		Quaternion povLerpEnd = Quaternion.FromToRotation (povNormal, transform.forward) * transform.rotation;
		playerView.transform.rotation = Quaternion.Slerp (transform.rotation, povLerpEnd, 1.0f);
		*/

			yield return null;
		//}
	}


	IEnumerator MoveToDoor (RaycastHit reachedDoor) {
		for (float t = 0; t < 1; t += Time.fixedDeltaTime) {
			Vector3 targetPos = new Vector3 (reachedDoor.transform.position.x, transform.position.y, reachedDoor.transform.position.z - 1);
			transform.position = Vector3.Slerp (transform.position, targetPos, (90*t)/100);
			//Vector3.RotateTowards (transform.position, reachedDoor.transform.position, 90.0f, 1.0f);
			rb.MoveRotation (Quaternion.Slerp (rb.rotation, reachedDoor.transform.rotation, (90*t)/100));

			yield return null;
		}
	}

}
