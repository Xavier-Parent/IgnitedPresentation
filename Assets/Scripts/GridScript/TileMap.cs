using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour 
{
	[SerializeField]
	private GameObject selectedUnit;
	[SerializeField]
	private List<GameObject> availableUnits;
	[SerializeField]
	private TileType[] tileTypes;

	int[,] tiles;
	Node[,] graph;

	[SerializeField]
	private int m_Width = 10;
	[SerializeField]
	private int m_Height = 10;
	public CombatCameraControl cameraController;
	void Start() {
		if (availableUnits.Count > 0)
		{
			selectedUnit = availableUnits[0];
		}
		// Setup the selectedUnit's variable
		selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
		selectedUnit.GetComponent<Unit>().tileY = (int)selectedUnit.transform.position.z;
		selectedUnit.GetComponent<Unit>().map = this;

		GenerateMapData();
		GenerateMapVisual();
		GeneratePathfindingGraph();
	}

	void GenerateMapData() {
		// Allocate our map tiles
		tiles = new int[m_Width,m_Height];
		
		int x,y;
		
		// Initialize our map tiles to be grass
		for(x=0; x < m_Width; x++) {
			for(y=0; y < m_Height; y++) {
				Collider[] colliders = Physics.OverlapSphere(new Vector3(x, -1, y), 0.1f);
				tiles[x,y] = 0;
			}
		}

	}

	public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY) {

		TileType tt = tileTypes[ tiles[targetX,targetY] ];

		if(UnitCanEnterTile(targetX, targetY) == false)
			return Mathf.Infinity;

		float cost = tt.movementCost;

		if( sourceX!=targetX && sourceY!=targetY) {
			// We are moving diagonally!  Fudge the cost for tie-breaking
			// Purely a cosmetic thing!
			cost += 0.001f;
		}
		return cost;
	}
	
	void GeneratePathfindingGraph() {
		// Initialize the array
		graph = new Node[m_Width,m_Height];

		// Initialize a Node for each spot in the array
		for(int x=0; x < m_Width; x++) {
			for(int y=0; y < m_Height; y++) {
				graph[x,y] = new Node();
				graph[x,y].x = x;
				graph[x,y].y = y;
			}
		}

		// Now that all the nodes exist, calculate their neighbours
		for(int x=0; x < m_Width; x++) {
			for(int y=0; y < m_Height; y++) {

				// This is the 4-way connection version:
				if (x > 0)
					graph[x,y].neighbours.Add( graph[x-1, y] );
				if(x < m_Width-1)
					graph[x,y].neighbours.Add( graph[x+1, y] );
				if(y > 0)
					graph[x,y].neighbours.Add( graph[x, y-1] );
				if(y < m_Height-1)
					graph[x,y].neighbours.Add( graph[x, y+1] );	
			}
		}
	}
	

	void GenerateMapVisual() {

		for (int x = 0; x < m_Width; x ++)
		{
			for (int y = 0; y < m_Height; y ++)
			{
				Vector3 spawnPosition = transform.position + new Vector3(x, -0.55f, y);
				// Check if there is an object already at the spawn position
				Collider[] colliders = Physics.OverlapSphere(spawnPosition, 0.2f);
				if (colliders.Length > 0)
				{
					// There is an object at the spawn position, so skip this iteration
					continue;
				}
				TileType tt = tileTypes[tiles[x, y]];
				GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, spawnPosition, Quaternion.identity);

				ClickableTile ct = go.GetComponent<ClickableTile>();
				ct.tileX = x;
				ct.tileY = y;
				ct.map = this;
				var m_Offset = (x % 2 == 0 && y % 2 != 0 || x % 2 != 0 && y % 2 == 0);
				ct.OffsetInit(m_Offset);
			}
		}
	}

	public Vector3 TileCoordToWorldCoord(int x, int y) {
		return new Vector3(x, 0.25f, y);
	}

	public bool UnitCanEnterTile(int x, int y) {

		// We could test the unit's walk/hover/fly type against various
		// terrain flags here to see if they are allowed to enter the tile.
		Collider[] colliders = Physics.OverlapSphere(new Vector3(x, 0, y), 0.2f);
		if (colliders.Length > 0)
		{
			// There is an obstacle at the specified tile position
			return false;
		}
		return tileTypes[ tiles[x,y] ].isWalkable;
	}

	public void GeneratePathTo(int x, int y) {
		// Clear out our unit's old path.
		selectedUnit.GetComponent<Unit>().currentPath = null;

		if( UnitCanEnterTile(x,y) == false ) {
			// We probably clicked on a mountain or something, so just quit out.
			return;
		}

		Dictionary<Node, float> dist = new Dictionary<Node, float>();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

		// Setup the "Q" -- the list of nodes we haven't checked yet.
		List<Node> unvisited = new List<Node>();
		
		Node source = graph[
		                    selectedUnit.GetComponent<Unit>().tileX, 
		                    selectedUnit.GetComponent<Unit>().tileY
		                    ];
		
		Node target = graph[
		                    x, 
		                    y
		                    ];
		
		dist[source] = 0;
		prev[source] = null;

		// Initialize everything to have INFINITY distance, since
		// we don't know any better right now. Also, it's possible
		// that some nodes CAN'T be reached from the source,
		// which would make INFINITY a reasonable value
		foreach(Node v in graph) {
			if(v != source) {
				dist[v] = Mathf.Infinity;
				prev[v] = null;
			}

			unvisited.Add(v);
		}

		while(unvisited.Count > 0) {
			// "u" is going to be the unvisited node with the smallest distance.
			Node u = null;

			foreach(Node possibleU in unvisited) {
				if(u == null || dist[possibleU] < dist[u]) {
					u = possibleU;
				}
			}

			if(u == target) {
				break;	// Exit the while loop!
			}

			unvisited.Remove(u);

			foreach(Node v in u.neighbours) {
				//float alt = dist[u] + u.DistanceTo(v);
				float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
				if( alt < dist[v] ) {
					dist[v] = alt;
					prev[v] = u;
				}
			}
		}

		// If we get there, the either we found the shortest route
		// to our target, or there is no route at ALL to our target.

		if(prev[target] == null) {
			// No route between our target and the source
			return;
		}

		List<Node> currentPath = new List<Node>();

		Node curr = target;

		// Step through the "prev" chain and add it to our path
		while(curr != null) {
			currentPath.Add(curr);
			curr = prev[curr];
		}

		// Right now, currentPath describes a route from out target to our source
		// So we need to invert it!

		currentPath.Reverse();

		selectedUnit.GetComponent<Unit>().currentPath = currentPath;
	}
	public void SelectUnit(GameObject unit)
	{
		if (availableUnits.Contains(unit))
		{
			selectedUnit.GetComponent<Unit>().NextTurn();
			int currentIndex = availableUnits.IndexOf(selectedUnit);
			int nextIndex = (currentIndex + 1) % availableUnits.Count;
			selectedUnit = availableUnits[nextIndex];
			cameraController.SwitchCameraTarget(selectedUnit);
		}
	}

}
