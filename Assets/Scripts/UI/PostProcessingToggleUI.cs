using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PostProcessingToggleUI : MonoBehaviour {
  [SerializeField] private Toggle toggle;
  [SerializeField] private Volume globalVolume;

  private void Awake() {
    toggle.isOn = SettingsManager.Instance.Settings.enablePostProcessing;
    toggle.onValueChanged.AddListener(OnToggleChanged);
    SettingsManager.Instance.OnSettingsChanged += HandleSettingsChanged;
  }

  private void OnDestroy() {
    toggle.onValueChanged.RemoveListener(OnToggleChanged);
    SettingsManager.Instance.OnSettingsChanged -= HandleSettingsChanged;
  }

  private void OnToggleChanged(bool enabled) {
    SettingsManager.Instance.UpdateSetting(settings => {
      settings.enablePostProcessing = enabled;
    });
  }

  private void HandleSettingsChanged(GameSettings settings) {
    toggle.SetIsOnWithoutNotify(settings.enablePostProcessing);
  }
}
