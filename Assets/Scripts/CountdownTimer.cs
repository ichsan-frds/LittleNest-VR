using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 600f;
    private float remainingTime;
    public TextMeshProUGUI timerText;
    private bool isRunning = true;

    public IndicatorStatus indicatorStatus;
    public TaskManager taskManager;

    private float taskStartTime;
    private float maxTaskTime = 120f; // 2 menit per task
    private bool[] taskCompleted;
    private int currentTaskIndex = 0;
    private bool isTaskRunning = true;

    void Start()
    {
        remainingTime = totalTime;
        taskStartTime = totalTime;

        int totalTasks = indicatorStatus != null ? indicatorStatus.indicatorCircles.Count : 5;
        taskCompleted = new bool[totalTasks];

        if (timerText == null)
            Debug.LogWarning("❗ timerText belum di-assign!");
        if (taskManager == null)
            Debug.LogWarning("❗ taskManager belum di-assign!");

        UpdateTimerDisplay();
        taskManager?.ShowOnlyCurrentTask(currentTaskIndex); // 👈 Tampilkan task pertama saja
    }

    void Update()
    {
        if (!isRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (isTaskRunning && !taskCompleted[currentTaskIndex])
            {
                float elapsed = taskStartTime - remainingTime;

                if (elapsed > maxTaskTime)
                {
                    MarkCurrentTaskAsFailed();
                }
            }
        }
        else
        {
            remainingTime = 0;
            isRunning = false;
            UpdateTimerDisplay();
            Debug.Log("⏰ Timer selesai!");
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void MarkCurrentTaskAsSuccess()
    {
        if (IsCurrentTaskInvalid()) return;

        taskCompleted[currentTaskIndex] = true;
        indicatorStatus?.SetTaskStatus(currentTaskIndex, true);
        taskManager?.MarkCurrentTaskComplete();

        Debug.Log($"✅ Task {currentTaskIndex} selesai.");

        GoToNextTask();
    }

    void MarkCurrentTaskAsFailed()
    {
        if (IsCurrentTaskInvalid()) return;

        taskCompleted[currentTaskIndex] = true;
        indicatorStatus?.SetTaskStatus(currentTaskIndex, false);
        taskManager?.MarkCurrentTaskFailed();

        Debug.Log($"❌ Task {currentTaskIndex} GAGAL (lewat 2 menit).");

        GoToNextTask();
    }

    void GoToNextTask()
{
    isTaskRunning = false;
    currentTaskIndex++;

    if (currentTaskIndex < taskCompleted.Length)
    {
        taskStartTime = remainingTime;
        isTaskRunning = true;

        taskManager?.ShowOnlyCurrentTask(currentTaskIndex); // ✅ Munculin hanya task aktif
    }
    else
    {
        Debug.Log("🎉 Semua task selesai!");
        taskManager?.ShowOnlyCurrentTask(currentTaskIndex); // ❗ Tambahkan ini buat ilangin semua task
    }
}



    public void ResetTimer()
    {
        remainingTime = totalTime;
        isRunning = true;
        currentTaskIndex = 0;
        taskStartTime = totalTime;
        isTaskRunning = true;

        for (int i = 0; i < taskCompleted.Length; i++)
            taskCompleted[i] = false;

        indicatorStatus?.ResetAll();
        taskManager?.ResetTasks();

        UpdateTimerDisplay();
        taskManager?.ShowOnlyCurrentTask(currentTaskIndex);
        Debug.Log("🔄 Timer direset.");
    }

    private bool IsCurrentTaskInvalid()
    {
        return currentTaskIndex >= taskCompleted.Length || taskCompleted[currentTaskIndex];
    }
}
