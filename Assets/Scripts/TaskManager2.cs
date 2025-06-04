using UnityEngine;
using TMPro;

public class TaskManager2 : MonoBehaviour
{
    [Header("Referensi UI")]
    public TextMeshProUGUI taskText;
    public IndicatorStatus indicatorStatus; // Jika Anda menggunakan ini untuk indikator visual per task
    public EmoteController babyEmoteController; // TAMBAHKAN INI: Referensi ke EmoteController bayi

    private string[] tasks = {
        "Mengukur Suhu Bayi",
        "Memberi Obat",
        "Mengompres Bayi"
        // Urutan ini sudah sesuai dengan keinginan Anda: ukur suhu, lalu beri obat, lalu kompres.
    };

    private int currentTaskIndex = 0;
    // private bool isBottleGiven = false; // Variabel ini sepertinya tidak digunakan di Stage 2 ini, bisa dihapus jika tidak relevan

    private enum TaskResult { None, Success, Failed }
    private TaskResult[] taskResults;

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

        taskResults = new TaskResult[tasks.Length];
        InitializeStage2();
    }

    // Fungsi baru untuk inisialisasi Stage 2
    public void InitializeStage2()
    {
        currentTaskIndex = 0;
        // isBottleGiven = false; // Hapus jika tidak relevan
        taskResults = new TaskResult[tasks.Length]; // Reset hasil task
        indicatorStatus?.ResetAll(); // Reset indikator status jika ada

        // 1. Bayi langsung mengeluarkan emote demam di awal Stage 2
        if (babyEmoteController != null)
        {
            babyEmoteController.ShowFeverEmote(); // Panggil fungsi untuk menunjukkan emote demam
            Debug.Log("TaskManager2: Menampilkan emote demam pada bayi.");
        }

        // 2. Tampilkan tugas pertama (Mengukur Suhu Bayi)
        ShowOnlyCurrentTask(currentTaskIndex);
    }

    // Fungsi UpdateTaskText sepertinya bertujuan menampilkan semua status task,
    // sedangkan ShowOnlyCurrentTask fokus pada task saat ini.
    // Saya akan biarkan keduanya, tapi pastikan pemanggilannya konsisten.
    // Untuk alur satu per satu, ShowOnlyCurrentTask lebih sering dipakai.
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
            else if (i == currentTaskIndex) // Hanya tampilkan yang bold jika itu task saat ini dan belum selesai/gagal
            {
                result += $"<b>{i + 1}. {tasks[i]}</b>\n";
            }
            // Jika Anda ingin hanya task saat ini yang tampil, dan yang sudah selesai,
            // maka logic di ShowOnlyCurrentTask lebih cocok.
            // Jika ingin semua list tampil dengan statusnya, logic ini oke.
        }
        taskText.text = result.TrimEnd();
    }

    public void ShowOnlyCurrentTask(int index)
    {
        if (taskText == null) return;

        string result = "Task List\n";
        if (index < tasks.Length)
        {
            // Selalu tampilkan task saat ini, apa pun statusnya
            if (taskResults[index] == TaskResult.Success)
                result += $"<color=green>{index + 1}. ✔ {tasks[index]}</color>\n";
            else if (taskResults[index] == TaskResult.Failed)
                result += $"<color=red>{index + 1}. ✘ {tasks[index]}</color>\n";
            else // TaskResult.None (belum dikerjakan)
                result += $"<b>{index + 1}. {tasks[index]}</b>\n"; // Tampil bold
        }
        else // Semua task sudah melewati indeks terakhir
        {
            result += "<color=yellow><b>Semua task Stage 2 selesai!</b></color>";
            // Anda mungkin ingin menyembunyikan emote demam atau menggantinya dengan happy di sini
            // if (babyEmoteController != null) {
            //    babyEmoteController.ShowHappy(); // Contoh
            // }
        }
        taskText.text = result.TrimEnd();
    }

    public void MarkCurrentTaskComplete()
    {
        if (currentTaskIndex >= tasks.Length) return;

        taskResults[currentTaskIndex] = TaskResult.Success;
        indicatorStatus?.SetTaskStatus(currentTaskIndex, true);

        // Setelah task "Mengukur Suhu Bayi" selesai, Anda mungkin ingin menyembunyikan emote demam
        // atau menggantinya dengan emote suhu 39 derajat jika itu bagian dari feedbacknya.
        // Ini contoh jika emote demam disembunyikan setelah suhu diukur:
        // if (tasks[currentTaskIndex] == "Mengukur Suhu Bayi" && babyEmoteController != null) {
        //     babyEmoteController.HideAllDisplays(); // Atau ganti dengan emote lain
        // }

        // Setelah task "Memberi Obat" selesai, bayi mungkin jadi happy
        // if (tasks[currentTaskIndex] == "Memberi Obat" && babyEmoteController != null) {
        //     babyEmoteController.ShowHappy();
        // }

        // UpdateTaskText(); // Jika ingin menampilkan semua list task dengan statusnya
        ShowOnlyCurrentTask(currentTaskIndex); // Jika hanya ingin fokus pada task saat ini (sudah jadi hijau)
                                               // Lalu panggil NextTask() untuk pindah
    }

    public void MarkCurrentTaskFailed()
    {
        if (currentTaskIndex >= tasks.Length) return;

        taskResults[currentTaskIndex] = TaskResult.Failed;
        indicatorStatus?.SetTaskStatus(currentTaskIndex, false);
        // UpdateTaskText();
        ShowOnlyCurrentTask(currentTaskIndex); // Tampilkan bahwa task saat ini gagal (jadi merah)
                                               // Pertimbangkan apakah game lanjut ke task berikutnya atau ada konsekuensi lain
    }

    public void NextTask()
    {
        if (currentTaskIndex >= tasks.Length) { // Jika sudah di luar batas, jangan lakukan apa-apa
             ShowOnlyCurrentTask(currentTaskIndex); // Pastikan pesan "Semua task selesai" ditampilkan
             return;
        }

        // Hanya increment jika task saat ini sudah selesai (Success)
        // Atau jika Anda membolehkan skip task, maka bisa langsung increment.
        // Untuk sekarang, asumsikan NextTask dipanggil setelah MarkCurrentTaskComplete.
        if (taskResults[currentTaskIndex] == TaskResult.Success || taskResults[currentTaskIndex] == TaskResult.Failed) // Atau hanya jika Success
        {
             currentTaskIndex++;
        }
        // Jika task saat ini belum selesai, jangan pindah ke task berikutnya
        // kecuali ada logika khusus untuk skip.
        // Untuk keamanan, pastikan NextTask() dipanggil setelah task selesai.

        ShowOnlyCurrentTask(currentTaskIndex); // Tampilkan task berikutnya atau pesan selesai
    }

    public int GetCurrentTaskIndex()
    {
        return currentTaskIndex;
    }

    public string GetCurrentTaskName()
    {
        if (currentTaskIndex < tasks.Length)
        {
            return tasks[currentTaskIndex];
        }
        return "Semua task selesai";
    }

    public void ResetTasks() // Berguna jika stage dimulai ulang
    {
        InitializeStage2(); // Panggil ulang inisialisasi
    }
}