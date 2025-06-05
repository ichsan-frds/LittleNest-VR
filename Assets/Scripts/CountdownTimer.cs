using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 600f;
    private float remainingTime;
    public TextMeshProUGUI timerText;
    private bool isRunning = true;

    public IndicatorStatus indicatorStatus;
    public TaskManager taskManager;

    private float taskStartTime;
    private float maxTaskTime = 120f;
    private bool[] taskCompleted;
    private int currentTaskIndex = 0;

    private bool isTaskRunning = true;
    private bool isTransitioning = false;

    void Start()
    {
        remainingTime = totalTime;
        taskStartTime = Time.time;

        int totalTasks = indicatorStatus != null ? indicatorStatus.indicatorCircles.Count : 5;
        taskCompleted = new bool[totalTasks];

        if (timerText == null) Debug.LogWarning("❗ timerText belum di-assign!");
        if (taskManager == null) Debug.LogWarning("❗ taskManager belum di-assign!");

        UpdateTimerDisplay();
        taskManager?.ShowOnlyCurrentTask(currentTaskIndex);
        Debug.Log($"🏁 Task {currentTaskIndex} dimulai pada Time.time: {Time.time:F2}");
    }

    void Update()
    {
        if (!isRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (!isTransitioning && isTaskRunning && IsValidTaskIndex(currentTaskIndex) && !taskCompleted[currentTaskIndex])
            {
                float elapsedTaskTime = Time.time - taskStartTime;
                
                if (elapsedTaskTime >= maxTaskTime)
                {
                    Debug.Log($"⏰ Task {currentTaskIndex} melebihi waktu ({elapsedTaskTime:F2}s)");
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

        var currentStatus = taskManager.GetTaskResult(currentTaskIndex);
        if (currentStatus == TaskManager.TaskStatus.Failed)
        {
            Debug.Log($"⚠️ Task {currentTaskIndex} sudah gagal, tidak bisa diubah ke sukses.");
            GoToNextTask();
            return;
        }

        if (!taskCompleted[currentTaskIndex])
        {
            float elapsedTaskTime = Time.time - taskStartTime;
            taskCompleted[currentTaskIndex] = true;
            isTaskRunning = false;

            indicatorStatus?.SetTaskStatus(currentTaskIndex, true);
            taskManager?.MarkCurrentTaskComplete();

            Debug.Log($"✅ Task {currentTaskIndex} selesai dalam {elapsedTaskTime:F2} detik.");
            
            // ✅ PERBAIKAN: Hapus duplikasi - hanya GoToNextTask() yang akan handle NextTask()
            GoToNextTask();
        }
    }

    void MarkCurrentTaskAsFailed()
    {
        if (IsCurrentTaskInvalid()) return;

        if (!taskCompleted[currentTaskIndex])
        {
            float elapsedTaskTime = Time.time - taskStartTime;
            taskCompleted[currentTaskIndex] = true;
            isTaskRunning = false;

            indicatorStatus?.SetTaskStatus(currentTaskIndex, false);
            taskManager?.MarkCurrentTaskFailed();

            Debug.Log($"❌ Task {currentTaskIndex} GAGAL setelah {elapsedTaskTime:F2} detik (batas: {maxTaskTime}s).");
            
            // ✅ PERBAIKAN: Hapus duplikasi - hanya GoToNextTask() yang akan handle NextTask()
            GoToNextTask();
        }
    }

    void GoToNextTask()
    {
        isTaskRunning = false;
        isTransitioning = true;
        
        // ✅ PERBAIKAN: Panggil NextTask() di sini saja, tidak di MarkCurrentTaskAsSuccess/Failed
        taskManager?.NextTask();
        
        currentTaskIndex++;

        if (currentTaskIndex < taskCompleted.Length)
        {
            StartCoroutine(DelayStartNextTask());
        }
        else
        {
            Debug.Log("🎉 Semua task selesai!");
            taskManager?.ShowOnlyCurrentTask(currentTaskIndex);
            StartCoroutine(DelayCheckEnding());
        }
    }

    IEnumerator DelayStartNextTask()
    {
        yield return new WaitForEndOfFrame();

        taskStartTime = Time.time;
        isTaskRunning = true;
        isTransitioning = false;

        taskManager?.ShowOnlyCurrentTask(currentTaskIndex);
        Debug.Log($"➡️ Task {currentTaskIndex} dimulai pada Time.time: {taskStartTime:F2}");
        
        // ✅ Debug tambahan untuk memastikan sinkronisasi
        Debug.Log($"[SYNC CHECK] CountdownTimer.currentTaskIndex = {currentTaskIndex}, TaskManager.GetCurrentTaskIndex() = {taskManager?.GetCurrentTaskIndex()}");
    }

    private bool IsCurrentTaskInvalid()
    {
        return !IsValidTaskIndex(currentTaskIndex) || taskCompleted[currentTaskIndex];
    }

    private bool IsValidTaskIndex(int index)
    {
        return index >= 0 && index < taskCompleted.Length;
    }

    public void NotifyTaskSuccessFromInteraction()
    {
        Debug.Log($"🟢 Task {currentTaskIndex} mendapat notifikasi sukses dari interaksi eksternal.");
        MarkCurrentTaskAsSuccess();
    }

    public void ResetTimer()
    {
        remainingTime = totalTime;
        isRunning = true;
        currentTaskIndex = 0;
        taskStartTime = Time.time;
        isTaskRunning = true;
        isTransitioning = false;

        for (int i = 0; i < taskCompleted.Length; i++)
            taskCompleted[i] = false;

        indicatorStatus?.ResetAll();
        taskManager?.ResetTasks();

        UpdateTimerDisplay();
        taskManager?.ShowOnlyCurrentTask(currentTaskIndex);
        Debug.Log($"🔄 Timer direset. Task 0 dimulai pada: {taskStartTime:F2}");
    }

    public void GetCurrentTaskInfo()
    {
        if (IsValidTaskIndex(currentTaskIndex))
        {
            float elapsed = Time.time - taskStartTime;
            Debug.Log($"📊 Task {currentTaskIndex}: Elapsed {elapsed:F2}s / Max {maxTaskTime}s");
        }
    }

    public int GetCurrentTaskIndex()
    {
        return currentTaskIndex;
    }

    private IEnumerator DelayCheckEnding()
    {
        yield return new WaitForSeconds(1f);

        int success = taskManager.GetSuccessCount();
        int failed = taskManager.GetFailureCount();

        Debug.Log($"[ENDING CHECK] ✔: {success}, ✘: {failed}");

        if (success > failed)
        {
            Debug.Log("✅ GOOD ENDING! Pindah ke scene: Success");
            SceneManager.LoadScene("Success");
        }
        else
        {
            Debug.Log("❌ BAD ENDING! Pindah ke scene: Failed");
            SceneManager.LoadScene("Failed");
        }
    }
}