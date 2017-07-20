using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {

	//Component References
	MeshCollider roomCol;
	Vector3 roomOffset;

	//Room Bounds Variables
	Vector3[] roomSurfaces;
	int surfaceSpawn;
	public List<int> surfaceList;

	//Variable References
	[SerializeField]
	int spawnCount;
	[SerializeField]
	int laserCount = 40;
	[SerializeField]
	int laserCountRemainder = 0;

	Vector3 laserPos;
	Quaternion laserRot;
	float laserScaleTotal;

	//GameObject References
	GameObject Player;
	Bounds playerBounds;
	Bounds laserBounds;
	
	//Laser Spawn References and Lists
	[SerializeField]
	GameObject laserNode;
	public List<GameObject> Nodes;
	[SerializeField]
	GameObject laserReceiver;
	public List<GameObject> Receivers;
	[SerializeField]
	GameObject laser;

	//SPECIFIC Laser Spawn References
	GameObject nodePrefab;
	GameObject receiverPrefab;
	GameObject laserPrefab;


	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		roomCol = GetComponent<MeshCollider> ();
		roomOffset = gameObject.transform.position;

		spawnCount = laserCount;

		SpawnNodes ();
		SpawnReceivers ();
		SpawnLasers ();
	}
	

	void Update () {
		
	}


