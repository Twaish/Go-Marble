using UnityEngine;

public class MarbleSkinHandler : MonoBehaviour {
  [SerializeField] private GameObject defaultMarble;

  private void AddMarbleSkin(GameObject marbleModel) {
    foreach (Transform child in transform) {
      Destroy(child.gameObject);
    }
    Instantiate(marbleModel, transform);
  }
  
  public void Apply(MarbleSkin marble)
  {
    if (marble == null)
    {
      AddMarbleSkin(defaultMarble);
      return;
    }

    AddMarbleSkin(marble.marblePrefab);

    Debug.Log("APPLYING MARBLE: " + marble.skinName);
  }
}
