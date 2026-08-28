public class Shrine : SpawnableObject
{
	protected override void Start()
	{
		base.Start();
		if (spawned)
		{
			AchievementManager.instance.shrineSpawned = true;
		}
	}
}
