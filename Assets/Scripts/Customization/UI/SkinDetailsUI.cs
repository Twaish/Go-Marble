using TMPro;
using UnityEngine;

public class SkinDetailsUI : MonoBehaviour {
  [SerializeField] private TextMeshProUGUI skinName;
  [SerializeField] private TextMeshProUGUI skinDescription;

  public void SetSkin(BaseSkin skin) {
    if (skin == null) {
      skinName.text = "None";
      skinDescription.text = "None";
      return;
    }
    skinName.text = skin.skinName;
    skinDescription.text = skin.description;
  }
}
