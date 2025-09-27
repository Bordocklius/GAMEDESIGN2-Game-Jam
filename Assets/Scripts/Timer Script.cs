using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TimerScript : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float cooldownDuration = 60f; // 1 minute
    [SerializeField] private Text timerText;               // Assign a UI Text or TMP_Text component
    [SerializeField] private GameObject targetParent;      // Parent object containing target children

    private float _timeRemaining;
    private bool _timerRunning = false;

    private void Start()
    {
        _timeRemaining = cooldownDuration;
        _timerRunning = true;
    }

    private void Update()
    {
        if (_timerRunning)
        {
            // Check if all targets are hit
            if (AllTargetsHit())
            {
                StopTimer();
                return;
            }

            // Countdown
            _timeRemaining -= Time.deltaTime;

            if (_timeRemaining <= 0f)
            {
                _timeRemaining = 0f;
                StopTimer();
            }

            UpdateTimerUI();
        }
    }

    private void StopTimer()
    {
        _timerRunning = false;
        Debug.Log("Timer stopped (all targets hit or time expired).");
        // Optional: trigger event here
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private bool AllTargetsHit()
    {
        foreach (Transform child in targetParent.transform)
        {

            if (!child.GetComponentInChildren<Target>()._isHit) 
            {
             return false;
            }
        }
   
        return true;
    }
}