using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public TextMeshProUGUI taskText;
    public IndicatorStatus indicatorStatus;

    private string[] tasks = {
        "Memandikan",
        "Mengganti Popok",
        "Menenangkan Bayi",
        "Memberi Makan",
        "Memberi Susu"
    };

    private int currentTaskIndex = 0;
    private bool isBottleGiven = false;

    private enum TaskResult { None, Success, Failed }
    private TaskResult[] taskResults;

    public enum TaskStatus { None, Success, Failed }

    void Start()
    {
        taskResults = new TaskResult[tasks.Length];
        ShowOnlyCurrentTask(currentTaskIndex);
    }

    void UpdateTaskText()
    {
        if (taskText == null) return;

        string result = "Task List\n";
        for (int i = 0; i < tasks.Length; i++)
        {
            if (taskResults[i] == TaskResult.Success)
            {
                result += $"<color=green>{i + 1}. ✔ {tasks[i]}</color>\n";
            }
            else if (taskResults[i] == TaskResult.Failed)
            {
                result += $"<color=red>{i + 1}. ✘ {tasks[i]}</color>\n";
            }
            else if (i == currentTaskIndex)
            {
                result += $"<b>{i + 1}. {tasks[i]}</b>\n";
            }
        }

        taskText.text = result.TrimEnd();
    }

    public void ShowOnlyCurrentTask(int index)
    {
        if (taskText == null) return;

        string result = "Task List\n";
        if (index < tasks.Length)
        {
            if (taskResults[index] == TaskResult.Success)
                result += $"<color=green>{index + 1}. ✔ {tasks[index]}</color>\n";
            else if (taskResults[index] == TaskResult.Failed)
                result += $"<color=red>{index + 1}. ✘ {tasks[index]}</color>\n";
            else
                result += $"<b>{index + 1}. {tasks[index]}</b>\n";
        }
        else
        {
            result += "<color=yellow><b>Semua task selesai!</b></color>";
        }

        taskText.text = result.TrimEnd();
    }

    public void MarkCurrentTaskComplete()
    {
        if (currentTaskIndex >= tasks.Length) return;

        if (taskResults[currentTaskIndex] == TaskResult.Failed)
        {
            Debug.Log($"⚠️ Task {currentTaskIndex} sudah gagal. Tidak bisa ditandai sukses.");
            return;
        }

        taskResults[currentTaskIndex] = TaskResult.Success;
        indicatorStatus?.SetTaskStatus(currentTaskIndex, true);
        Debug.Log($"✅ Task {currentTaskIndex} ditandai berhasil.");
        UpdateTaskText();
    }

    public void MarkCurrentTaskFailed()
    {
        if (currentTaskIndex >= tasks.Length) return;

        taskResults[currentTaskIndex] = TaskResult.Failed;
        indicatorStatus?.SetTaskStatus(currentTaskIndex, false);
        Debug.Log($"❌ Task {currentTaskIndex} ditandai gagal.");
        UpdateTaskText();
    }

    public void NextTask()
    {
        currentTaskIndex++;

        if (currentTaskIndex < tasks.Length)
        {
            Debug.Log($"➡️ Pindah ke Task {currentTaskIndex}: {tasks[currentTaskIndex]}");
            ShowOnlyCurrentTask(currentTaskIndex);
        }
        else
        {
            taskText.text += "\n<color=yellow><b>Semua task selesai!</b></color>";
            Debug.Log("🎉 Semua task selesai!");
        }
    }

    public int GetCurrentTaskIndex()
    {
        return currentTaskIndex;
    }

    public int GetTaskCount()
    {
        return tasks.Length;
    }

    public TaskStatus GetTaskResult(int index)
    {
        if (index < 0 || index >= taskResults.Length)
            return TaskStatus.None;

        return taskResults[index] switch
        {
            TaskResult.Success => TaskStatus.Success,
            TaskResult.Failed => TaskStatus.Failed,
            _ => TaskStatus.None
        };
    }

    public bool IsTaskFailed(int index)
    {
        if (index < 0 || index >= taskResults.Length)
            return false;

        return taskResults[index] == TaskResult.Failed;
    }

    public void SetBottleGiven()
    {
        isBottleGiven = true;
    }

    public bool CanSleep()
    {
        return currentTaskIndex == tasks.Length - 1 && isBottleGiven;
    }

    public void ResetTasks()
    {
        currentTaskIndex = 0;
        isBottleGiven = false;
        taskResults = new TaskResult[tasks.Length];
        indicatorStatus?.ResetAll();
        ShowOnlyCurrentTask(currentTaskIndex);
        Debug.Log("🔄 Semua task direset.");
    }

    // ✅ Tambahan penting: dipakai di CountdownTimer
    public int GetSuccessCount()
    {
        int count = 0;
        foreach (var result in taskResults)
        {
            if (result == TaskResult.Success) count++;
        }
        return count;
    }

    public int GetFailureCount()
    {
        int count = 0;
        foreach (var result in taskResults)
        {
            if (result == TaskResult.Failed) count++;
        }
        return count;
    }
}
