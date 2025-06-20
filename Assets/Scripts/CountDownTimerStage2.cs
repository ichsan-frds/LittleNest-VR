using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimerStage2 : MonoBehaviour
{
    public float totalTime = 90f;
    private float remainingTime;
    public TextMeshProUGUI timerText;
    private bool isRunning = true;

    public IndicatorStatus indicatorStatus;
    public TaskManager2 taskManager;

    private float taskStartTime;
    public float maxTaskTime = 45f; // Durasi maksimal per task (1,5 menit)
    private bool[] taskCompleted;
    private int currentTaskIndex = 0;
    private bool isTaskRunning = true;

    public float delayBetweenTasks = 0.5f;

    void Awake()
    {
        int totalTasks = 2;
        if (taskManager != null)
        {
            totalTasks = taskManager.GetTotalTasks();
        }
        taskCompleted = new bool[totalTasks];

        for (int i = 0; i < taskCompleted.Length; i++)
        {
            taskCompleted[i] = false;
        }

        Debug.Log($"CountdownTimerStage2: taskCompleted array diinisialisasi di Awake dengan ukuran: {taskCompleted.Length}");
    }

    void Start()
    {
        remainingTime = totalTime;
        taskStartTime = totalTime;

        if (timerText == null)
            Debug.LogWarning("❗ CountdownTimerStage2: timerText belum di-assign di Inspector!");
        if (taskManager == null)
            Debug.LogWarning("❗ CountdownTimerStage2: taskManager belum di-assign di Inspector!");
        if (indicatorStatus == null)
            Debug.LogWarning("❗ CountdownTimerStage2: indicatorStatus belum di-assign di Inspector!");

        taskManager?.InitializeStage2(currentTaskIndex);
        indicatorStatus?.ResetAll();

        UpdateTimerDisplay();
        taskManager?.ShowOnlyCurrentTask(currentTaskIndex, TaskManager2.TaskResult.None);
    }

    void Update()
    {
        if (!isRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (isTaskRunning && taskCompleted != null && currentTaskIndex < taskCompleted.Length && !taskCompleted[currentTaskIndex])
            {
                float elapsed = taskStartTime - remainingTime;

                if (elapsed > maxTaskTime)
                {
                    // ✅ Matikan task SEBELUM tandai gagal supaya tidak terjadi eksekusi berulang
                    isTaskRunning = false;
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
        Debug.Log($"MarkCurrentTaskAsSuccess() dipanggil untuk Task Index: {currentTaskIndex}");

        if (IsCurrentTaskInvalid())
        {
            Debug.LogWarning($"Task {currentTaskIndex} sudah selesai atau di luar batas. Tidak dapat menandai sukses.");
            return;
        }

        isTaskRunning = false; // ✅ Hentikan task saat dinyatakan berhasil
        taskCompleted[currentTaskIndex] = true;
        indicatorStatus?.SetTaskStatus(currentTaskIndex, true);

        taskManager?.ShowOnlyCurrentTask(currentTaskIndex, TaskManager2.TaskResult.Success);

        Debug.Log($"✅ Task {currentTaskIndex} selesai oleh pemain.");
        GoToNextTask();
    }

    void MarkCurrentTaskAsFailed()
    {
        Debug.Log($"MarkCurrentTaskAsFailed() dipanggil untuk Task Index: {currentTaskIndex}");

        if (IsCurrentTaskInvalid())
        {
            Debug.LogWarning($"Task {currentTaskIndex} sudah selesai atau di luar batas. Tidak dapat menandai gagal.");
            return;
        }

        taskCompleted[currentTaskIndex] = true;
        indicatorStatus?.SetTaskStatus(currentTaskIndex, false);

        taskManager?.ShowOnlyCurrentTask(currentTaskIndex, TaskManager2.TaskResult.Failed);

        Debug.Log($"❌ Task {currentTaskIndex} GAGAL (lewat batas waktu).");
        GoToNextTask();
    }

    void GoToNextTask()
    {
        currentTaskIndex++;
        StartCoroutine(DelayNextTask());
    }

    IEnumerator DelayNextTask()
    {
        Debug.Log($"➡️ Berpindah ke task berikutnya. Menunggu {delayBetweenTasks} detik...");
        yield return new WaitForSeconds(delayBetweenTasks);

        if (taskCompleted != null && currentTaskIndex < taskCompleted.Length)
        {
            taskStartTime = remainingTime;
            isTaskRunning = true;
            Debug.Log($"▶️ Task {currentTaskIndex} dimulai.");
            taskManager?.ShowOnlyCurrentTask(currentTaskIndex, TaskManager2.TaskResult.None);
        }
        else
        {
            Debug.Log("🎉 Semua task selesai!");
            taskManager?.ShowOnlyCurrentTask(currentTaskIndex, TaskManager2.TaskResult.None);
            isRunning = false;
        }
    }

    public void ResetTimer()
    {
        remainingTime = totalTime;
        isRunning = true;
        currentTaskIndex = 0;
        taskStartTime = totalTime;
        isTaskRunning = true;

        if (taskCompleted == null || taskCompleted.Length == 0)
        {
            int totalTasks = (taskManager != null) ? taskManager.GetTotalTasks() : 2;
            taskCompleted = new bool[totalTasks];
            Debug.Log($"CountdownTimerStage2: taskCompleted array diinisialisasi ulang di ResetTimer dengan ukuran: {taskCompleted.Length}");
        }

        for (int i = 0; i < taskCompleted.Length; i++)
            taskCompleted[i] = false;

        indicatorStatus?.ResetAll();
        taskManager?.InitializeStage2(currentTaskIndex);

        UpdateTimerDisplay();
        Debug.Log("🔄 Timer direset.");
    }

    public int GetCurrentActiveTaskIndex()
    {
        return currentTaskIndex;
    }

    private bool IsCurrentTaskInvalid()
    {
        if (taskCompleted == null)
        {
            Debug.LogError("FATAL ERROR: taskCompleted array is NULL in IsCurrentTaskInvalid! (Still null after Awake/Start)");
            return true;
        }

        return currentTaskIndex >= taskCompleted.Length || taskCompleted[currentTaskIndex];
    }
}
