using System;
using TMPro;
using UnityEngine;

public class LevelResultUIController : MonoBehaviour {
  [SerializeField] private TimerManager timerManager;

  [Header("UI Elements")]
  [SerializeField] private MedalUI clearMedal;
  [SerializeField] private TextMeshProUGUI clearTime;

  private LevelMedalEvaluator levelMedalEvaluator;
  private LevelResultsRepository levelResultsRepository;

  private void Awake() {
    levelMedalEvaluator = GetComponent<LevelMedalEvaluator>();
    levelResultsRepository = GetComponent<LevelResultsRepository>();
  }

  public void UpdateUI(BaseLevel level) {
    float timeInSeconds = timerManager.GetTime();
    TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
    clearTime.text = $"Time {time:mm\\:ss\\.ff}s";
    clearMedal.SetMedalTexture(levelMedalEvaluator.EvaluateMedalTexture(level, timeInSeconds));
  }
}
