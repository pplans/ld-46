using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum TileState
{
	Empty,
	Ennemy,
	Occupied,
	Player,
	BorderRight,
	Border
}

public interface ITileInfo
{
	void SetBorderColor(Color _c);
	void SetEmissiveScale(float _v);
	void SetVisited();
	TileState GetState();
	WorldObject GetWorldObject();
	Vector2 GetPosition();
}

public class World : MonoBehaviour
{
	public DV_GameManager gameManager;

	public class WorldCacheItem
	{
		public List<string> Desc;
		public List<string> RawData;
	}

	public class WorldCache
	{
		public List<WorldCacheItem> TutoCache = new List<WorldCacheItem>();
		public List<WorldCacheItem> cache = new List<WorldCacheItem>();
	}
	#region TileInfo
	private class TileInfo : ITileInfo
	{
		private WorldTile m_tile;
		TileState m_state;
		Vector2 m_pos;

		public TileInfo(TileState _state, WorldTile _tile, Vector2 pos)
		{
			m_tile = _tile;
			m_state = _state;
			m_pos = pos;
		}
		public void SetBorderColor(Color _c)
		{
			Material mat = m_tile.Tile.GetComponent<MeshRenderer>().material;
			mat.SetColor("Color_D10C4CBD", _c);
		}
		public void SetEmissiveScale(float _v)
		{
			Material mat = m_tile.Tile.GetComponent<MeshRenderer>().material;
			mat.SetFloat("Vector1_237226DD", _v);
		}
		public WorldObject GetWorldObject()
		{
			return m_tile.Object;
		}
		public Vector2 GetPosition()
		{
			return m_pos;
		}
		public void SetVisited() { m_tile.Visited = true; }
		public TileState GetState() { return m_state; }
	}
	#endregion

	#region WorldTile
	private class WorldTile
	{
		private GameObject tileObject;
		private WorldObject worldObject;
		private bool bVisited;

