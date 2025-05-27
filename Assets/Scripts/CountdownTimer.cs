using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 150f; // 2 menit 30 detik = 150 detik
    private float remainingTime;
    public TextMeshProUGUI timerText;
    private bool isRunning = true;

    void Start()
    {
        remainingTime = totalTime;
    }

    void Update()
    {
        if (!isRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            remainingTime = 0;
            isRunning = false;
            UpdateTimerDisplay();
            Debug.Log("Timer selesai!");
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Bisa dipanggil dari tombol jika ingin reset/start ulang
    public void ResetTimer()
    {
        remainingTime = totalTime;
        isRunning = true;
    }
}
