using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObstacleData
{
	public GameObject obsObject;
	public bool isEnabled;
	public float yBase;
}

public class ObstcaleSpanwer : MonoBehaviour
{
	public List<ObstacleData> smallObstacles;
	public List<ObstacleData> mediumObstacles;
	public List<ObstacleData> largeObstacles;

	public ObstacleGrid ObstacleGrid;

	private void Start()
	{
		GenerateObstacles();
	}

	public void GenerateObstacles()
	{
		List<ObstacleSpawningPair> spawningPairs;
		ObstacleGrid.GetRandomObstacles(out spawningPairs);

		foreach (ObstacleSpawningPair pair in spawningPairs)
		{
			ObstacleData data = GetRandomObstacleData(pair.size);
			if (data.isEnabled)
			{
				GameObject obs = data.obsObject != null ? Instantiate(data.obsObject) : new GameObject() { name = $"Obstacle : {pair.size}" };
				obs.transform.parent = pair.transform;
				obs.transform.localPosition = new Vector3(0, data.yBase, 0);
			}
		}
	}

	private ObstacleData GetRandomObstacleData(ObstacleSize size)
	{
		switch (size)
		{
			case ObstacleSize.Small: return smallObstacles[UnityEngine.Random.Range(0, smallObstacles.Count)];
			case ObstacleSize.Medium: return mediumObstacles[UnityEngine.Random.Range(0, mediumObstacles.Count)];
			case ObstacleSize.Large: return largeObstacles[UnityEngine.Random.Range(0, largeObstacles.Count)];
			default: throw new ArgumentOutOfRangeException("size", size, null);
		}
	}
}
