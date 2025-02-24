using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene reloading

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure your player has the tag "Player"
        {
            // Reload the current scene or handle respawn
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
