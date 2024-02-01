using System.Collections.Generic;
using UnityEngine;

public enum ObstacleSize
{
	Large = 0,
	Medium = 1,
	Small = 2
}

public struct ObstacleSpawningPair
{
	public Transform transform;
	public ObstacleSize size;

	public ObstacleSpawningPair(Transform parent, ObstacleSize size)
	{
		this.transform = parent;
		this.size = size;
	}
}

public class ObstacleGrid : MonoBehaviour
{
	public void GetRandomObstacles(out List<ObstacleSpawningPair> spawningPairs)
	{
		spawningPairs = new List<ObstacleSpawningPair>();

		int currentSize = 0;

		CreatePairs(transform, currentSize, ref spawningPairs);
	}

	public void CreatePairs(Transform currentTransfom, int currentSize, ref List<ObstacleSpawningPair> spawningPairs)
	{
		for (int i = 0; i < currentTransfom.transform.childCount; i++)
		{
			Transform child = currentTransfom.transform.GetChild(i);

			if (Random.Range(0, 2) == 0 || child.childCount == 0)
			{
				spawningPairs.Add(new ObstacleSpawningPair(child, (ObstacleSize)currentSize));
			}
			else
			{
				CreatePairs(child, (currentSize + 1), ref spawningPairs);
			}
		}
	}
}
