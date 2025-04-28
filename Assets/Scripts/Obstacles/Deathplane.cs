using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene reloading

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell the player to die
            PlayerDeath playerDeath = other.GetComponent<PlayerDeath>();
            if (playerDeath != null)
            {
                playerDeath.Die();
            }
        }
    }
}
