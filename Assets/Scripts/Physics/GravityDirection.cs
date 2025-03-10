using UnityEngine;

// https://github.com/ThermiteFe8/Custom-Gravity-Physics-Unity/blob/main/Assets/Scripts/GravityUtils/GravPackageStruct.cs
public class GravityDirection : Object {
  public Vector3 gravity;
  public int priority;

  public GravityDirection(Vector3 gravity, int priority) {
    this.gravity = gravity;
    this.priority = priority;
  }
}

