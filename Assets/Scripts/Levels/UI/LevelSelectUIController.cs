using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUIController : MonoBehaviour {
  [SerializeField] private GameObject levelButtonPrefab;
  [SerializeField] private Transform levelGridParent;

  [SerializeField] private AutoScrollController levelScrollController;

  private Dictionary<BaseLevel, LevelTileUI> buttonLookup = new();
  private LevelMedalEvaluator levelMedalEvaluator;

  private void Awake() {
    levelMedalEvaluator = GetComponent<LevelMedalEvaluator>();
  }

  public void PopulateLevelButtons(
    IEnumerable<BaseLevel> levels, 
    Action<BaseLevel> onLevelClicked, 
    Action<BaseLevel> onLevelFocused, 
    BaseLevel selectedLevel
  ) {
    buttonLookup.Clear();
    foreach (Transform child in levelGridParent) {
      Destroy(child.gameObject);
    }

    foreach (var level in levels) {
      GameObject go = Instantiate(levelButtonPrefab, levelGridParent);
      LevelTileUI button = go.GetComponent<LevelTileUI>();
      button.Setup(level, onLevelClicked, data => {
        onLevelFocused(data);
        levelScrollController.CenterOnItem(go);
      }, levelMedalEvaluator);
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