using System.Collections.Generic;
using UnityEngine;

public class AccessorySkinHandler : MonoBehaviour {
  [SerializeField] private GameObject accessoryPivot;
  
  public void Apply(List<AccessorySkin> accessories) {
    foreach (Transform child in accessoryPivot.transform) {
      Destroy(child.gameObject);
    }

    foreach (var accessory in accessories)
    {
      if (accessory == null) continue;
      Instantiate(accessory.accessoryPrefab, accessoryPivot.transform);
      Debug.Log("APPLYING ACCESSORY: " + accessory.skinName);
    }
  }
}
