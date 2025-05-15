using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinTile : MonoBehaviour, ISelectHandler {
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
    // background.color = isSelected ? selectedColor : normalColor;
  }

  [SerializeField]
  private SkinTileEvent onSelectEvent;
  [SerializeField]
  private TMP_Text text;

  public SkinTileEvent OnSelectEvent {
    get => onSelectEvent;
    set => onSelectEvent = value;
  }

  public string Text {
    get => text.text;
    set => text.text = value;
  }

  public void OnSelect(BaseEventData eventData) {
    onSelectEvent.Invoke(this);
  }

  public void ObtainSelectionFocus() {
    EventSystem.current.SetSelectedGameObject(gameObject);
    onSelectEvent.Invoke(this);
  }
}

[System.Serializable]
public class SkinTileEvent : UnityEvent<SkinTile>{}