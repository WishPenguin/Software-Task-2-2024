using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GameController : MonoBehaviour
{
    public GameObject hazard;  // Your asteroid prefab
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;

    public PlayerController playerController;  // Reference to the PlayerController script

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        for (int i = 0; i < hazardCount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnValues.x, spawnValues.x),
                spawnValues.y,
                spawnValues.z
            );
            Quaternion spawnRotation = Quaternion.identity;

            // Instantiate an asteroid
            GameObject asteroidInstance = Instantiate(hazard, spawnPosition, spawnRotation);

            // Set the player’s missile target to the newly spawned asteroid
            if (playerController != null)
            {
                playerController.SetTarget(asteroidInstance);
            }

            yield return new WaitForSeconds(spawnWait);
        }
    }
}
