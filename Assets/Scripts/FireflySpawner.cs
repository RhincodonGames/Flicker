using UnityEngine;

public class FireflySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject friendPrefab;
    public GameObject grubPrefab;

    [Header("Counts")]
    public int friendCount = 6;
    public int grubCount = 14;

    [Header("Spawn Area (shared footprint)")]
    public Vector2 areaSize = new Vector2(30f, 30f);

    [Header("Firefly Friend Height (they fly)")]
    public float friendMinHeight = 1f;
    public float friendMaxHeight = 4f;

    [Header("Grub Ground Level")]
    public float groundY = 0f; // set this to match your actual ground plane's Y position

    void Start()
    {
        for (int i = 0; i < friendCount; i++)
            SpawnFriend();

        for (int i = 0; i < grubCount; i++)
            SpawnGrub();
    }

    void SpawnFriend()
    {
        if (friendPrefab == null) return;
        Vector3 pos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            Random.Range(friendMinHeight, friendMaxHeight),
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
        );
        Instantiate(friendPrefab, pos, Quaternion.identity);
    }

    void SpawnGrub()
    {
        if (grubPrefab == null) return;
        Vector3 pos = transform.position + new Vector3(
            Random.Range(-areaSize.x / 2f, areaSize.x / 2f),
            groundY,
            Random.Range(-areaSize.y / 2f, areaSize.y / 2f)
        );
        Instantiate(grubPrefab, pos, Quaternion.identity);
    }
}