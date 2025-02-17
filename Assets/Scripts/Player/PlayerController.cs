using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {
  private Rigidbody rb;
  private float movementX;
  private float movementY;

  public float speed;
  public TextMeshProUGUI scoreText;

  private int score;

  void Start() {
    speed = 0;
    score = 0;
    rb = GetComponent<Rigidbody>();
    // UpdateScoreText();
  }

  void FixedUpdate() {
    Vector3 movement = new Vector3(movementX, 0.0f, movementY).normalized;

    rb.AddForce(movement * speed);
  }

  void OnTriggerEnter(Collider other) {
    if (other.gameObject.CompareTag("PickUp")) {
      other.gameObject.SetActive(false);
      score++;
      UpdateScoreText();
    }
  }

  void OnMove(InputValue movementValue) {
    Vector2 movementVector = movementValue.Get<Vector2>();

    movementX = movementVector.x;
    movementY = movementVector.y;
  }

  void UpdateScoreText() {
    scoreText.text = "Score: " + score.ToString();
  }
}
