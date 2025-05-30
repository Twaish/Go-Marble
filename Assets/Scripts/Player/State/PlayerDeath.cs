using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public float LevelRestartDelay = 2f; // Delay before restarting
    public GameObject deathEffectPrefab; // Optional

    public void Die()
    {
        // Play death effect
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // Hide the player
        gameObject.SetActive(false);

        // Tell the LevelManager to restart after delay
        if (LevelManager.instance != null)
        {
            LevelManager.instance.PlayerDeathRestartLevel(LevelRestartDelay);
        }
        else
        {
            Debug.LogError("LevelManager not found!");
        }
    }
}
