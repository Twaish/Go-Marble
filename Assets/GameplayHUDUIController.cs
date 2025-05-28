using System.Collections;
using UnityEngine;

public class GameplayHUDUIController : MonoBehaviour {
  [SerializeField] private Animator gameplayHUDAnimator;

  private void Start() {
    LevelManager.instance.OnLevelStarted += HandleLevelStarted;
    LevelManager.instance.OnLevelEnded += HandleLevelEnded;
  }

  private void OnDestroy() {
    if (LevelManager.instance != null) {
      LevelManager.instance.OnLevelStarted -= HandleLevelStarted;
      LevelManager.instance.OnLevelEnded -= HandleLevelEnded;
    }
  }

  private void HandleLevelStarted(BaseLevel level) {
    gameplayHUDAnimator.gameObject.SetActive(true);
    gameplayHUDAnimator.SetBool("active", true);
  }

  private void HandleLevelEnded(BaseLevel level) {
    gameplayHUDAnimator.SetBool("active", false);
    StartCoroutine(DisableAfterAnimation());
  }
  
  private IEnumerator DisableAfterAnimation() {
    yield return new WaitForSeconds(0.5f);
    gameplayHUDAnimator.gameObject.SetActive(false);
  }
}
