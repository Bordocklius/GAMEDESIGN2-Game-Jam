using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _spawnPoints; // Must be more than 7 in size

    [SerializeField]
    private GameObject Target;

    [SerializeField]
    private int targetCount = 7; // Number of targets to spawn

    private void Start()
    {
        SpawnTargets();
    }

    private void SpawnTargets()
    {
        // Safety check: Make sure there are enough spawn points
        if (_spawnPoints.Count < targetCount)
        {
            Debug.LogError("Not enough spawn points to spawn " + targetCount + " targets!");
            return;
        }

        // Shuffle the list using Fisher–Yates shuffle
        List<Transform> shuffled = new List<Transform>(_spawnPoints);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randIndex]) = (shuffled[randIndex], shuffled[i]);
        }

        // Spawn targets at the first 7 random unique spawn points
        for (int i = 0; i < targetCount; i++)
        {
            GameObject newTarget = Instantiate(Target, shuffled[i].position, shuffled[i].rotation, this.transform);
        }
    }
}
