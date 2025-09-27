using UnityEngine;
using System.Collections.Generic;

public class BallSpawning : MonoBehaviour
{
    [SerializeField]
    private List<Material> BallColors;

    [SerializeField]
    private GameObject SpawnArea;

    [SerializeField]
    private GameObject BallPrefab;

    [SerializeField]
    private uint BallQuantity = 100;

    // Define spawn area size (in world units)
    [SerializeField]
    private Vector3 spawnAreaSize = new Vector3(11f, 5f, 11f);

    private void Start()
    {
        Vector3 areaCenter = SpawnArea.transform.position;

        for (int i = 0; i < BallQuantity; i++)
        {
            // Pick random position inside manually defined area
            Vector3 randomPos = areaCenter + new Vector3(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
                Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
            );

            // Instantiate the ball and parent it to this GameObject
            GameObject newBall = Instantiate(BallPrefab, randomPos, Quaternion.identity, this.transform);

            // Assign random color/material
            Material randomMaterial = BallColors[Random.Range(0, BallColors.Count)];
            Renderer renderer = newBall.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = randomMaterial;
            }
        }
    }

    // Optional: Draw spawn area in scene view
    private void OnDrawGizmosSelected()
    {
        if (SpawnArea != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(SpawnArea.transform.position, spawnAreaSize);
        }
    }
}
