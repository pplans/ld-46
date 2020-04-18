using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileInfo
{
	bool IsAvailable();
	void SetBorderColor(Color _c);
}

public class World : MonoBehaviour
{
	#region TileInfo
	private class TileInfo : ITileInfo
	{
		private bool m_bIsAvailable;
		private WorldTile m_tile;

		public TileInfo(bool _bAvailable, WorldTile _tile)
		{
			m_bIsAvailable = _bAvailable;
			m_tile = _tile;
		}
		public bool IsAvailable() { return m_bIsAvailable; }
		public void SetBorderColor(Color _c)
		{
			if (!IsAvailable() || m_tile == null) return;
			Material mat = m_tile.Tile.GetComponent<MeshRenderer>().material;
			mat.SetColor("Color_D10C4CBD", _c);
		}
	}
	#endregion

	#region WorldTile
	private class WorldTile
	{
		private GameObject tileObject;
		private WorldObject worldObject;

		public GameObject Tile { get => tileObject; set { tileObject = value; } }
		public WorldObject Object { get => worldObject;
			set
			{
				worldObject = value;
				worldObject.transform.parent = tileObject.transform;
			}
		}
	}
	#endregion

	#region World

	[SerializeField]
	private Vector2 TileStartPos = new Vector2(0, 0);
	[SerializeField]
	private Vector2 TileEndPos = new Vector2(10, 4);
	[SerializeField]
	private Vector2 TileSize = new Vector2(1, 1);

	[SerializeField]
	private GameObject WorldObject = null;

	[SerializeField]
	private GameObject tilePrefab = null;

	private bool m_bIsWorldInit;

	private Vector2 m_worldAnchor;

	private WorldTile[,] m_2dGrid;

	private static World s_Instance = null;

	public World Instance { get { if (s_Instance) s_Instance = new World(); return s_Instance; } }

	private World() => m_bIsWorldInit = false;

	public void Init()
	{
		if (m_bIsWorldInit) return;

		Vector2 gridSize = GetNumberOfTiles();
		m_2dGrid = new WorldTile[Mathf.RoundToInt(gridSize.x), Mathf.RoundToInt(gridSize.y)];
		for(int i = 0; i < gridSize.x; ++i)
		{
			for(int j = 0; j < gridSize.y; ++j)
			{
				m_2dGrid[i, j] = new WorldTile();
				Color rdrCol = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
				m_2dGrid[i, j].Tile = Instantiate(tilePrefab);
				Vector2 it = new Vector2(i, j);
				Vector2 pos = TileStartPos + it * TileSize;
				m_2dGrid[i, j].Tile.transform.position = new Vector3(pos.x, 0, pos.y);
				m_2dGrid[i, j].Tile.transform.parent = WorldObject.transform;
				Material mat = m_2dGrid[i, j].Tile.GetComponent<MeshRenderer>().material;
				mat.SetColor("Color_D10C4CBD", rdrCol);
			}
		}
		m_bIsWorldInit = true;
	}

	public bool ProjectToGrid(ref Vector2 pos)
	{
		Vector2 numberOfTiles = GetNumberOfTiles();
		bool bTileIsAvailable = pos.y < numberOfTiles.y && pos.y >= 0 && pos.x>=0 && pos.x < numberOfTiles.x;
		if (bTileIsAvailable)
		{
			pos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
		}
		return bTileIsAvailable;
	}

	public ITileInfo GetTileInfo(Vector2 pos, Vector2 dir)
	{
		Vector2 numberOfTiles = GetNumberOfTiles();
		Vector2 posInTile = ((pos + dir) / TileSize);
		bool bTileIsAvailable = ProjectToGrid(ref posInTile);
		WorldTile wo = null;
		if (bTileIsAvailable)
		{
			wo = m_2dGrid[Mathf.RoundToInt(posInTile.x), Mathf.RoundToInt(posInTile.y)];
			bTileIsAvailable = bTileIsAvailable && wo.Object == null;
		}
		return new TileInfo(bTileIsAvailable, wo);
	}

	public Vector2 GetNumberOfTiles()
	{
		return (TileEndPos - TileStartPos) / TileSize;
	}

	public class Backup { public WorldObject wo; public Vector2 pos; };
	public void SetWorldAnchor(Vector2 _anchor)
	{
		// if different from old world anchor, move stuff around
		{
			Vector2 direction = _anchor - m_worldAnchor;
			direction.y = 0;
			Vector2 gridSize = GetNumberOfTiles();
			bool bPlaced = false;
			List<Backup> backups = new List<Backup>();
			// first backup elements, then place them
			for (int i = 0; i < gridSize.x && !bPlaced; ++i)
			{
				for (int j = 0; j < gridSize.y && !bPlaced; ++j)
				{
					Vector2 newPos = new Vector2(i, j) + direction;
					if (m_2dGrid[i,j].Object && ProjectToGrid(ref newPos))
					{
						Backup bkp = new Backup();
						bkp.wo = m_2dGrid[i, j].Object;
						bkp.pos = new Vector2(i, j);
						backups.Add(bkp);
					}
				}
			}
			// place them
			foreach (Backup bkp in backups)
			{
				m_2dGrid[Mathf.RoundToInt(bkp.pos.x), Mathf.RoundToInt(bkp.pos.y)].Object = bkp.wo;
			}
		}
		m_worldAnchor = _anchor;
	}

	public void PlaceObject(WorldObject _wo)
	{
		Vector2 gridSize = GetNumberOfTiles();
		float threshold = 0.98f, step = 1f / (gridSize.x * gridSize.y);
		bool bPlaced = false;
		for (int i = 0; i < gridSize.x && !bPlaced; ++i)
		{
			for (int j = 0; j < gridSize.y && !bPlaced; ++j)
			{
				if (m_2dGrid[i, j].Object == null && Random.value > threshold)
				{
					m_2dGrid[i, j].Object = _wo;
					bPlaced = true;
				}
				else
					threshold = threshold - step;
			}
		}
	}

	public void PlaceObject(WorldObject _wo, Vector2 _pos)
	{
		Vector2 gridSize = GetNumberOfTiles();
		if (!ProjectToGrid(ref _pos))
			return;
		m_2dGrid[Mathf.RoundToInt(_pos.x), Mathf.RoundToInt(_pos.y)].Object = _wo;
	}

	public void OnDrawGizmos()
	{
		Vector2 NumberOfTiles = GetNumberOfTiles();
		for(int j = 0; j< NumberOfTiles.y +1; ++j)
		{
			Vector2 itHor = new Vector2(0, j);
			{
				Vector2 start = TileStartPos + itHor * TileSize;
				Vector2 end = new Vector2(TileEndPos.x, start.y);
				Debug.DrawLine(start, end);
			}
			for (int i = 0; i < NumberOfTiles.x + 1; ++i)
			{
				Vector2 itVer = new Vector2(i, 0);
				Vector2 start = TileStartPos + itVer * TileSize;
				Vector2 end = new Vector2(start.x, TileEndPos.y);
				Debug.DrawLine(start, end);
			}
		}
	}
#endregion
}
