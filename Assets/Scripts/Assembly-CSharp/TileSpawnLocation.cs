using UnityEngine;

public class TileSpawnLocation : MonoBehaviour
{
	public int eulerAngle;

	public int posX;

	public int posY;

	private void Start()
	{
		SpawnManager.instance.tileSpawnUis.Add(base.gameObject);
		if (SpawnManager.instance.combat)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void SpawnTile()
	{
		TileManager.instance.SpawnNewTile(posX, posY, eulerAngle);
		SpawnManager.instance.tileSpawnUis.Remove(base.gameObject);
		SpawnManager.instance.StartNextWave();
		Object.Destroy(base.gameObject);
	}
}
