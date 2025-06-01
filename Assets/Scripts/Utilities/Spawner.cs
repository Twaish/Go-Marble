using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {
  [SerializeField] private GameObject prefabToSpawn;
  [SerializeField] private GameObject spawnEffect;
  [SerializeField] private float respawnDelay = 5f;
  [SerializeField] private bool spawnOnStart;

  private GameObject currentInstance;
  private bool isSpawning = false;

  private void Start() {
    if (spawnOnStart) {
      SpawnPrefab();
    }
  }
  private void OnDestroy() {
    if (currentInstance != null) {
      Destroy(currentInstance);
    }
  }

  private void Update() {
    if (currentInstance == null && !isSpawning) {
      StartCoroutine(RespawnRoutine());
    }
  }

  private IEnumerator RespawnRoutine() {
    isSpawning = true;
    yield return new WaitForSeconds(respawnDelay);
    SpawnPrefab();
    isSpawning = false;
  }

  private void SpawnPrefab() {
    if (spawnEffect != null) {
      Instantiate(spawnEffect, transform.position, transform.rotation);
    }

    currentInstance = Instantiate(prefabToSpawn, transform.position, transform.rotation);
  }
}
