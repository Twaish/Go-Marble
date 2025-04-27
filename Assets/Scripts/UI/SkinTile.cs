using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkinTile : MonoBehaviour, ISelectHandler {

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