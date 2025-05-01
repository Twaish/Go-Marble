using UnityEngine;

public abstract class BasePowerUp : ScriptableObject {
	public string Name;
	public Sprite Icon;
	public string Description;
	public int Uses;
	public int Cooldown;

	public abstract void Effect(PlayerController player);
	public virtual void OnActivate(PlayerController player) {}
	public virtual void OnExpire(PlayerController player) {}
}