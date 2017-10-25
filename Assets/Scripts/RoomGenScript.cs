using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomGenScript : MonoBehaviour {

	//SpawnRoom variables
	GameObject[] Planes;
	Vector3[] Surfaces;
	GameObject[] Doors;
	public List<int> surfaceList;
	int surfaceSpawn;
	Vector3 floorScale;
	float wallHeight;

	//SpawnCoins and SpawnTreasure variables
	[SerializeField] GameObject caseRef;
	[SerializeField] GameObject caseRef2;
	GameObject[] Cases;
	GameObject displayCase;
	[SerializeField] GameObject coinRef;
	GameObject coin;
	[SerializeField] GameObject treasureRef;
	[SerializeField] GameObject treasureRef2;
	GameObject[] Treasures;
	GameObject treasure;
	public List<GameObject> treasureList;

	//SpawnPlayer variables
	[SerializeField] GameObject playerObject;
	GameObject player;

	//SpawnNodes, SpawnReceivers, SpawnLasers variables
	GameObject laserParents;
	GameObject nodeParent;
	GameObject receiverParent;
	GameObject beamParent;
	[SerializeField] GameObject laserNode;
	public List<GameObject> Nodes;
	[SerializeField] GameObject laserReceiver;
	public List<GameObject> Receivers;
	[SerializeField] GameObject laser;
	GameObject nodePrefab;
	GameObject receiverPrefab;
	GameObject laserPrefab;

	[SerializeField] int spawnCount;
	int laserCountRemainder;


//


	void Awake () {
		SpawnRoom ();
		SpawnCoins ();
		SpawnTreasure ();
	}

	void Start () {
		SpawnPlayer ();

		surfaceList = new List<int> ();
		Nodes = new List<GameObject>();
		Receivers = new List<GameObject>();

		float roomSizeMag = new Vector3 (Planes [0].transform.localScale.x, Planes [1].transform.localScale.z, Planes [0].transform.localScale.z).magnitude * 10;
		spawnCount = (int)(roomSizeMag*2.0f);

		SpawnLaserParents ();
		SpawnNodes ();
		SpawnReceivers ();
		SpawnLasers ();

		//AlternateSpawnMethod ();
	}
	

	/*void AlternateSpawnMethod () {
		*Spawn a node/receiver pair each at a random position from the CubePositions list
		 * Then, have them lerp to an adjacent CubePositions position over time (1 second?)
		 * 		Make sure they don't move to the same position (or maybe even the same surface) at the same time
		 * lerp the position/rotation/scale of the laserbeam between them as they move
		 * Repeat constantly (turn this method into an IEnumerator)
		 * 
		 * The game view will need to be from a third-person perspective, then
		 *

		nodePrefab = Instantiate(laserNode, PathCubes[Random.Range(0, CubePositions.Count)].transform.position, Quaternion.identity);
		Nodes.Add (nodePrefab);
		receiverPrefab = Instantiate(laserReceiver, PathCubes[Random.Range(0, CubePositions.Count)].transform.position, Quaternion.identity);
		Receivers.Add (receiverPrefab);

		float laserScaleTotal = (Receivers [0].transform.position - Nodes [0].transform.position).magnitude;
		Vector3 laserPos = (Nodes [0].transform.position + Receivers [0].transform.position) / 2;

		laserPrefab = Instantiate (laser, laserPos, Quaternion.identity);

		laserPrefab.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, laserScaleTotal);
		laserPrefab.gameObject.transform.rotation = Quaternion.LookRotation (laserPrefab.transform.position - Receivers [0].transform.position);
	}
*/

	void Update () {
	}


	void SpawnRoom () {
		//Create a room by spawning and positioning primitive 3D shapes
//This is good for creating a cuboid room, but what if I want to make another shape?

		//Spawn Floor
		GameObject floor = GameObject.CreatePrimitive (PrimitiveType.Plane);
		floor.name = ("Floor");
		floor.tag = "Floor";
		floorScale = new Vector3 (Random.Range (0.8f, 1.5f), 1.0f, Random.Range (0.8f, 1.5f));
		floor.transform.localScale = floorScale;
		floor.transform.SetParent (gameObject.transform);
		Vector3 floorCol = floor.GetComponent<MeshCollider> ().bounds.extents;

		//Spawn Far (+z) Wall
		GameObject wall_1 = GameObject.CreatePrimitive (PrimitiveType.Plane);
		wall_1.name = ("Wall_1");
		wall_1.tag = "Wall";
		wallHeight = Random.Range (0.3f, 0.4f);
		wall_1.transform.localScale = new Vector3 (floorScale.x, 1f, wallHeight);
		wall_1.transform.Translate (0.0f, wallHeight * 5f, floorCol.z);
		wall_1.transform.Rotate (Vector3.left * 90.0f);
		wall_1.transform.SetParent (gameObject.transform);
		Vector3 wall_1Col = wall_1.GetComponent<MeshCollider> ().bounds.extents;

		//Spawn Near (-z) Wall
		GameObject wall_2 = GameObject.CreatePrimitive (PrimitiveType.Plane);
		wall_2.name = ("Wall_2");
		wall_2.tag = "Wall";
		wall_2.transform.localScale = new Vector3 (floorScale.x, 1f, wallHeight);
		wall_2.transform.Translate (0.0f, wallHeight * 5f, -floorCol.z);
		wall_2.transform.Rotate (Vector3.right * 90.0f);
		wall_2.transform.SetParent (gameObject.transform);
		Vector3 wall_2Col = wall_2.GetComponent<MeshCollider> ().bounds.extents;

		//Spawn Left (-x) Wall
		GameObject wall_3 = GameObject.CreatePrimitive (PrimitiveType.Plane);
		wall_3.name = ("Wall_3");
		wall_3.tag = "Wall";
		wall_3.transform.localScale = new Vector3 (wallHeight, 1f, floorScale.z);
		wall_3.transform.Translate (-floorCol.x, wallHeight * 5f, 0.0f);
		wall_3.transform.Rotate (Vector3.back * 90.0f);
		wall_3.transform.SetParent (gameObject.transform);
		Vector3 wall_3Col = wall_3.GetComponent<MeshCollider> ().bounds.extents;

		//Spawn Right (+x) Wall
		GameObject wall_4 = GameObject.CreatePrimitive (PrimitiveType.Plane);
		wall_4.name = ("Wall_4");
		wall_4.tag = "Wall";
		wall_4.transform.localScale = new Vector3 (wallHeight, 1f, floorScale.z);
		wall_4.transform.Translate (floorCol.x, wallHeight * 5f, 0.0f);
		wall_4.transform.Rotate (Vector3.forward * 90.0f);
		wall_4.transform.SetParent (gameObject.transform);
		Vector3 wall_4Col = wall_4.GetComponent<MeshCollider> ().bounds.extents;

		//Spawn Ceiling
		GameObject ceiling = GameObject.CreatePrimitive (PrimitiveType.Plane);
		ceiling.name = ("Ceiling");
		ceiling.tag = "Ceiling";
		ceiling.transform.localScale = floorScale;
		ceiling.transform.Translate (0.0f, wallHeight * 10, 0.0f);
		ceiling.transform.Rotate (Vector3.forward * 180.0f);
		ceiling.transform.SetParent (gameObject.transform);
		Vector3 ceilingCol = ceiling.GetComponent<MeshCollider> ().bounds.extents;

		//Spawn Entry Door
		GameObject door_1 = GameObject.CreatePrimitive (PrimitiveType.Cube);
		door_1.name = ("EntryDoor");
		door_1.tag = "EntryDoor";
		Vector3 doorScale = new Vector3 (1.0f, 2.5f, 0.2f);
		door_1.transform.localScale = doorScale;
		door_1.transform.Translate (0.0f, 1.25f, -floorScale.z * 5);
		door_1.transform.SetParent (gameObject.transform);
		BoxCollider door_1Col = door_1.GetComponent<BoxCollider> ();
		door_1Col.size = new Vector3 (2.0f, 1.0f, 10.0f);
		door_1Col.isTrigger = true;
		door_1.AddComponent<BoxCollider> ();

		//Spawn Exit Door
		GameObject door_2 = GameObject.CreatePrimitive (PrimitiveType.Cube);
		door_2.name = ("ExitDoor");
		door_2.tag = "ExitDoor";
		door_2.transform.localScale = doorScale;
		door_2.transform.Translate (0.0f, 1.25f, floorScale.z * 5);
		door_2.transform.SetParent (gameObject.transform);
		BoxCollider door_2Col = door_2.GetComponent<BoxCollider> ();
		door_2Col.size = new Vector3 (3.0f, 1.0f, 15.0f);
		door_2Col.isTrigger = true;
		door_2.AddComponent<BoxCollider> ();

		//Initialize Arrays
		Planes = new GameObject[] { floor, wall_1, wall_2, wall_3, wall_4, ceiling };
		Surfaces = new Vector3[] { floorCol, wall_1Col, wall_2Col, wall_3Col, wall_4Col, ceilingCol };
		Doors = new GameObject[] { door_1, door_2 };
	}


	void SpawnPlayer () {
		Vector3 playerSpawn = new Vector3 (0.0f, 1f, (-Planes[0].transform.localScale.z * 5) + 1);
		player = Instantiate (playerObject, playerSpawn, Quaternion.identity);
		player.name = ("Player");
	}


	void SpawnCoins () {
		GameObject treasureParent = new GameObject();
		treasureParent.name = "TreasureParent";
		for (int i = 0; i < spawnCount/10; i++) {
			Vector3 coinSpawn = new Vector3 (Random.Range (-Surfaces [0].x + 0.5f, Surfaces [0].x - 0.5f), Surfaces [0].y + 0.5f, Random.Range (-Surfaces [0].z + 0.5f, Surfaces [0].z - 0.5f));
			coin = Instantiate (coinRef, coinSpawn, Quaternion.Euler(Vector3.right * 90));
			coin.tag = "Coin";
			coin.transform.SetParent (treasureParent.transform);
			treasureList.Add (coin);
		}
	}


	void SpawnTreasure () {
		Cases = new GameObject[] {caseRef, caseRef2};
		int whichCase = Random.Range (0, 2);
		Vector3 caseSpawn = new Vector3 (Random.Range (-Surfaces [0].x + 0.5f, Surfaces [0].x - 0.5f), Surfaces [0].y, Random.Range (-Surfaces [0].z + 0.5f, Surfaces [0].z - 0.5f));
		displayCase = Instantiate (Cases[whichCase], caseSpawn, Quaternion.Euler(Vector3.left * 90));


		Treasures = new GameObject[] {treasureRef, treasureRef2};
		int whichTreasure = Random.Range (0, 2);
		Vector3 treasureSpawn = Vector3.zero;
		if (whichCase == 0) {
			treasureSpawn = new Vector3 (caseSpawn.x, caseSpawn.y + 1.325f, caseSpawn.z);
		} else if (whichCase == 1) {
			treasureSpawn = new Vector3 (caseSpawn.x, caseSpawn.y + 0.6f, caseSpawn.z);
		} else {
			treasureSpawn = new Vector3 (caseSpawn.x, caseSpawn.y, caseSpawn.z);
		}
		treasure = Instantiate (Treasures[whichTreasure], treasureSpawn, Quaternion.Euler(Vector3.left * 90));
		treasureList.Add (treasure);
	}


	void SpawnLaserParents () {
		//Setting up Parent GameObject for Parent GameObjects
		laserParents = new GameObject();
		laserParents.name = "LaserParents";

		//Setting up Parent GameObject for Nodes
		nodeParent = new GameObject();
		nodeParent.name = "NodeParent";
		nodeParent.transform.SetParent (laserParents.transform);

		//Setting up Parent GameObject for Receivers
		receiverParent = new GameObject();
		receiverParent.name = "ReceiverParent";
		receiverParent.transform.SetParent (laserParents.transform);

		//Setting up Parent GameObject for Lasers
		beamParent = new GameObject();
		beamParent.name = "LaserParent";
		beamParent.transform.SetParent (laserParents.transform);
	}


	void SpawnNodes () {
		//Spawning Nodes at random locations on ceiling, floor, and walls
		for (int i = 0; i < spawnCount; i++) {
			Vector3[] roomSurfaces = new Vector3[] {
				/*Floor*/new Vector3(Random.Range (-Surfaces[0].x, Surfaces[0].x), Surfaces[0].y, Random.Range (-Surfaces[0].z, Surfaces[0].z)),
				/*zWall*/new Vector3(Random.Range (-Surfaces[1].x, Surfaces[1].x), Random.Range(0.0f, Surfaces[1].y * 2), Planes[1].transform.position.z),
				/*-zWall*/new Vector3(Random.Range (-Surfaces[2].x, Surfaces[2].x), Random.Range(0.0f, Surfaces[2].y * 2), Planes[2].transform.position.z),
				/*-xWall*/new Vector3(Planes[3].transform.position.x, Random.Range(0.0f, Surfaces[3].y*2), Random.Range (-Surfaces[3].z, Surfaces[3].z)),
				/*xWall*/new Vector3(Planes[4].transform.position.x, Random.Range(0.0f, Surfaces[4].y*2), Random.Range (-Surfaces[4].z, Surfaces[4].z)),
				/*Ceiling*/new Vector3(Random.Range (-Surfaces[5].x, Surfaces[5].x), Planes[5].transform.position.y, Random.Range (-Surfaces[5].z, Surfaces[5].z))
			};
		
			surfaceSpawn = Random.Range (0, roomSurfaces.Length);
			surfaceList.Add (surfaceSpawn);

			nodePrefab = Instantiate(laserNode, roomSurfaces[surfaceSpawn], Quaternion.identity, nodeParent.transform);
			Nodes.Add(nodePrefab);
		}
	}


	void SpawnReceivers () {
		//Spawning Receivers at random locations on ceiling, floor, and walls
		for (int i = 0; i < spawnCount; i++) {
			Vector3[] roomSurfaces = new Vector3[] {
				/*Floor*/new Vector3 (Random.Range (-Surfaces [0].x, Surfaces [0].x), Surfaces [0].y, Random.Range (-Surfaces [0].z, Surfaces [0].z)),
				/*zWall*/new Vector3 (Random.Range (-Surfaces [1].x, Surfaces [1].x), Random.Range (0.0f, Surfaces [1].y * 2), Planes [1].transform.position.z),
				/*-zWall*/new Vector3 (Random.Range (-Surfaces [2].x, Surfaces [2].x), Random.Range (0.0f, Surfaces [2].y * 2), Planes [2].transform.position.z),
				/*-xWall*/new Vector3 (Planes [3].transform.position.x, Random.Range (0.0f, Surfaces [3].y * 2), Random.Range (-Surfaces [3].z, Surfaces [3].z)),
				/*xWall*/new Vector3 (Planes [4].transform.position.x, Random.Range (0.0f, Surfaces [4].y * 2), Random.Range (-Surfaces [4].z, Surfaces [4].z)),
				/*Ceiling*/new Vector3 (Random.Range (-Surfaces [5].x, Surfaces [5].x), Planes [5].transform.position.y, Random.Range (-Surfaces [5].z, Surfaces [5].z))
			};

			surfaceSpawn = Random.Range (0, roomSurfaces.Length);

			//Don't spawn the Receiver on the same surface as its paired Node
			if (surfaceSpawn == surfaceList [i]) {
				//Debug.Log ("Rolled the same #");
				if (surfaceSpawn == 0) {
					surfaceSpawn = Random.Range (1, 6);
				} else if (surfaceSpawn == roomSurfaces.Length - 1) {
					surfaceSpawn = Random.Range (0, 5);
				} else {
					//This solution is a bit weighted. Is there a better, more random solution?
					surfaceSpawn = surfaceSpawn + -1;
				}
			}
			receiverPrefab = Instantiate(laserReceiver, roomSurfaces[surfaceSpawn], Quaternion.identity, receiverParent.transform);
			Receivers.Add(receiverPrefab);
		}
		surfaceList.Clear ();
	}


	void SpawnLasers () {
		//Spawning Laser Beams between Nodes and Receivers
		for (int i = 0; i < spawnCount; i++) {
			float laserScaleTotal = (Receivers [0].transform.position - Nodes [0].transform.position).magnitude;
			Vector3 laserPos = (Nodes [0].transform.position + Receivers [0].transform.position) / 2;

			laserPrefab = Instantiate (laser, laserPos, Quaternion.identity, beamParent.transform);

			laserPrefab.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, laserScaleTotal);
			laserPrefab.gameObject.transform.rotation = Quaternion.LookRotation (laserPrefab.transform.position - Receivers [0].transform.position);

			if (laserPrefab.GetComponent<BoxCollider> ().bounds.Intersects (player.GetComponent<CapsuleCollider> ().bounds)
			    || laserPrefab.GetComponent<BoxCollider> ().bounds.Intersects (Doors [0].GetComponent<BoxCollider> ().bounds)
			    || laserPrefab.GetComponent<BoxCollider> ().bounds.Intersects (Doors [1].GetComponent<BoxCollider> ().bounds)
				//||treasureList.ForEach(gameObject.GetComponent<CapsuleCollider>().bounds.Intersects)
				//|| PathCubeBounds.Any(b => b.Intersects(laserPrefab.GetComponent<BoxCollider> ().bounds))
			) {
				Destroy (laserPrefab);
				Destroy (Nodes [0]);
				Destroy (Receivers [0]);

				Nodes.Add (nodePrefab);
				Receivers.Add (receiverPrefab);

				laserCountRemainder++;
			}

			Nodes [0].transform.LookAt (Receivers [0].transform.position);
			Receivers [0].transform.LookAt (Nodes [0].transform.position);


			Nodes.RemoveAt (0);
			Receivers.RemoveAt (0);
		}

		spawnCount = laserCountRemainder;
		laserCountRemainder = 0;

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
	}

}
