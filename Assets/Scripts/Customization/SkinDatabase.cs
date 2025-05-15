using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skins/SkinDatabase")]
public class SkinDatabase : ScriptableObject {
  public List<MarbleSkin> marbles;
  public List<TrailSkin> trails;
  public List<AccessorySkin> accessories;
}
