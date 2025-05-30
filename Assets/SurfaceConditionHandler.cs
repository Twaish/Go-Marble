using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SurfaceCondition {
  public string surfaceTag;

  [Header("Movement Modifiers")]
  public float speed;
  public float turnSpeed;
  public float decelerationRate;

  [Header("Physics Modifiers")]
  public float torque;
  public float bounciness;
}

public class SurfaceConditionHandler : MonoBehaviour {
  [SerializeField] private SurfaceCondition defaultCondition;
  [SerializeField] private List<SurfaceCondition> surfaceConditions;

  public SurfaceCondition GetCondition(GameObject other) {
    SurfaceCondition match = surfaceConditions.Find(c => other.CompareTag(c.surfaceTag));
    return match ?? defaultCondition;
  }
}
