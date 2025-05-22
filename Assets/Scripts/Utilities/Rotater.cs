using UnityEngine;

public class Rotater : MonoBehaviour {
  [SerializeField] private Vector3 rotationSpeed = new(15, 30, 45);

  void Update() {
    transform.Rotate(rotationSpeed * Time.deltaTime);
  }
}
