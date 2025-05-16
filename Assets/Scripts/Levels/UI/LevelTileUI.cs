using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelTileUI : MonoBehaviour, IPointerEnterHandler, ISelectHandler {
  [SerializeField] private Image background;
  [SerializeField] private Color normalColor = Color.white;
  [SerializeField] private Color selectedColor = Color.yellow;

  [SerializeField] private Image previewImage;
  [SerializeField] private MedalUI medalUI;
  [SerializeField] private TextMeshProUGUI text;

  private BaseLevel levelData;
  private Action<BaseLevel> onClick;
  private Action<BaseLevel> onFocus;
  private LevelMedalEvaluator levelMedalEvaluator;

  public void Setup(BaseLevel levelData, Action<BaseLevel> onClick, Action<BaseLevel> onFocus, LevelMedalEvaluator levelMedalEvaluator) {
    this.onClick = onClick;
    this.onFocus = onFocus;
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

  public void OnPointerEnter(PointerEventData eventData) {
    onFocus?.Invoke(levelData);
  }

  public void OnSelect(BaseEventData eventData) {
    onFocus?.Invoke(levelData);
  }
  
  public void SetSelected(bool isSelected) {
    if (isSelected) {
      Debug.Log(levelData.levelName);
    }
    text.text = isSelected ? "X" : "";
    // background.color = isSelected ? selectedColor : normalColor;
  }

  public void RefreshMedal() {
    if (levelMedalEvaluator == null || levelData == null) return;
    medalUI.SetMedalTexture(levelMedalEvaluator.GetMedalTexture(levelData));
  }
}
