using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomGenScript : MonoBehaviour {

	//SpawnRoom variables
	GameObject[] Planes;
	Vector3[] Surfaces;
	GameObject[] Doors;
	public List<GameObject> PathCubes;
	public List<GameObject> NextCubes;
	public List<GameObject> CubesForPath;
	public List<Vector3> CubePositions;
	public List<Vector3> CubePosNext;
	Vector3 nextNodePos;
	List<Bounds> PathCubeBounds;
	public List<int> surfaceList;
	int surfaceSpawn;
	Vector3 floorScale;
	float wallHeight;

	//SpawnCoins variables
	[SerializeField] GameObject coinRef;
	GameObject coin;

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


	void Awake () {
		SpawnRoom ();
		CreatePath();
		SpawnCoins ();
	}

	void Start () {
		SpawnPlayer ();

		surfaceList = new List<int> ();
		Nodes = new List<GameObject>();
		Receivers = new List<GameObject>();

		float roomSizeMag = new Vector3 (Planes [0].transform.localScale.x, Planes [1].transform.localScale.z, Planes [0].transform.localScale.z).magnitude * 10;
		spawnCount = (int)(roomSizeMag*2.0f);

		//SpawnLaserParents ();
		//SpawnNodes ();
		//SpawnReceivers ();
		//SpawnLasers ();

		AlternateSpawnMethod ();
	}
	

	void AlternateSpawnMethod () {
		/*Spawn a node/receiver pair each at a random position from the CubePositions list
		 * Then, have them lerp to an adjacent CubePositions position over time (1 second?)
		 * 		Make sure they don't move to the same position (or maybe even the same surface) at the same time
		 * lerp the position/rotation/scale of the laserbeam between them as they move
		 * Repeat constantly (turn this method into an IEnumerator)
		 * 
		 * The game view will need to be from a third-person perspective, then
		 */

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

	void Update () {
		StartCoroutine (MoveLasers ());
	}

	IEnumerator MoveLasers () {
		//Determine which positions are adjacent to the current position of Nodes[0] and configure a list (CubePosNext)
		foreach (var cube in PathCubes) {
			if (Vector3.Distance(cube.transform.position, Nodes[0].transform.position) <= 1.0f) {
				CubePosNext.Add (cube.transform.position);
			}
			/*if (cube.transform.position.x == Nodes[0].transform.position.x &&
				cube.transform.position.y <= Nodes[0].transform.position.y + 1.0f &&
				cube.transform.position.y >= Nodes[0].transform.position.y - 1.0f
				||cube.transform.position.x == Nodes[0].transform.position.x &&
				cube.transform.position.z <= Nodes[0].transform.position.z + 1.0f &&
				cube.transform.position.z >= Nodes[0].transform.position.z - 1.0f

				||cube.transform.position.y == Nodes[0].transform.position.y &&
				cube.transform.position.x <= Nodes[0].transform.position.x + 1.0f &&
				cube.transform.position.x >= Nodes[0].transform.position.x - 1.0f
				||cube.transform.position.y == Nodes[0].transform.position.y &&
				cube.transform.position.z <= Nodes[0].transform.position.z + 1.0f &&
				cube.transform.position.z >= Nodes[0].transform.position.z - 1.0f

				||cube.transform.position.z == Nodes[0].transform.position.z &&
				cube.transform.position.x <= Nodes[0].transform.position.x + 1.0f &&
				cube.transform.position.x >= Nodes[0].transform.position.x - 1.0f
				||cube.transform.position.z == Nodes[0].transform.position.z &&
				cube.transform.position.y <= Nodes[0].transform.position.y + 1.0f &&
				cube.transform.position.y >= Nodes[0].transform.position.y - 1.0f
			) {

				CubePosNext.Add (cube.transform.position);
			}*/
		}

		//Select one of the adjacent positions to move to
		if (CubePosNext.Count > 0) {
			nextNodePos = CubePosNext [Random.Range (0, CubePosNext.Count)];
		} else {
			nextNodePos = Nodes [0].transform.position;
		}

		//Lerp position of Nodes[0] to the new position
		for (float t = 0; t < 1.0f; t += Time.deltaTime) {
			Nodes [0].transform.position = Vector3.Lerp (Nodes [0].transform.position, nextNodePos, t/1000);

			//Adjust laser position, rotation, and scale as the node moves
			float laserScaleTotal = (Receivers [0].transform.position - Nodes [0].transform.position).magnitude;
			Vector3 laserPos = (Nodes [0].transform.position + Receivers [0].transform.position) / 2;
			laserPrefab.gameObject.transform.position = laserPos;
			laserPrefab.gameObject.transform.localScale = new Vector3 (0.01f, 0.01f, laserScaleTotal);
			laserPrefab.gameObject.transform.rotation = Quaternion.LookRotation (laserPrefab.transform.position - Receivers [0].transform.position);
		}
		CubePosNext.RemoveRange(0, CubePosNext.Count-1);

		yield return new WaitForSeconds (5.0f);
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


	void CreatePath () {
//Give cubes on path a tag which can be checked against the spawning nodes/receivers/lasers later to prevent overlapping

		//Creating a path through the room
		//Initialize Path Lists
		PathCubes = new List<GameObject> ();
		NextCubes = new List<GameObject> ();
		CubesForPath = new List<GameObject> ();
		CubePositions = new List<Vector3>();
		PathCubeBounds = new List<Bounds> ();

		//Spawn cubes along floor and add to PathCubes list
		for (float c = -floorScale.z*5; c < floorScale.z*5; c++) {
			for (float b = -floorScale.x*5; b < floorScale.x*5; b++) {
				GameObject pathCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				pathCube.transform.position = new Vector3 (b, 0.0f, c);
				PathCubes.Add (pathCube);
			}
		}
		//Spawn cubes along ceiling and add to PathCubes list
		for (float c = -floorScale.z*5; c < floorScale.z*5; c++) {
			for (float b = -floorScale.x*5; b < floorScale.x*5; b++) {
				GameObject pathCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				pathCube.transform.position = new Vector3 (b, wallHeight*10, c);
				PathCubes.Add (pathCube);
			}
		}
		//Spawn cubes along near wall and add to PathCubes list
		for (float a = -floorScale.x*5; a < floorScale.x*5; a++) {
			for (float b = 0; b < wallHeight*10; b++) {
				GameObject pathCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				pathCube.transform.position = new Vector3 (a, b, -floorScale.z*5);
				PathCubes.Add (pathCube);
			}
		}
		//Spawn cubes along left wall and add to PathCubes list
		for (float c = -floorScale.z*5; c < floorScale.z*5; c++) {
			for (float b = 0; b < wallHeight*10; b++) {
				GameObject pathCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				pathCube.transform.position = new Vector3 (-floorScale.x*5, b, c);
				PathCubes.Add (pathCube);
			}
		}
		//Spawn cubes along right wall and add to PathCubes list
		for (float c = -floorScale.z*5; c < floorScale.z*5; c++) {
			for (float b = 0; b < wallHeight*10; b++) {
				GameObject pathCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				pathCube.transform.position = new Vector3 (floorScale.x*5, b, c);
				PathCubes.Add (pathCube);
			}
		}
		//Spawn cubes along far wall and add to PathCubes list
		for (float a = -floorScale.x*5; a < floorScale.x*5; a++) {
			for (float b = 0; b < wallHeight*10; b++) {
				GameObject pathCube = GameObject.CreatePrimitive (PrimitiveType.Cube);
				pathCube.transform.position = new Vector3 (a, b, floorScale.z*5);
				PathCubes.Add (pathCube);
			}
		}


		//Find which cubes are positioned near the Entry Door and add them to the NextCubes list
		foreach (var cube in PathCubes) {
			cube.GetComponent<MeshRenderer> ().enabled = false;
			cube.GetComponent<BoxCollider> ().isTrigger = true;
			if (Doors[0].GetComponent<BoxCollider>().bounds.Contains(cube.transform.position)) {
				NextCubes.Add (cube);
			}
		}

		//Start the path at the location of one of the cubes near the Entry Door
		//And add it to the relevant lists
		GameObject startCube = NextCubes [Random.Range (0, NextCubes.Count)];
		CubePositions.Add (startCube.transform.position);
		CubesForPath.Add (startCube);
		PathCubeBounds.Add(startCube.GetComponent<BoxCollider>().bounds);

		//Determine which cubes from PathCubes are touching startCube and reuse/repopulate NextCubes
		NextCubes.Clear ();
		foreach (var cube in PathCubes) {
			if (startCube.GetComponent<BoxCollider>().bounds.Intersects(cube.GetComponent<BoxCollider>().bounds)) {
				NextCubes.Add(cube);
			}
		}

		//Repeat previous step until a path is formed from Entry Door to Exit Door
		KeepGoingWithPath ();

	}


	void KeepGoingWithPath () {
		//Continue building path by selecting a random cube touching the currently selected cube
		//Continuing from startCube, determine which of the cubes touching it will be next on the path
//This "if statement" may not be necessary
		if (NextCubes.Count > 0) {
			GameObject nextCube = NextCubes [Random.Range (0, NextCubes.Count)];
			NextCubes.Clear ();
			CubePositions.Add (nextCube.transform.position);

			//Determine which nearby cubes to choose from when adding to path
			foreach (var cube in PathCubes) {
				//If a cube is touching the current cube...
				if (nextCube.GetComponent<BoxCollider> ().bounds.Intersects (cube.GetComponent<BoxCollider> ().bounds)) {
					//...and its position is not already on the CubePositions list (preventing backtracking of path)...
					///...and its position would move the path forward...
					if (!CubePositions.Contains (cube.transform.position) && 
						cube.transform.position.z >= nextCube.transform.position.z) {
						//...then add it to Nextcubes
						NextCubes.Add (cube);
					}
				}
			}

			//When the path reaches the Exit Door, it is done
			if (nextCube.transform.position.z >= floorScale.z*5) {
				//Clear the room of all superfluous cubes...
				/*Debug.Log(PathCubes.Count);
				foreach (var cube in PathCubes) {
					Destroy (cube);
				}
				PathCubes.Clear ();
*/
				NextCubes.Clear ();

				//...but keep the ones used to create the path
				foreach (var position in CubePositions) {
					GameObject yellowBrick = GameObject.CreatePrimitive (PrimitiveType.Cube);
					yellowBrick.transform.position = position;
					CubesForPath.Add (yellowBrick);
					PathCubeBounds.Add (yellowBrick.GetComponent<BoxCollider> ().bounds);
				}
				foreach (var pathCube in CubesForPath) {
					pathCube.name = "PathCube";
					//pathCube.tag = "PathCube";
					Vector3 cubeColSize = pathCube.GetComponent<BoxCollider> ().bounds.extents;
					cubeColSize = new Vector3 (0.1f, 0.1f, 0.1f);
					pathCube.GetComponent<BoxCollider> ().isTrigger = true;
				}
			} else {
				//If the path has yet to reach the Exit Door, repeat the selection process and continue building
				KeepGoingWithPath ();
			}
		}
	}


	void SpawnPlayer () {
		Vector3 playerSpawn = new Vector3 (0.0f, 1f, (-Planes[0].transform.localScale.z*5) + 1);
		player = Instantiate (playerObject, playerSpawn, Quaternion.identity);
		player.name = ("Player");
	}


	void SpawnCoins () {
		GameObject coinParent = new GameObject();
		coinParent.name = "CoinParent";
		for (int i = 0; i < spawnCount/10; i++) {
			Vector3 coinSpawn = new Vector3 (Random.Range (-Surfaces [0].x + 0.5f, Surfaces [0].x - 0.5f), Surfaces [0].y + 0.5f, Random.Range (-Surfaces [0].z + 0.5f, Surfaces [0].z - 0.5f));
			coin = Instantiate (coinRef, coinSpawn, Quaternion.Euler(Vector3.right * 90));
			coin.tag = "Coin";
			coin.transform.SetParent (coinParent.transform);
		}
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
				//|| PathCubeBounds.Any(b => b.Intersects(laserPrefab.GetComponent<BoxCollider> ().bounds))
			) {
				Destroy (laserPrefab);
				Destroy (Nodes [0]);
				Destroy (Receivers [0]);

				Nodes.Add (nodePrefab);
				Receivers.Add (receiverPrefab);

				laserCountRemainder++;
			}
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
