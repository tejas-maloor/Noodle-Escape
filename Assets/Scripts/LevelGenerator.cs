using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour 
{

	public const float PLATFORM_SIZE = 40f;
    public const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 200f;

    [SerializeField] private SetPiece levelPart_Start;
    [SerializeField] private List<SetPiece> levelPartList;
    [SerializeField] private PlayerController player;

    private Vector3 lastEndPosition;

    private void Awake() 
    {
        lastEndPosition = levelPart_Start.endPosition.position;//.Find("EndPosition").position;

        int startingSpawnLevelParts = 5;
        for (int i = 0; i < startingSpawnLevelParts; i++) {
            SpawnLevelPart();
        }
    }

    private void Update() 
    {
        if (Vector3.Distance(player.transform.position, lastEndPosition) < PLAYER_DISTANCE_SPAWN_LEVEL_PART) 
        {
            SpawnLevelPart();
        }
    }

    private void SpawnLevelPart() 
    {
		SetPiece chosenLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];
		SetPiece lastLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPosition);
        lastEndPosition = lastLevelPartTransform.endPosition.position;// .Find("EndPosition").position;
    }

    private SetPiece SpawnLevelPart(SetPiece levelPart, Vector3 spawnPosition) 
    {
		SetPiece levelPartTransform = Instantiate(levelPart.gameObject, spawnPosition, Quaternion.identity).GetComponent<SetPiece>();
        return levelPartTransform;
    }

}
