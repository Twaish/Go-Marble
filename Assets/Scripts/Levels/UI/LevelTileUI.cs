using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelTileUI : MonoBehaviour, IPointerEnterHandler, ISelectHandler {
  [SerializeField] private Image previewImage;
  [SerializeField] private MedalUI medalUI;
  [SerializeField] private TextMeshProUGUI text;

  private BaseLevel levelData;
  private Action<BaseLevel> onClick;
  private Action<BaseLevel> onFocus;
  private LevelMedalEvaluator levelMedalEvaluator;
  private Outline outline;

  public void Setup(BaseLevel levelData, Action<BaseLevel> onClick, Action<BaseLevel> onFocus, LevelMedalEvaluator levelMedalEvaluator) {
    this.onClick = onClick;
    this.onFocus = onFocus;
    this.levelData = levelData;
    this.levelMedalEvaluator = levelMedalEvaluator;

    outline = GetComponent<Outline>();

    medalUI.SetMedalTexture(
      levelMedalEvaluator.GetMedalTexture(levelData)
    );
    if (levelData.previewImage != null) {
      previewImage.sprite = levelData.previewImage;
    }
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
    text.text = isSelected ? "X" : "";
    outline.useGraphicAlpha = !isSelected;
  }

  public void RefreshMedal() {
    if (levelMedalEvaluator == null || levelData == null) return;
    medalUI.SetMedalTexture(levelMedalEvaluator.GetMedalTexture(levelData));
  }
}
