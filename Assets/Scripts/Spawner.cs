using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
  [SerializeField]
  private int maxPickUps = 10;

  [SerializeField]
  private int spawnInterval = 5;

  [SerializeField]
  private GameObject pickUpPrefab;

  private float baseY = 0.5f;

  private Collider boxCollider;
  private Bounds spawnBounds;
  private List<GameObject> pickUps = new List<GameObject>();

  void Start() {
    boxCollider = GetComponent<BoxCollider>();

    if (boxCollider == null) {
      Debug.Log("Collider is null");
      return;
    }
    
    spawnBounds = boxCollider.bounds;

    Debug.Log(spawnBounds);

    InvokeRepeating(nameof(SpawnPickUp), 0f, spawnInterval);
  }

  void SpawnPickUp() {
    pickUps.RemoveAll(pickUp => pickUp == null);

    if (pickUps.Count < maxPickUps) {
      Vector3 spawnPosition = GetRandomSpawnPosition();
      GameObject newPickUp = Instantiate(pickUpPrefab, spawnPosition, Quaternion.identity);
      pickUps.Add(newPickUp);
    }
  }

  Vector3 GetRandomSpawnPosition() {
    float x = Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x);
    float z = Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z);

    Debug.Log($"Spawn Position: ({x}, {baseY}, {z})");

    return new Vector3(x, baseY, z);
  }
}
