using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject friendPrefab;
    public GameObject grubPrefab;

    [Header("Counts")]
    public int friendCount = 6;
    public int grubCount = 14;

    [Header("Spawn Area")]
    public Vector2 areaSize = new Vector2(30f, 30f); // width/depth centered on this object's position
    public float minHeight = 1f;
    public float maxHeight = 4f;

    void Start()
    {
        for (int i = 0; i < friendCount; i++)
            SpawnAt(friendPrefab);

        for (int i = 0; i < grubCount; i++)
            SpawnAt(grubPrefab);
    }

    void SpawnAt(GameObject prefab)
    {
        if (prefab == null) return;

        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(minHeight, maxHeight),
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
        );

        Instantiate(prefab, randomPos, Quaternion.identity);
    }
}