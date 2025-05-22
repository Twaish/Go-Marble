using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public TMP_Text timerText;
    private float timeElapsed;
    private bool isTiming = true;

    void Start()
    {
        if (timerText != null)
        {
            return;
        }
    
        timerText = GetComponentInChildren<TMP_Text>();

        if (timerText == null)
        {
            Debug.LogError(
                "Timer Text not found! Make sure the TextMeshPro component is a child of TimerManager."
            );
        }
    }

    void Update()
    {
        if (isTiming)
        {
            timeElapsed += Time.deltaTime;
            timerText.text =  timeElapsed.ToString("F2"); // Display with 2 decimals
        }
    }

    public void StopTimer()
    {
        isTiming = false;
    }
}
