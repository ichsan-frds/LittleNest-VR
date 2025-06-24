using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class CountdownTimerStage1 : MonoBehaviour
{
    public float totalTime = 600f;
    private float remainingTime;
    public TextMeshProUGUI timerText;
    private bool isRunning = true;

    public IndicatorStatus indicatorStatus;
    public TaskManager taskManager;
    public SpoonInteraction spoonInteraction;

    private float taskStartTime;
    private float maxTaskTime = 90f;
    private bool[] taskCompleted;

    private bool isTaskRunning = true;
    private bool isTransitioning = false;
    private bool isReadyToTrackTime = false;

    void Start()
    {
        remainingTime = totalTime;

        int totalTasks = indicatorStatus != null ? indicatorStatus.indicatorCircles.Count : 5;
        taskCompleted = new bool[totalTasks];

        if (timerText == null) Debug.LogWarning("❗ timerText belum di-assign!");
        if (taskManager == null) Debug.LogWarning("❗ taskManager belum di-assign!");

        isTaskRunning = true;
        isTransitioning = false;
        isReadyToTrackTime = false;

        UpdateTimerDisplay();

        int currentTaskIndex = taskManager.GetCurrentTaskIndex();
        taskManager?.ShowOnlyCurrentTask(currentTaskIndex);
        StartCoroutine(DelayStartNextTask());
    }

    void Update()
    {
        if (!isRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();

            int currentTaskIndex = taskManager.GetCurrentTaskIndex();

            if (!isTransitioning && isTaskRunning && isReadyToTrackTime &&
                IsValidTaskIndex(currentTaskIndex) && !taskCompleted[currentTaskIndex])
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
        int currentTaskIndex = taskManager.GetCurrentTaskIndex();

        if (IsCurrentTaskInvalid(currentTaskIndex)) return;

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
            GoToNextTask();
        }
    }

    void MarkCurrentTaskAsFailed()
    {
        int currentTaskIndex = taskManager.GetCurrentTaskIndex();

        if (IsCurrentTaskInvalid(currentTaskIndex)) return;

        if (!taskCompleted[currentTaskIndex])
        {
            float elapsedTaskTime = Time.time - taskStartTime;
            taskCompleted[currentTaskIndex] = true;
            isTaskRunning = false;

            indicatorStatus?.SetTaskStatus(currentTaskIndex, false);
            taskManager?.MarkCurrentTaskFailed();

            Debug.Log($"❌ Task {currentTaskIndex} GAGAL setelah {elapsedTaskTime:F2} detik (batas: {maxTaskTime}s).");
            GoToNextTask();
        }
    }

    void GoToNextTask()
    {
        isTaskRunning = false;
        isTransitioning = true;
        isReadyToTrackTime = false;

        taskManager?.NextTask();

        int nextIndex = taskManager.GetCurrentTaskIndex();
        if (nextIndex < taskCompleted.Length)
        {
            StartCoroutine(DelayStartNextTask());
        }
        else
        {
            Debug.Log("🎉 Semua task selesai!");
            taskManager?.ShowOnlyCurrentTask(nextIndex);
            StartCoroutine(DelayCheckEnding());
        }
    }

    IEnumerator DelayStartNextTask()
    {
        yield return new WaitForEndOfFrame();

        taskStartTime = Time.time;
        isTaskRunning = true;
        isTransitioning = false;
        isReadyToTrackTime = true;

        int currentTaskIndex = taskManager.GetCurrentTaskIndex();
        taskManager?.ShowOnlyCurrentTask(currentTaskIndex);

        Debug.Log($"➡️ Task {currentTaskIndex} dimulai pada Time.time: {taskStartTime:F2}");
        Debug.Log($"[SYNC CHECK] CountdownTimer.GetCurrentTaskIndex() = {currentTaskIndex}");
    }

    private bool IsCurrentTaskInvalid(int index)
    {
        return !IsValidTaskIndex(index) || taskCompleted[index];
    }

    private bool IsValidTaskIndex(int index)
    {
        return index >= 0 && index < taskCompleted.Length;
    }

    public void NotifyTaskSuccessFromInteraction()
    {
        int currentTaskIndex = taskManager.GetCurrentTaskIndex();
        Debug.Log($"🟢 Task {currentTaskIndex} mendapat notifikasi sukses dari interaksi eksternal.");
        MarkCurrentTaskAsSuccess();
    }

    public void ResetTimer()
    {
        remainingTime = totalTime;
        isRunning = true;
        taskStartTime = Time.time;
        isTaskRunning = true;
        isTransitioning = false;
        isReadyToTrackTime = true;

        for (int i = 0; i < taskCompleted.Length; i++)
            taskCompleted[i] = false;

        indicatorStatus?.ResetAll();
        taskManager?.ResetTasks();
        spoonInteraction?.ResetFeeding();

        UpdateTimerDisplay();
        taskManager?.ShowOnlyCurrentTask(taskManager.GetCurrentTaskIndex());
        Debug.Log($"🔄 Timer direset. Task {taskManager.GetCurrentTaskIndex()} dimulai pada: {taskStartTime:F2}");
    }

    public void GetCurrentTaskInfo()
    {
        int currentTaskIndex = taskManager.GetCurrentTaskIndex();
        if (IsValidTaskIndex(currentTaskIndex))
        {
            float elapsed = Time.time - taskStartTime;
            Debug.Log($"📊 Task {currentTaskIndex}: Elapsed {elapsed:F2}s / Max {maxTaskTime}s");
        }
    }

    public int GetCurrentTaskIndex()
    {
        return taskManager?.GetCurrentTaskIndex() ?? -1;
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