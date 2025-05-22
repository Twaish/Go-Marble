using UnityEngine;

public class ParticleLightSync : MonoBehaviour {
  [SerializeField] private Light pointLight;
  [SerializeField] private ParticleSystem targetParticleSystem;
  [SerializeField] private AnimationCurve intensityOverTime = AnimationCurve.Linear(0, 1, 1, 0);
  [SerializeField] private float maxIntensity = 5f;

  private float particleStartTime;
  private float particleLifetime;
  private bool isActive;

  void Awake() {
    pointLight.intensity = 0f;
  }

  void OnEnable() {
    var main = targetParticleSystem.main;
    if (main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants) {
      particleLifetime = main.startLifetime.constantMax;
    }
    else {
      particleLifetime = main.startLifetime.constant;
    }

    targetParticleSystem.Play(true);
    particleStartTime = Time.time;
    pointLight.intensity = maxIntensity;
    isActive = true;

    particleStartTime = Time.time;

    pointLight.intensity = maxIntensity;
  }

  void Update() {
    if (!isActive) return;
    
    if (!targetParticleSystem.IsAlive()) {
      pointLight.intensity = 0f;
      isActive = false;
      return;
    }

    float elapsed = Time.time - particleStartTime;
    float t = Mathf.Clamp01(elapsed / particleLifetime);
    float evaluated = intensityOverTime.Evaluate(t);
    pointLight.intensity = maxIntensity * evaluated;
  }
}