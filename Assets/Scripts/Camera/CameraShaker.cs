using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour {
  private Vector3 shakeOffset;
  private Vector3 targetShakeOffset;
  private float shakeTimeRemaining = 0;
  private float shakePower = 0;
  private float shakeFadeTime = 0;
  private float shakeRotationPower = 0;
  [SerializeField] 
  private float shakeInterpolationSpeed = 10f;
  
  public Vector3 GetShakeOffset() => shakeOffset;
  
  private void Update() {
    if (shakeTimeRemaining > 0) {
      shakeTimeRemaining -= Time.deltaTime;
      
      float xAmount = Random.Range(-1f, 1f) * shakePower;
      float yAmount = Random.Range(-1f, 1f) * shakePower;
      
      targetShakeOffset = new Vector3(xAmount, yAmount, 0);
      
      if (shakeTimeRemaining <= shakeFadeTime) {
        float fadePercent = shakeTimeRemaining / shakeFadeTime;
        shakePower = Mathf.Lerp(0, shakePower, fadePercent);
        shakeRotationPower = Mathf.Lerp(0, shakeRotationPower, fadePercent);
      }
    } else {
      targetShakeOffset = Vector3.zero;
    }

    shakeOffset = Vector3.Lerp(shakeOffset, targetShakeOffset, shakeInterpolationSpeed * Time.deltaTime);
  }
  
  public IEnumerator Shake(float duration, float magnitude) {
    shakeTimeRemaining = duration;
    shakePower = magnitude;
    shakeFadeTime = duration / 3;
    
    yield return null;
  }
}