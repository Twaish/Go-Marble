using UnityEngine;

public class MarbleSkinHandler : MonoBehaviour {
  [SerializeField] private GameObject defaultMarble;
  private GameObject currentSkin;

  private void AddMarbleSkin(GameObject marbleModel) {
    if (currentSkin != null) {
      Destroy(currentSkin);
    }
    currentSkin = Instantiate(marbleModel, transform);
  }
  
  public void Apply(MarbleSkin marble) {
    if (marble == null) {
      if (currentSkin != null) {
        Destroy(currentSkin);
        currentSkin = null;
      }

      defaultMarble.SetActive(true);
      return;
    }

    defaultMarble.SetActive(false);
    AddMarbleSkin(marble.marblePrefab);

    Debug.Log("APPLYING MARBLE: " + marble.skinName);
  }
}
