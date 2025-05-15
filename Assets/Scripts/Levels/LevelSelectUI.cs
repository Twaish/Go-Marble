using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : MonoBehaviour {
  [SerializeField] private GameObject levelButtonPrefab;
  [SerializeField] private Transform levelGridParent;

  private Dictionary<BaseLevel, LevelTile> buttonLookup = new();
  private LevelMedalEvaluator levelMedalEvaluator;

  private void Awake() {
    levelMedalEvaluator = GetComponent<LevelMedalEvaluator>();
  }

  public void PopulateLevelButtons(IEnumerable<BaseLevel> levels, Action<BaseLevel> onLevelClicked, BaseLevel selectedLevel) {
    buttonLookup.Clear();
    foreach (Transform child in levelGridParent) {
      Destroy(child.gameObject);
    }

    foreach (var level in levels) {
      GameObject go = Instantiate(levelButtonPrefab, levelGridParent);
      LevelTile button = go.GetComponent<LevelTile>();
      button.Setup(level, onLevelClicked, levelMedalEvaluator);
      buttonLookup[level] = button;
    }
    HighlightSelected(selectedLevel);
  }
  
  public void HighlightSelected(BaseLevel selected) {
    foreach (var kvp in buttonLookup) {
      kvp.Value.SetSelected(kvp.Key == selected);
    }
  }
  public void RefreshMedals() {
    foreach (var kvp in buttonLookup) {
      kvp.Value.RefreshMedal();
    }
  }
}