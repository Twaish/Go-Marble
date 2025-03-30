using UnityEngine;

public class FollowMarblePreview : MonoBehaviour {
  [SerializeField]
  private Transform marble;
  [SerializeField]
  private float distance = 10f; 
  [SerializeField]
  private float height = 4f;
  [SerializeField]
  private float followSpeed = 5f;
  [SerializeField]
  private float rotationSpeed = 20f;
  [SerializeField]
  private float screenRightPosition = 0.1f;
  
  private Vector3 offset;

  void Start()  {
    CalculateOffset();
  }

  void LateUpdate() {
    offset = Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0) * offset;
    Vector3 targetPosition = marble.position + offset;
    transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

    Vector3 rightOffset = (screenRightPosition * 2 - 1) * 0.5f * distance * transform.right;
    transform.LookAt(marble.position + rightOffset);
  }

  
  void CalculateOffset() {
    Vector3 viewportPoint = new(0.8f, 0.5f, distance);
    
    Camera tempCam = new GameObject("TempCam").AddComponent<Camera>();
    tempCam.transform.SetPositionAndRotation(marble.position, Quaternion.identity);
    offset = tempCam.ViewportToWorldPoint(viewportPoint) - marble.position;
    Destroy(tempCam.gameObject);

    offset.y = height;
    offset = offset.normalized * distance;
  }
}
