using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// https://www.youtube.com/watch?v=8QNypjVWAoU&ab_channel=CodingwithRobby
public class OverlayManager : MonoBehaviour {
  private const string wrapperClass = "transition";
  private const string wrapperActiveClass = "active";

  static public OverlayManager Instance;

  [SerializeField]
  private UIDocument uiDocument;
  [SerializeField]
  private float transitionDuration = 0.5f; // Match USS

  private VisualElement rootElement;
  private VisualElement wrapperElement;

  void OnEnable() {
    rootElement = uiDocument.rootVisualElement;
    wrapperElement = rootElement.Q(className: wrapperClass);
  }

  void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    } else if (Instance != this) {
      Destroy(gameObject);
    }
  }

  public IEnumerator DisplayOverlay() {
    wrapperElement.AddToClassList(wrapperActiveClass);
    yield return new WaitForSeconds(transitionDuration);
  }
  public IEnumerator HideOverlay() {
    wrapperElement.RemoveFromClassList(wrapperActiveClass);
    yield return new WaitForSeconds(transitionDuration);
  }
  
}
