using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUps; 
    public Tilemap tilemap; 
    public float spawnInterval = 60f; 
    public int maxAttempts = 10; 

    public Transform spawnArea; // Transform que representa el cuadrado

    void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnPowerUp();
        }
    }

    void SpawnPowerUp()
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float randomX = Random.Range(spawnArea.position.x - spawnArea.localScale.x / 2, spawnArea.position.x + spawnArea.localScale.x / 2);
            float randomY = Random.Range(spawnArea.position.y - spawnArea.localScale.y / 2, spawnArea.position.y + spawnArea.localScale.y / 2);

            Vector3Int cellPosition = tilemap.WorldToCell(new Vector3(randomX, randomY, 0));

            if (!tilemap.HasTile(cellPosition))
            {
                Vector3 worldPosition = tilemap.CellToWorld(cellPosition) + tilemap.tileAnchor;

                Collider2D hitCollider = Physics2D.OverlapCircle(worldPosition, 0.5f);
                if (hitCollider == null)
                {
                    GameObject powerUpPrefab = powerUps[Random.Range(0, powerUps.Length)];

                    Instantiate(powerUpPrefab, worldPosition, Quaternion.identity);
                    return;
                }
            }
        }

        Debug.LogWarning("No se pudo encontrar una posición válida para el power-up después de varios intentos.");
    }
}
