using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnArea
{
    public float weight = 1f;
    [SerializeField] private Transform transform;
    [SerializeField] private Vector3 directions;
    [SerializeField] private bool xNegative;
    [SerializeField] private bool yNegative;
    [SerializeField] private bool zNegative;

    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    [SerializeField] private bool AsChild = false;
    public bool asChild
    {
        get
        {
            return AsChild;
        }
    }

    public Vector3 randomSpawnPosition()
    {
        Vector3 randomPosition = transform.position;

        Vector3 direction = new Vector3(Random.Range(xNegative ? -1f : 0f, directions.x),
            Random.Range(yNegative ? -1f : 0f, directions.y),
            Random.Range(zNegative ? -1f : 0f, directions.z));
        direction.Normalize();

        direction = transform.rotation * direction;

        float distance = Random.Range(minDistance, maxDistance);

        return randomPosition + (direction * distance);
    }

    public Quaternion spawnRotation()
    {
        return transform.rotation;
    }
}
