using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileInfo
{
	bool IsAvailable();
}

public class World : MonoBehaviour
{
#region TileInfo
	private class TileInfo : ITileInfo
	{
		private bool m_bIsAvailable;

		public TileInfo(bool bAvailable) => m_bIsAvailable = bAvailable;
		public bool IsAvailable() { return m_bIsAvailable; }
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
	private GameObject tilePrefab = null;

	private bool m_bIsWorldInit;

	public GameObject[,] m_2dGrid;

	private static World s_Instance = null;

	public World Instance { get { if (s_Instance) s_Instance = new World(); return s_Instance; } }

	private World() => m_bIsWorldInit = false;

	public void Awake()
	{
		if (m_bIsWorldInit) return;
		Vector2 gridSize = (TileEndPos - TileStartPos) / TileSize;
		m_2dGrid = new GameObject[Mathf.RoundToInt(gridSize.x), Mathf.RoundToInt(gridSize.y)];
		for(int i = 0; i < gridSize.x; ++i)
		{
			for(int j = 0; j < gridSize.y; ++j)
			{
				Color rdrCol = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
				m_2dGrid[i, j] = Instantiate(tilePrefab);
				Vector2 it = new Vector2(i, j);
				Vector2 pos = TileStartPos + it * TileSize;
				m_2dGrid[i, j].transform.position = new Vector3(pos.x, 0, pos.y);
				Material mat = m_2dGrid[i, j].GetComponent<MeshRenderer>().material;
				mat.SetColor("Color_D10C4CBD", rdrCol);
			}
		}
		m_bIsWorldInit = true;
	}

	public ITileInfo GetTileFromPositionAndDirection(Vector2 pos, Vector2 dir)
	{

		return new TileInfo(true);
	}

	public void OnDrawGizmos()
	{
		Vector2 NumberOfTiles = (TileEndPos - TileStartPos) / TileSize;
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
