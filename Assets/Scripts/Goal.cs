using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private TimerManager timerManager; // Drag and drop the TimerManager GameObject

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            timerManager.StopTimer(); // Stop the timer
            Debug.Log("Goal Reached! Timer Stopped.");
        }
    }
}
