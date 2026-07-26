using System.Collections.Generic;
using UnityEngine;

public class FireflySwarm : MonoBehaviour
{
    public GameObject companionPrefab; 
    private List<GameObject> companions = new List<GameObject>();

    public void AddCompanion()
    {
        GameObject companion = Instantiate(companionPrefab, transform.position, Quaternion.identity);
        SwarmFollower follower = companion.GetComponent<SwarmFollower>();
        if (follower != null)
        {
            follower.target = transform;
            follower.indexInSwarm = companions.Count;
        }
        companions.Add(companion);
    }

    public void RemoveCompanion()
    {
        if (companions.Count == 0) return;
        GameObject last = companions[companions.Count - 1];
        companions.RemoveAt(companions.Count - 1);
        if (last != null) Destroy(last);
    }
}