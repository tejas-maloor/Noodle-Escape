using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectData
{
    public GameObject envObject;
    public bool isEnabled;
    public int objectCount;
    public float yBase;
}

public class ObjectSpawner : MonoBehaviour
{
    public Vector2 dimensions = new Vector2(35, 35);
    public Vector3 offset = Vector3.zero;
    public List<ObjectData> objects;

    public void Start()
    {
        foreach (var item in objects)
        {
            if(item.isEnabled)
            {
                SpawnObjects(item);
            }
        }
    }

    public void SpawnObjects(ObjectData objectData)
    {
        for (int i = 0; i < objectData.objectCount; i++)
        {
            GameObject g = GameObject.Instantiate(objectData.envObject);
            g.transform.parent = transform;
            g.transform.localPosition = new Vector3()
            {
                x = Random.Range(-dimensions.x / 2, dimensions.x / 2),
                y = objectData.yBase + transform.position.y,
                z = Random.Range(-dimensions.y / 2, dimensions.y / 2)
            } + offset;
            g.transform.rotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 debugDimensions = new Vector3(dimensions.x, 0, dimensions.y);
        Gizmos.DrawCube(transform.position + offset, debugDimensions);
    }
}
