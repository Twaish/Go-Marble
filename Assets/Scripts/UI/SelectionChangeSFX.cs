using UnityEngine;

public class SelectionChangeSFX : MonoBehaviour {
  [SerializeField] private string soundName;

  private SoundPlayer soundPlayer;
  private BorderCursor borderCursor;

  private void Awake() {
    soundPlayer = GetComponent<SoundPlayer>();
    borderCursor = GetComponent<BorderCursor>();
    borderCursor.OnSelectionChanged += HandleSelectionChanged;
  }

  private void OnDestroy() {
    borderCursor.OnSelectionChanged -= HandleSelectionChanged;
  }

  private void HandleSelectionChanged() {
    soundPlayer.PlaySound(soundName);
  }
}
