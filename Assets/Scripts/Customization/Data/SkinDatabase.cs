using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skins/Skin Database")]
public class SkinDatabase : ScriptableObject {
  public List<MarbleSkin> marbles;
  public List<TrailSkin> trails;
  public List<AccessorySkin> accessories;
}
