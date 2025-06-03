using System.Collections.Generic;
using UnityEngine;

public class AccessorySkinHandler : MonoBehaviour {
  [SerializeField] private GameObject accessoryPivot;

  public void Apply(List<AccessorySkin> accessories) {
    if (accessoryPivot == null) {
      Debug.LogWarning("AccessorySkinHandler: No accessoryPivot defined");
      return;
    }
    foreach (Transform child in accessoryPivot.transform) {
      Destroy(child.gameObject);
    }

    var appliedNames = new List<string>();
    foreach (var accessory in accessories) {
      if (accessory == null) continue;
      Instantiate(accessory.accessoryPrefab, accessoryPivot.transform);
      appliedNames.Add(accessory.skinName);
    }
    
    if (appliedNames.Count > 0) {
      Debug.Log("[Accessories] " + string.Join(", ", appliedNames));
    }
  }
}
