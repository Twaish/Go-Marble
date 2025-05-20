using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoScrollController : MonoBehaviour {
  [Header("Scroll Setup")]
  [SerializeField] private ScrollRect scrollRect;
  [SerializeField] private RectTransform contentRect;

  [Header("Scrolling Options")]
  [SerializeField] private float scrollSpeed = 7.5f;
  [SerializeField] private float verticalOffsetFactor = 0.5f;
  [SerializeField] private float horizontalOffsetFactor = 0f;
  
  [Header("Axis")]
  [SerializeField] private bool enableVerticalScroll = true;
  [SerializeField] private bool enableHorizontalScroll = true;

  private Coroutine scrollRoutine;

  public void CenterOnItem(GameObject tileUI) {
    // RectTransform target = tileUI.GetComponent<RectTransform>();
    // Canvas.ForceUpdateCanvases();

    // Vector2 viewportCenterInWorld = scrollRect.viewport.TransformPoint(scrollRect.viewport.rect.center);
    // Vector2 targetCenterInWorld = target.TransformPoint(target.rect.center);

    // Vector2 diff = viewportCenterInWorld - targetCenterInWorld;

    // float contentHeight = contentRect.rect.height;
    // float viewportHeight = scrollRect.viewport.rect.height;

    // float contentWidth = contentRect.rect.width;
    // float viewportWidth = scrollRect.viewport.rect.width;

    // float scrollDeltaY = -diff.y / Mathf.Max(1f, contentHeight - viewportHeight);
    // float scrollDeltaX = -diff.x / Mathf.Max(1f, contentWidth - viewportWidth);

    // float targetVerticalPos = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollDeltaY);
    // float targetHorizontalPos = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + scrollDeltaX);

    // if (scrollRoutine != null) {
    //   StopCoroutine(scrollRoutine);
    // }

    // scrollRoutine = StartCoroutine(SmoothScrollTo(targetHorizontalPos, targetVerticalPos));
    // Debug.Log($"diff.y: {diff.y}, scrollDeltaY: {scrollDeltaY}, new V: {targetVerticalPos}");

    RectTransform target = tileUI.GetComponent<RectTransform>();
    Canvas.ForceUpdateCanvases();

    Vector2 viewportCenterInWorld = scrollRect.viewport.TransformPoint(scrollRect.viewport.rect.center);
    Vector2 targetCenterInWorld = target.TransformPoint(target.rect.center);

    Vector2 verticalOffsetWorld = target.TransformVector(target.rect.height * verticalOffsetFactor * Vector2.up);
    Vector2 horizontalOffsetWorld = target.TransformVector(horizontalOffsetFactor * target.rect.width * Vector2.right);

    Vector2 diff = viewportCenterInWorld - targetCenterInWorld - new Vector2(horizontalOffsetWorld.x, verticalOffsetWorld.y);

    float contentHeight = contentRect.rect.height;
    float viewportHeight = scrollRect.viewport.rect.height;

    float contentWidth = contentRect.rect.width;
    float viewportWidth = scrollRect.viewport.rect.width;

    float currentV = scrollRect.verticalNormalizedPosition;
    float currentH = scrollRect.horizontalNormalizedPosition;

    float targetVerticalPos = currentV;
    float targetHorizontalPos = currentH;
    
    if (enableVerticalScroll) {
      float scrollDeltaY = -diff.y / Mathf.Max(1f, contentHeight - viewportHeight);
      targetVerticalPos = Mathf.Clamp01(currentV + scrollDeltaY);
    }

    if (enableHorizontalScroll) {
      float scrollDeltaX = -diff.x / Mathf.Max(1f, contentWidth - viewportWidth);
      targetHorizontalPos = Mathf.Clamp01(currentH + scrollDeltaX);
    }

    if (scrollRoutine != null) {
      StopCoroutine(scrollRoutine);
    }

    scrollRoutine = StartCoroutine(SmoothScrollTo(targetHorizontalPos, targetVerticalPos));

    // float scrollDeltaY = -diff.y / Mathf.Max(1f, contentHeight - viewportHeight);
    // float scrollDeltaX = -diff.x / Mathf.Max(1f, contentWidth - viewportWidth);

    // float targetVerticalPos = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollDeltaY);
    // float targetHorizontalPos = Mathf.Clamp01(scrollRect.horizontalNormalizedPosition + scrollDeltaX);

    // if (scrollRoutine != null) {
    //   StopCoroutine(scrollRoutine);
    // }

    // scrollRoutine = StartCoroutine(SmoothScrollTo(targetHorizontalPos, targetVerticalPos));
  }
  
  private IEnumerator SmoothScrollTo(float targetH, float targetV) {
    float duration = 0.35f; // Time in seconds
    float time = 0f;

    float startH = scrollRect.horizontalNormalizedPosition;
    float startV = scrollRect.verticalNormalizedPosition;

    while (time < duration) {
      time += Time.unscaledDeltaTime * scrollSpeed;
      float t = Mathf.SmoothStep(0f, 1f, time / duration);
      
      if (enableHorizontalScroll)
        scrollRect.horizontalNormalizedPosition = Mathf.Lerp(startH, targetH, t);

      if (enableVerticalScroll)
        scrollRect.verticalNormalizedPosition = Mathf.Lerp(startV, targetV, t);

      yield return null;

      // scrollRect.horizontalNormalizedPosition = Mathf.Lerp(startH, targetH, t);
      // scrollRect.verticalNormalizedPosition = Mathf.Lerp(startV, targetV, t);

      // if (reflect != null)
      //   reflect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollRect.horizontalNormalizedPosition);

      // yield return null;
    }

    if (enableHorizontalScroll)
      scrollRect.horizontalNormalizedPosition = targetH;

    if (enableVerticalScroll)
      scrollRect.verticalNormalizedPosition = targetV;
  }
}
