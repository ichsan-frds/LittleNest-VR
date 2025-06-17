using UnityEngine;
using TMPro;

public class TaskManager2 : MonoBehaviour
{
    [Header("Referensi UI")]
    public TextMeshProUGUI taskText;
    // Hapus public IndicatorStatus indicatorStatus; karena ini dikelola oleh CountdownTimerStage2
    public EmoteController babyEmoteController; // Referensi ke EmoteController bayi

    // Enumerasi untuk hasil task (dideklarasikan di sini agar bisa diakses dari luar)
    public enum TaskResult { None, Success, Failed }

    // List task Anda
    private string[] tasks = {
        "Mengukur Suhu Bayi",
        "Memberi Obat",
        "Mengompres Bayi" // Asumsi Anda punya 3 task
    };

    // currentTaskIndex dan taskResults[] akan dikelola di CountdownTimerStage2
    // private int currentTaskIndex = 0;
    // private TaskResult[] taskResults;

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
        // taskResults = new TaskResult[tasks.Length]; // Tidak diperlukan lagi di sini
        // InitializeStage2(); // Sekarang akan dipanggil dari CountdownTimerStage2
    }

    // Dipanggil oleh CountdownTimerStage2 untuk inisialisasi tampilan UI task
    public void InitializeStage2(int startTaskIndex)
    {
        // Tidak perlu mereset currentTaskIndex atau taskResults di sini
        // Karena ini dikelola oleh CountdownTimerStage2
        
        // Emote demam di awal stage
        if (babyEmoteController != null)
        {
            babyEmoteController.ShowFeverEmote();
            Debug.Log("TaskManager2: Menampilkan emote demam pada bayi.");
        }

        // Tampilkan task pertama
        ShowOnlyCurrentTask(startTaskIndex, TaskResult.None);
    }

    // Metode ini yang akan dipanggil oleh CountdownTimerStage2 untuk memperbarui UI
    public void ShowOnlyCurrentTask(int index, TaskResult result = TaskResult.None)
    {
        if (taskText == null) return;

        string displayMessage = "Task List\n";
        if (index < tasks.Length) // Jika masih dalam batas task yang ada
        {
            string taskName = tasks[index];
            string statusPrefix = "";

            if (result == TaskResult.Success)
            {
                statusPrefix = "<color=green>✔</color> ";
                // Emote bayi jika task ini selesai berhasil
                if (babyEmoteController != null) {
                    if (index == 0) // Jika task Mengukur Suhu Bayi
                        babyEmoteController.Show39DegreeTemperature(); // Ganti dengan emote suhu
                    else if (index == 1) // Jika task Memberi Obat
                        babyEmoteController.ShowHappy(); // Bayi senang
                    // Tambahkan kondisi untuk task lain jika ada
                }
            }
            else if (result == TaskResult.Failed)
            {
                statusPrefix = "<color=red>✘</color> ";
                // Emote bayi jika task ini gagal
                if (babyEmoteController != null) {
                    babyEmoteController.ShowAngry(); // Bayi marah/sedih
                }
            }
            else // TaskResult.None (task sedang berjalan atau belum dimulai)
            {
                statusPrefix = "<b>"; // Tampil bold untuk task aktif
            }
            
            displayMessage += $"{statusPrefix}{index + 1}. {taskName}";
            if (result == TaskResult.None) displayMessage += "</b>"; // Tutup bold jika sedang berjalan
        }
        else // Semua task sudah selesai
        {
            displayMessage = "<color=yellow><b>Semua task Stage 2 selesai!</b></color>";
            if (babyEmoteController != null) {
                babyEmoteController.ShowHappy(); // Bayi happy di akhir stage
            }
        }
        taskText.text = displayMessage.TrimEnd();
    }

    // Metode GetTotalTasks dan GetTaskName tetap dipertahankan
    // agar CountdownTimerStage2 bisa mengambil informasi tentang task
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

    // Hapus semua metode MarkCurrentTaskComplete, MarkCurrentTaskFailed, NextTask, ResetTasks
    // karena mereka sekarang dikelola sepenuhnya oleh CountdownTimerStage2
    // public void MarkCurrentTaskComplete() { ... }
    // public void MarkCurrentTaskFailed() { ... }
    // public void NextTask() { ... }
    // public void ResetTasks() { ... }
}