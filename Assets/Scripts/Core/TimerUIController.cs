using TMPro;
using UnityEngine;

public class TimerUIController : MonoBehaviour {
  [SerializeField] private TMP_Text timerText;

  private TimerManager timerManager;

  private void Awake() {
    if (timerText == null) {
      Debug.LogError("TimerUIController: timerText not assigned");
      enabled = false;
      return;
    }

    timerManager = GetComponent<TimerManager>();
    if (timerManager == null) {
      Debug.LogError("TimerUIController: TimerManager not found");
      enabled = false;
      return;
    }

    timerManager.OnTimerUpdated += UpdateTimerText;
    timerManager.OnTimerStopped += UpdateTimerText;
    timerManager.OnTimerReset += ClearText;
  }

  private void OnDestroy() {
    if (timerManager == null) return;
    timerManager.OnTimerUpdated -= UpdateTimerText;
    timerManager.OnTimerReset -= ClearText;
  }

  private void UpdateTimerText(float time) {
    int minutes = Mathf.FloorToInt(time / 60f);
    int seconds = Mathf.FloorToInt(time % 60f);
    int hundredths = Mathf.FloorToInt(time * 100f % 100f);

    timerText.text = $"{minutes:00}:{seconds:00}.{hundredths:00}s";
  }

  private void ClearText() {
    timerText.text = "00:00.00s";
  }
}