// The goal here is to be able to have a scene with just a single Empty GameObject, and have the environment build itself
	void SpawnRoom () {
		/* Spawn Floor, Walls, and Ceiling planes
		 * Position, rotate, scale, name and tag each of them
		 * Make sure they each have a collider, material, etc.
		 * Overwrite them into the roomSurfaces array for use in SpawnNodes() and SpawnReceivers()
		 * 
		 * Maybe later, spawn more of each to create more complex room shapes/layouts
		 * Maybe later later, turn room layout into a sort of maze of rooms and doors
		 */
	}


	void SpawnPlayer () {
		/*Spawn the Player GameObject and declare its name, position, rotation, scale
		Add Rigidbody, PlayerMoveScript, and other necessary components
		Don't forget to tag as "Player"
		*/
	}
		

	void SpawnNodes () {
		//Spawning Nodes at random locations on ceiling, floor, and walls
		for (int i = 0; i < spawnCount; i++) {
			roomSurfaces = new Vector3[] {
				/*Ceiling*/new Vector3(Random.Range (-roomCol.bounds.extents.x, roomCol.bounds.extents.x), roomOffset.y + roomCol.bounds.extents.y, Random.Range (-roomCol.bounds.extents.z, roomCol.bounds.extents.z)),
				/*Floor*/new Vector3(Random.Range (-roomCol.bounds.extents.x, roomCol.bounds.extents.x), roomOffset.y - roomCol.bounds.extents.y, Random.Range (-roomCol.bounds.extents.z, roomCol.bounds.extents.z)),
				/*zWalls*/new Vector3(Random.Range (-roomCol.bounds.extents.x, roomCol.bounds.extents.x), Random.Range(0.0f, roomCol.bounds.extents.y * 2), roomCol.bounds.extents.z),
				/*-zWalls*/new Vector3(Random.Range (-roomCol.bounds.extents.x, roomCol.bounds.extents.x), Random.Range(0.0f, roomCol.bounds.extents.y * 2), -roomCol.bounds.extents.z),
				/*xWalls*/new Vector3(roomCol.bounds.extents.x, Random.Range(roomOffset.y, roomOffset.y + roomCol.bounds.extents.y), Random.Range (-roomCol.bounds.extents.z, roomCol.bounds.extents.z)),
				/*-xWalls*/new Vector3(-roomCol.bounds.extents.x, Random.Range(roomOffset.y, roomOffset.y + roomCol.bounds.extents.y), Random.Range (-roomCol.bounds.extents.z, roomCol.bounds.extents.z))};
			surfaceSpawn = Random.Range (0, 6);
			surfaceList.Add (surfaceSpawn);

			nodePrefab = Instantiate(laserNode, roomSurfaces[surfaceSpawn], Quaternion.LookRotation(roomCol.bounds.center));
			Nodes.Add(nodePrefab);
		}
	}

	void SpawnReceivers () {
		//Spawning Receivers at random locations on ceiling, floor, and walls
		for (int i = 0; i < spawnCount; i++) {
			roomSurfaces = new Vector3[] {
				/*Ceiling*/new Vector3(Random.Range (-roomCol.bounds.extents.x, roomCol.bounds.extents.x), roomOffset.y + roomCol.bounds.extents.y, Random.Range (-roomCol.bounds.extents.z, roomCol.bounds.extents.z)),
				/*Floor*/new Vector3(Random.Range (-roomCol.bounds.extents.x, roomCol.bounds.extents.x), roomOffset.y - roomCol.bounds.extents.y, Random.Range (-roomCol.bounds.extents.z, roomCol.bounds.extents.z)),
				/*zWalls*/new Vector3(Random.Range (-roomCol.bounds.extents.x, roomCol.bounds.extents.x), Random.Range(0.0f, roomCol.bounds.extents.y * 2), roomCol.bounds.extents.z),
				/*-zWalls*/new Vector3(Random.Range (-roomCol.bounds.extents.x, roomCol.bounds.extents.x), Random.Range(0.0f, roomCol.bounds.extents.y * 2), -roomCol.bounds.extents.z),
				/*xWalls*/new Vector3(roomCol.bounds.extents.x, Random.Range(roomOffset.y, roomOffset.y + roomCol.bounds.extents.y), Random.Range (-roomCol.bounds.extents.z, roomCol.bounds.extents.z)),
				/*-xWalls*/new Vector3(-roomCol.bounds.extents.x, Random.Range(roomOffset.y, roomOffset.y + roomCol.bounds.extents.y), Random.Range (-roomCol.bounds.extents.z, roomCol.bounds.extents.z))};
			surfaceSpawn = Random.Range (0, 6);
		
			//Don't spawn the Receiver on the same surface as its paired Node
			if (surfaceSpawn == surfaceList[i]) {
				//Debug.Log ("Rolled the same #");
				if (surfaceSpawn == 0) {
					surfaceSpawn = Random.Range (1, 6);
				} else if (surfaceSpawn == 5) {
					surfaceSpawn = Random.Range (0, 5);
				} else {
//This solution is a bit weighted. Is there a better, more random solution?
					surfaceSpawn = surfaceSpawn +- 1;
				}

/*ALTERNATE SOLUTION, KEEPING IF NEEDED LATER:
 * If the number is not different by this point, then...
 * Destroy the receiver
 * Add 1 to an int variable for the # of receivers destroyed this way
 * Set spawnCount equal to the new variable
 * Run SpawnReceivers() again until everything is equalized
 * (Since the Receiver in the next index will move down(or won't?), what if it is the same number?)
 * Also, I won't be able to use the surfaceList[i] reference
 * Set spawnCount back to its initial value, so that the lasers below spawn the correct number
 */
			}
			receiverPrefab = Instantiate(laserReceiver, roomSurfaces[surfaceSpawn], Quaternion.LookRotation(roomCol.bounds.center));
			Receivers.Add(receiverPrefab);
		}
		surfaceList.Clear ();
	}

	void SpawnLasers () {
		//Spawning Laser Beams between Nodes and Receivers
		for (int i = 0; i < spawnCount; i++) {
			//if (Nodes.Count > 0 && Receivers.Count > 0) {
				laserScaleTotal = (Receivers [0].transform.position - Nodes [0].transform.position).magnitude;
				laserPos = (Nodes [0].transform.position + Receivers [0].transform.position) / 2;
				laserRot = Quaternion.identity;

				laserPrefab = Instantiate (laser, laserPos, laserRot);

				laserPrefab.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, laserScaleTotal);
				laserPrefab.gameObject.transform.rotation = Quaternion.LookRotation (laserPrefab.transform.position - Receivers [0].transform.position);

				if (laserPrefab.GetComponent<BoxCollider> ().bounds.Intersects (Player.GetComponent<CapsuleCollider> ().bounds)) {
					//Debug.Log ("Laser spawned inside Player" + laserPrefab.transform.position);

					Destroy (laserPrefab);
					Destroy (Nodes [0]);
					Destroy (Receivers [0]);

					Nodes.Add (nodePrefab);
					Receivers.Add (receiverPrefab);

					laserCountRemainder++;
				}
				Nodes.RemoveAt (0);
				Receivers.RemoveAt (0);

				//yield return new WaitForSeconds (0.5f);
			//}
		}

		spawnCount = laserCountRemainder;
		laserCountRemainder = 0;
		//Debug.Log ("spawnCount: " + spawnCount);
		//Debug.Log ("Nodes.Count: " + Nodes.Count);

		if (spawnCount > 0) {
			Nodes.Clear ();
			Receivers.Clear ();
			SpawnRetry ();
		}
	}


	void SpawnRetry () {
		SpawnNodes ();
		SpawnReceivers ();
		SpawnLasers ();
		//StartCoroutine("SpawnLasers");
	}
		

}
