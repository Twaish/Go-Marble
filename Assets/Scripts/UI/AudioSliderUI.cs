using UnityEngine;
using UnityEngine.UI;

public class AudioSliderUI : MonoBehaviour {
  [SerializeField] private Slider slider;

  private void Awake() {
    slider.value = SettingsManager.Instance.Settings.masterVolume;
    slider.onValueChanged.AddListener(HandleSliderValueChanged);
  }

  private void OnDestroy() {
    slider.onValueChanged.RemoveListener(HandleSliderValueChanged);
  }

  private void HandleSliderValueChanged(float value) {
    SettingsManager.Instance.UpdateSetting(settings => {
      settings.masterVolume = Mathf.Clamp01(value);
    });
  }
}
