using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileInfo
{
	bool IsAvailable();
}

public class World : MonoBehaviour
{
	private class TileInfo : ITileInfo
	{
		private bool m_bIsAvailable;

		public TileInfo(bool bAvailable) => m_bIsAvailable = bAvailable;
		public bool IsAvailable() { return m_bIsAvailable; }
	}

	public ITileInfo GetTileFromPositionAndDirection(Vector2 pos, Vector2 dir)
	{

		return new TileInfo(true);
	}

}
