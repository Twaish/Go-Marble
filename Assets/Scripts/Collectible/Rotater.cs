using UnityEngine;

public class Rotater : MonoBehaviour {
  public Vector3 rotationSpeed = new Vector3(15, 30, 45);

  void Update() {
    transform.Rotate(rotationSpeed * Time.deltaTime);
  }
}
