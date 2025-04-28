using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene reloading

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the player's death
            PlayerDeath playerDeath = other.GetComponent<PlayerDeath>();
            if (playerDeath != null)
            {
                playerDeath.Die();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Trigger the player's death
            PlayerDeath playerDeath = collision.gameObject.GetComponent<PlayerDeath>();
            if (playerDeath != null)
            {
                playerDeath.Die();
            }
        }
    }
}
