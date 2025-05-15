using UnityEngine;
using UnityEngine.UI;

public class MedalUI : MonoBehaviour {
  [SerializeField] private RawImage medalImageContainer;

  public void SetMedalTexture(Texture texture) {
    medalImageContainer.enabled = texture != null;
    medalImageContainer.texture = texture;
  }
}
