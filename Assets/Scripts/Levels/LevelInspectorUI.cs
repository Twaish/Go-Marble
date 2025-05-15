using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInspectorUI : MonoBehaviour {
  [SerializeField] private TextMeshProUGUI levelNameText;
  [SerializeField] private TextMeshProUGUI levelDescriptionText;
  [SerializeField] private Image previewImage;
  [SerializeField] private MedalUI currentMedal;
  [SerializeField] private TextMeshProUGUI bestTime;

  [Header("Medal Times")]
  [SerializeField] private TextMeshProUGUI goldTime;
  [SerializeField] private TextMeshProUGUI silverTime;
  [SerializeField] private TextMeshProUGUI bronzeTime;

  private LevelMedalEvaluator levelMedalEvaluator;
  private LevelResultsRepository levelResultsRepository;

  private void Awake() {
    levelMedalEvaluator = GetComponent<LevelMedalEvaluator>();
    levelResultsRepository = GetComponent<LevelResultsRepository>();
  }

  public void UpdateUI(BaseLevel level) {
    levelNameText.text = level.levelName;
    levelDescriptionText.text = level.description;
    goldTime.text = FormatTime(level.medalThresholds.goldTime);
    silverTime.text = FormatTime(level.medalThresholds.silverTime);
    bronzeTime.text = FormatTime(level.medalThresholds.bronzeTime);

    currentMedal.SetMedalTexture(levelMedalEvaluator.GetMedalTexture(level));
    
    float? bestTimeForLevel = levelResultsRepository.GetBestTimeForLevel(level.levelName);
    bestTime.text = bestTimeForLevel.HasValue ? $"Best Time - {FormatTime(bestTimeForLevel.Value)}" : "";
    // previewImage.sprite = level.previewImage;
  }

  private string FormatTime(float timeInSeconds) {
    TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
    return time.ToString(@"mm\:ss\.ff");
  }
}
