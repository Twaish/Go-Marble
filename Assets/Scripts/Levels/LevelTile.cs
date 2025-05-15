using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTile : MonoBehaviour {
  [SerializeField] 
  private Image background;
  [SerializeField] 
  private Color normalColor = Color.white;
  [SerializeField] 
  private Color selectedColor = Color.yellow;

  [SerializeField] private Image previewImage;
  [SerializeField] private MedalUI medalUI;

  private BaseLevel levelData;
  private Action<BaseLevel> onClick;
  private LevelMedalEvaluator levelMedalEvaluator;

  public void Setup(BaseLevel levelData, Action<BaseLevel> onClick, LevelMedalEvaluator levelMedalEvaluator) {
    this.onClick = onClick;
    this.levelData = levelData;
    this.levelMedalEvaluator = levelMedalEvaluator;
    
    medalUI.SetMedalTexture(
      levelMedalEvaluator.GetMedalTexture(levelData)
    );
    // previewImage.sprite = level.previewImage;
  }

  public void OnClick() {
    onClick?.Invoke(levelData);
  }
  
  public void SetSelected(bool isSelected) {
    if (isSelected) {
      Debug.Log(levelData.levelName);
    }
    // background.color = isSelected ? selectedColor : normalColor;
  }

  public void RefreshMedal() {
    if (levelMedalEvaluator == null || levelData == null) return;
    medalUI.SetMedalTexture(levelMedalEvaluator.GetMedalTexture(levelData));
  }
}