		public GameObject Tile { get => tileObject; set { tileObject = value; } }
		public bool Visited { get => bVisited; set { bVisited = value; } }
		public WorldObject Object { get => worldObject;
			set
			{
				if (value == null && worldObject != null)
				{
					worldObject.transform.parent = null;
					worldObject.transform.localPosition = Vector3.zero;
				}
				worldObject = value;
				if (worldObject != null)
				{
					worldObject.transform.parent = tileObject.transform;
					worldObject.transform.localPosition = Vector3.zero;
				}
			}
		}
		public void Reset()
		{
			Visited = false;
		}
		public void Update(float emissiveScale)
		{
			if (Visited)
			{
				Material mat = Tile.GetComponent<MeshRenderer>().material;
				mat.SetFloat("Vector1_237226DD", 20f * emissiveScale);
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
	[SerializeField]
	private List<Obstacle> ObstacleList = null;
	[SerializeField]
	private List<Ennemy> EnnemyList = null;
	[SerializeField]
	private List<TextAsset> textTutosAssets = null;
	[SerializeField]
	private List<TextAsset> textAssets = null;
	[SerializeField]
	private MusicHandler musicHandler = null;

	[SerializeField]
	private Texture m_tileMainTex = null;
	[SerializeField]
	private Texture m_tileNextLevelTex = null;

	[SerializeField]
	private List<Color> m_tileColorPalette = null;

	private int m_currentCacheIndex = 0;
	private bool m_bTutosPassed = false;

	private int CurrentCacheEnnemyCount = -1;

	private bool m_bIsWorldInit;

	private Vector2 m_worldAnchor;

	private WorldTile[,] m_2dGrid;
	private WorldTile[] m_2dGridEndColumn;

	private static World s_Instance = null;

	private WorldCache cache = new WorldCache();

	public World Instance { get { if (s_Instance) s_Instance = new World(); return s_Instance; } }

	private World() => m_bIsWorldInit = false;

	public bool IsInTuto() { return m_bIsWorldInit; }
	public int GetNumberTutorials() { return cache.TutoCache.Count; }

	public void Update()
	{
		if (m_2dGrid == null) return;

		Vector2 gridSize = GetNumberOfTiles();
		Vector2Int iGridSize = new Vector2Int(Mathf.RoundToInt(gridSize.x), Mathf.RoundToInt(gridSize.y));
		for (int i = 0; i < iGridSize.x; ++i)
		{
			for (int j = 0; j < iGridSize.y; ++j)
			{
				if(m_2dGrid[i,j]!=null)
					m_2dGrid[i, j].Update(sampleBeat(musicHandler));
			}
		}
		foreach (WorldTile wo in m_2dGridEndColumn)
		{
			wo.Update(sampleBeat(musicHandler));
		}
	}

	public int GetEnnemyCount() { return CurrentCacheEnnemyCount; }

	public int GetCacheSize()
	{
		return cache.cache.Count;
	}

	public void ActivateEndColumn()
	{
		foreach (WorldTile wo in m_2dGridEndColumn)
		{
			wo.Visited = true;
		}
	}

	private float RandomFromCoords(Vector2 p)
	{
		float d = p.x*p.x+p.y*p.y;
		d = 0.5f*(1f+ Mathf.Sin(d * 13.9384934f));
		return Mathf.Clamp01(d);
	}

	private Color PickColor(Vector2 p)
	{
		float random = RandomFromCoords(p);
		return m_tileColorPalette!=null ? m_tileColorPalette[Mathf.FloorToInt(random * m_tileColorPalette.Count)] : new Color(1f, 0f, 0f);
	}

	public void Init(Vector2 _gridStartPos, Vector2 _gridEndPos, Vector2 _gridSize)
	{
		if (m_bIsWorldInit) return;

		TileStartPos = new Vector2(Mathf.RoundToInt(_gridStartPos.x), Mathf.RoundToInt(_gridStartPos.y));
		TileEndPos = new Vector2(Mathf.RoundToInt(_gridEndPos.x), Mathf.RoundToInt(_gridEndPos.y));
		TileSize = new Vector2(Mathf.RoundToInt(_gridSize.x), Mathf.RoundToInt(_gridSize.y));

		Vector2 gridSize = GetNumberOfTiles();
		Vector2Int iGridSize = new Vector2Int(Mathf.RoundToInt(gridSize.x), Mathf.RoundToInt(gridSize.y));
		m_2dGrid = new WorldTile[iGridSize.x, iGridSize.y];
		m_2dGridEndColumn = new WorldTile[iGridSize.y];
		for (int i = 0; i < iGridSize.x; ++i)
		{
			for(int j = 0; j < iGridSize.y; ++j)
			{
				m_2dGrid[i, j] = new WorldTile();
				m_2dGrid[i, j].Tile = Instantiate(tilePrefab);
				Vector2 it = new Vector2(i, j);
				Vector2 pos = TileStartPos + it * TileSize;
				m_2dGrid[i, j].Tile.transform.position = new Vector3(pos.x, 0, pos.y);
				m_2dGrid[i, j].Tile.transform.parent = WorldObject.transform;
				Material mat = m_2dGrid[i, j].Tile.GetComponent<MeshRenderer>().material;
				mat.SetColor("Color_D10C4CBD", PickColor(it));
				mat.SetFloat("Vector1_237226DD", 0f);
				mat.SetTexture("Texture2D_67BA07E5", m_tileMainTex);
			}
		}
		for (int j = 0; j < iGridSize.y; ++j)
		{
			m_2dGridEndColumn[j] = new WorldTile();
			m_2dGridEndColumn[j].Tile = Instantiate(tilePrefab);
			Vector2 it = new Vector2(iGridSize.x, j);
			Vector2 pos = TileStartPos + it * TileSize;
			m_2dGridEndColumn[j].Tile.transform.position = new Vector3(pos.x, 0, pos.y);
			m_2dGridEndColumn[j].Tile.transform.parent = WorldObject.transform;
			Material mat = m_2dGridEndColumn[j].Tile.GetComponent<MeshRenderer>().material;
			mat.SetColor("Color_D10C4CBD", PickColor(it));
			mat.SetFloat("Vector1_237226DD", 0f);
			mat.SetTexture("Texture2D_67BA07E5", m_tileNextLevelTex);
		}
		m_bIsWorldInit = true;

		foreach(TextAsset textFile in textAssets)
		{
			ReadFile(cache.cache, textFile);
		}
		foreach(TextAsset textTutoFile in textTutosAssets)
		{
			ReadFile(cache.TutoCache, textTutoFile);
		}
		UseCache(cache.TutoCache[0]);
	}

	public void ReadFile(List<WorldCacheItem> _cache, TextAsset textAsset)
	{
		string text = textAsset.text;
		string line;
		List<string> StringRows = new List<string>();
		string[] rows = text.Split('\n');
		foreach (string row in rows)
		{
			StringRows.Add(row);
		}
		StringRows.Reverse();
		WorldCacheItem item = new WorldCacheItem();
		item.RawData = StringRows;
		_cache.Add(item);
	}

	public void NextCache()
	{
		m_currentCacheIndex++;
		if (!m_bTutosPassed && m_currentCacheIndex > textTutosAssets.Count-1)
		{
			m_bTutosPassed = true;
		}
		
		if(m_bTutosPassed)
		{
			m_currentCacheIndex = Random.Range(0, cache.cache.Count);
			UseCache(cache.cache[m_currentCacheIndex]);
		}
		else
		{
			UseCache(cache.TutoCache[m_currentCacheIndex]);
		}
	}

	public WorldCacheItem GetCurrentWorldCacheItem()
	{
		if (m_bTutosPassed)
		{
			return cache.cache[m_currentCacheIndex];
		}
		else
		{
			return cache.TutoCache[m_currentCacheIndex];
		}
	}

	private void UseCache(WorldCacheItem cache)
	{
		Vector2 GridSize = new Vector2(cache.RawData[0].Length, cache.RawData.Count);
		Vector2 actualGridSize = GetNumberOfTiles();
		//if (GridSize != actualGridSize) throw new System.Exception("World Cache grid sizes inconsistent.");
		Reinit();
		cache.Desc = new List<string>();
		bool bAggregateDesc = false;
		for (int j = 0; j < GridSize.y; ++j)
		{
			string line = cache.RawData[j];
			if (j < actualGridSize.y)
			{
				for (int i = 0; i < actualGridSize.x; ++i)
				{
					char c = line[i];
					// here we pick the things to spawn
					if (c == 'e')
					{
						m_2dGrid[i, j].Object = Instantiate(EnnemyList[0]);
						CurrentCacheEnnemyCount++;
					}
					else if (c >= '0' && c <= '9')
					{
						int indexPrefab = (int)c - '0';
						m_2dGrid[i, j].Object = Instantiate(ObstacleList[indexPrefab]);
					}
					m_2dGrid[i, j].Reset();
					Material mat = m_2dGrid[i, j].Tile.GetComponent<MeshRenderer>().material;
					mat.SetColor("Color_D10C4CBD", PickColor(new Vector2(i, j)));
					mat.SetFloat("Vector1_237226DD", 0f);
				}
			}
			else
			{
				if (line.Contains("--")) continue;
				if (line.Contains("##"))
				{
					bAggregateDesc = !bAggregateDesc;
					continue;
				}
				if(bAggregateDesc)
					cache.Desc.Add(line);
			}
		}
		cache.Desc.Reverse();
		int k = 0;
		foreach(WorldTile tile in m_2dGridEndColumn)
		{
			tile.Reset();
			Material mat = tile.Tile.GetComponent<MeshRenderer>().material;
			mat.SetColor("Color_D10C4CBD", PickColor(new Vector2(GridSize.x, k)));
			mat.SetFloat("Vector1_237226DD", 0f);
			k++;
		}
	}

	public void Reinit()
	{
		Vector2 gridSize = GetNumberOfTiles();
		for (int i = 0; i < gridSize.x; ++i)
		{
			for (int j = 0; j < gridSize.y; ++j)
			{
				if(m_2dGrid[i, j].Object!=null)
				{
					if(!m_2dGrid[i, j].Object.IsPlayer())
						Destroy(m_2dGrid[i, j].Object);
					m_2dGrid[i, j].Object = null;
				}
			}
		}
		m_bIsWorldInit = true;
		CurrentCacheEnnemyCount = 0;
	}

	public void ActiveAllCells()
	{
		Vector2 gridSize = GetNumberOfTiles();
		for (int i = 0; i < gridSize.x; ++i)
		{
			for (int j = 0; j < gridSize.y; ++j)
			{
				if (m_2dGrid[i, j] != null)
				{
					if (m_2dGrid[i, j]!=null)
						m_2dGrid[i, j].Visited = true;
				}
			}
		}
	}

	public TileState ProjectToGrid(ref Vector2 pos)
	{
		if (pos.x >= TileEndPos.x)
			return TileState.BorderRight;
		if (pos.x < 0 || pos.y < 0 || pos.y >= TileEndPos.y)
			return TileState.Border;

		pos = new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

		return TileState.Empty;
	}

	public ITileInfo GetTileInfo(Vector2 pos)
	{
		TileState state = ProjectToGrid(ref pos);
		WorldTile wo = null;
		if (state!=TileState.Border && state != TileState.BorderRight)
		{
			wo = m_2dGrid[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)];
			if (wo.Object == null)
				state = TileState.Empty;
			else if (wo.Object.IsObstacle())
				state = TileState.Occupied;
			else if (wo.Object.IsIA())
				state = TileState.Ennemy;
		}
		return new TileInfo(state, wo, new Vector2(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
	}

	public Vector2Int GetNumberOfTiles()
	{
		Vector2 fGridSize = (TileEndPos - TileStartPos) / TileSize;
		return new Vector2Int(Mathf.RoundToInt(fGridSize.x), Mathf.RoundToInt(fGridSize.y));
	}

	public class Backup { public WorldObject wo; public Vector2 pos; };
	public void SetWorldAnchor(Vector2 _anchor)
	{
		// if different from old world anchor, move stuff around
		/*{
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
		}*/
		m_worldAnchor = _anchor;
	}

	public void PlaceObject(WorldObject _wo)
	{
		WorldObject wo = Instantiate<WorldObject>(_wo);
		Vector2 gridSize = GetNumberOfTiles();
		float threshold = 0.98f, step = 1f / (gridSize.x * gridSize.y);
		bool bPlaced = false;
		for (int i = 0; i < gridSize.x && !bPlaced; ++i)
		{
			for (int j = 0; j < gridSize.y && !bPlaced; ++j)
			{
				if (m_2dGrid[i, j].Object == null && Random.value > threshold)
				{
					m_2dGrid[i, j].Object = wo;
					bPlaced = true;
				}
				else
					threshold = threshold - step;
			}
		}
	}

	public void PlaceObject(WorldObject _wo, Vector2 _pos, Vector2 _d)
	{
		WorldObject wo = Instantiate<WorldObject>(_wo);
		Vector2 newPos = _pos + _d;
		ITileInfo tileInfo = GetTileInfo(newPos);
		if (tileInfo.GetState() == TileState.Occupied || tileInfo.GetState() == TileState.Ennemy)
			return;
		tileInfo.SetVisited();
		m_2dGrid[Mathf.RoundToInt(_pos.x), Mathf.RoundToInt(_pos.y)].Object = null;
		m_2dGrid[Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y)].Object = wo;
	}

	List<ITileInfo> GetNeighbors(ITileInfo tile, int dist)
	{
		List<ITileInfo> ret = new List<ITileInfo>();
		Vector2 gridSize = GetNumberOfTiles();
		Vector2 pos = tile.GetPosition();
		Vector2Int iGridSize = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
		for (int i = Mathf.Max(0, iGridSize.x-dist); i < Mathf.Min(iGridSize.x+dist, gridSize.x); ++i)
		{
			for (int j = Mathf.Max(0, iGridSize.y - dist); j < Mathf.Min(0, iGridSize.y - dist); ++j)
			{
				ret.Add(GetTileInfo(pos));
			}
		}
		return ret;
	}

	private float sampleBeat(MusicHandler mh)
	{
		float x = mh.GetBeatOffset();
		float leftPart = 1f + Mathf.Sin(Mathf.Clamp(x, -0.125f, 0.125f) * 4f * Mathf.PI);
		float rightPart = Mathf.SmoothStep(1f, 0f, 2f * x);
		return Mathf.Clamp01(Mathf.Min(leftPart, rightPart));	}

	public TileState MoveObject(Vector2 _pos, Vector2 _d)
	{
		Vector2 newPos = _pos + _d;
		ITileInfo tileInfo = GetTileInfo(newPos);
		if (tileInfo.GetState() != TileState.Empty)
			return tileInfo.GetState();

		ITileInfo oldTileInfo = GetTileInfo(_pos);
		oldTileInfo.SetVisited();

		oldTileInfo.SetEmissiveScale(4f * sampleBeat(musicHandler));

		WorldObject owo = m_2dGrid[Mathf.RoundToInt(_pos.x), Mathf.RoundToInt(_pos.y)].Object;
		m_2dGrid[Mathf.RoundToInt(_pos.x), Mathf.RoundToInt(_pos.y)].Object = null;
		if (owo.IsPlayer())
			tileInfo.SetVisited();
		m_2dGrid[Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.y)].Object = owo;
		return tileInfo.GetState();
	}

	public void SetObject(WorldObject _wo, Vector2 _pos)
	{
		ITileInfo tileInfo = GetTileInfo(_pos);
		if (tileInfo.GetState() == TileState.Occupied || tileInfo.GetState() == TileState.Ennemy)
			return;
		tileInfo.SetVisited();
		m_2dGrid[Mathf.RoundToInt(_pos.x), Mathf.RoundToInt(_pos.y)].Object = _wo;
	}
#endregion
}
