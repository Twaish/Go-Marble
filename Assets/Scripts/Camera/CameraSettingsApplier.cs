using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraSettingsApplier : MonoBehaviour {
  private UniversalAdditionalCameraData cameraData;

  private void Awake() {
    cameraData = GetComponent<Camera>().GetUniversalAdditionalCameraData();

    SettingsManager.Instance.OnSettingsChanged += HandleSettingsChanged;
    HandleSettingsChanged(SettingsManager.Instance.Settings);
  }

  private void OnDestroy() {
    SettingsManager.Instance.OnSettingsChanged -= HandleSettingsChanged;
  }


  private void HandleSettingsChanged(GameSettings settings) {
    cameraData.renderPostProcessing = settings.enablePostProcessing;
  }

}
