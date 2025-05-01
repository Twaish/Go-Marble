using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class BorderCursor : MonoBehaviour {
  [SerializeField]
  private float pulseSpeed = 1f;
  [SerializeField]
  private float pulseSize = 1.2f;
  [SerializeField]
  private float borderThickness = 5f;
  [SerializeField]
  private float positionLerpSpeed = 10f;

  private RectTransform imageTransform;
  private GameObject currentFocusedUI;
  private Vector3 targetPosition;
  private float pulseTime;
  private Image image;

  private void Start() {
    image = GetComponent<Image>();
    imageTransform = image.GetComponent<RectTransform>();
    targetPosition = imageTransform.position;
    StartCoroutine(PulsateBorder());
  }

  private void FixedUpdate() {
    GameObject focusedUI = EventSystem.current.currentSelectedGameObject;
    if (focusedUI == null) return;
    UpdateBorderPosition(focusedUI);
  }

  private void UpdateBorderPosition(GameObject focusedUI) {
    if (!focusedUI.TryGetComponent<RectTransform>(out var focusedRect)) return;
    Vector2 centerPosition = focusedRect.TransformPoint(focusedRect.rect.center);

    imageTransform.position = Vector3.Lerp(imageTransform.position, centerPosition, Time.fixedDeltaTime * positionLerpSpeed);
    imageTransform.sizeDelta = focusedRect.sizeDelta + new Vector2(borderThickness, borderThickness);
  }

  private IEnumerator PulsateBorder() {
    Vector3 initialScale = imageTransform.localScale;

    while (true) {
      pulseTime += Time.deltaTime * pulseSpeed;
      float smoothedPulse = Mathf.SmoothStep(1f, pulseSize, Mathf.PingPong(pulseTime, 1f));
      imageTransform.localScale = initialScale * smoothedPulse;

      yield return null;
    }
  }

}
