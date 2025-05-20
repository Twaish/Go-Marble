using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinTileUI : MonoBehaviour, ISelectHandler {
  [SerializeField] private Image icon;
  [SerializeField] private TextMeshProUGUI title;

  private Action onClick;

  private BaseSkin skin;
  public BaseSkin Skin => skin;

  public void Setup(BaseSkin skin, Action onClick) {
    // icon.sprite = skin.icon;
    this.onClick = onClick;
    this.skin = skin;
  }

  public void OnClick() {
    onClick?.Invoke();
  }
  
  public void SetSelected(bool isSelected) {
    if (isSelected) {
      Debug.Log(skin?.skinName);
    }
    title.text = isSelected ? "X" : "";
  }

  [SerializeField] private SkinTileEvent onSelectEvent;

  public SkinTileEvent OnSelectEvent {
    get => onSelectEvent;
    set => onSelectEvent = value;
  }

  public void OnSelect(BaseEventData eventData) {
    onSelectEvent.Invoke(this);
  }
}

[Serializable]
public class SkinTileEvent : UnityEvent<SkinTileUI>{}