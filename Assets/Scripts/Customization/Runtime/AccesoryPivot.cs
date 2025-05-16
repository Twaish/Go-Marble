using UnityEngine;

public class AccesoryPivot : MonoBehaviour
{
  [SerializeField] private GameObject target;
  [SerializeField] private Vector3 offset = new(0, 0.5f, 0);

  private void Update() {
    Vector3 desiredPosition = target.transform.position + offset;

    transform.position = desiredPosition;
  }
}
