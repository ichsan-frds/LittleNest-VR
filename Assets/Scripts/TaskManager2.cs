using UnityEngine;
using TMPro;

public class TaskManager2 : MonoBehaviour
{
    [Header("Referensi UI")]
    public TextMeshProUGUI taskText;
    public EmoteController babyEmoteController;

    public enum TaskResult { None, Success, Failed }

    // List task Anda
    private string[] tasks = {
        "Mengukur Suhu Bayi",
        "Memberi Obat",
        "Mengompres Bayi"
    };

    void Start()
    {
        if (babyEmoteController == null)
        {
            Debug.LogError("TaskManager2: babyEmoteController belum di-assign di Inspector!");
        }

        if (taskText == null)
        {
            Debug.LogError("TaskManager2: taskText belum di-assign di Inspector!");
        }
    }

    public void InitializeStage2(int startTaskIndex)
    {
        if (babyEmoteController != null)
        {
            babyEmoteController.ShowFeverEmote();
            Debug.Log("TaskManager2: Menampilkan emote demam pada bayi.");
        }

        ShowOnlyCurrentTask(startTaskIndex, TaskResult.None);
    }

    public void ShowOnlyCurrentTask(int index, TaskResult result = TaskResult.None)
    {
        if (taskText == null) return;

        string displayMessage = "Task List\n";
        if (index < tasks.Length)
        {
            string taskName = tasks[index];
            string statusPrefix = "";

            if (result == TaskResult.Success)
            {
                statusPrefix = "<color=green>✔</color> ";
                if (babyEmoteController != null) {
                    if (index == 0) // Mengukur Suhu Bayi
                        babyEmoteController.Show39DegreeTemperature();
                    else if (index == 1) // Memberi Obat
                        babyEmoteController.ShowHappy();
                    else if (index == 2) // Mengompres Bayi
                        babyEmoteController.ShowLove(); // Tampilkan emote love
                }
            }
            else if (result == TaskResult.Failed)
            {
                statusPrefix = "<color=red>✘</color> ";
                if (babyEmoteController != null) {
                    babyEmoteController.ShowAngry();
                }
            }
            else
            {
                statusPrefix = "<b>";
            }

            displayMessage += $"{statusPrefix}{index + 1}. {taskName}";
            if (result == TaskResult.None) displayMessage += "</b>";
        }
        else // Semua task sudah selesai
        {
            displayMessage = "<color=yellow><b>Semua task Stage 2 selesai!</b></color>";
            // babyEmoteController.ShowHappy(); // BARIS INI DIHAPUS atau DIKOMENTARI
            // Emote terakhir (love) akan tetap bertahan karena tidak ada yang menimpanya
        }
        taskText.text = displayMessage.TrimEnd();
    }

    public int GetTotalTasks()
    {
        return tasks.Length;
    }

    public string GetTaskName(int index)
    {
        if (index >= 0 && index < tasks.Length)
        {
            return tasks[index];
        }
        return "Invalid Task";
    }
}