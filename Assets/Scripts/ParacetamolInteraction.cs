using UnityEngine;

public class ParacetamolInteraction : MonoBehaviour
{
    [Header("Referensi")]
    public EmoteController emoteController; // Assign EmoteController dari scene Anda
    public CountdownTimerStage2 countdownTimer; // <<< BARU: Referensi ke CountdownTimerStage2

    [Header("Pengaturan Interaksi")]
    public string babyTag = "Baby"; // Tag untuk GameObject bayi
    public float requiredContactDuration = 2.0f; // Durasi kontak yang dibutuhkan (dalam detik)

    private float currentContactTime = 0f;
    private bool isTouchingBaby = false;
    private bool actionHasBeenTriggered = false; // Untuk memastikan aksi hanya terjadi sekali per sentuhan

    void Start()
    {
        if (emoteController == null)
        {
            Debug.LogError("❌ ParacetamolInteraction: EmoteController belum di-assign di Inspector!");
        }
        if (countdownTimer == null) // BARU: Peringatan jika referensi kosong
        {
            Debug.LogError("❌ ParacetamolInteraction: CountdownTimerStage2 belum di-assign di Inspector!");
        }
    }

    void Update()
    {
        // Hanya proses interaksi jika sedang menyentuh bayi dan aksi belum terpicu
        if (isTouchingBaby && !actionHasBeenTriggered)
        {
            currentContactTime += Time.deltaTime;

            if (currentContactTime >= requiredContactDuration)
            {
                // VALIDASI TASK: Pastikan ini adalah task "Memberi Obat" (asumsi Task 1)
                // Dapatkan indeks task aktif dari CountdownTimerStage2
                if (countdownTimer != null && countdownTimer.GetCurrentActiveTaskIndex() == 1) // Asumsi "Memberi Obat" adalah task index 1
                {
                    Debug.Log("Paracetamol: Kontak cukup lama, bayi diberi paracetamol.");
                    
                    // --- GANTI LOGIKA EMOTE INI DENGAN PEMANGGILAN MarkCurrentTaskAsSuccess() ---
                    // Logika emote akan ditangani di CountdownTimerStage2 atau TaskManager2 setelah task sukses
                    // if (emoteController != null) {
                    //    emoteController.ShowHappy(); 
                    // }

                    // Ini adalah kunci: Beri tahu CountdownTimerStage2 bahwa task berhasil!
                    countdownTimer.MarkCurrentTaskAsSuccess();
                    actionHasBeenTriggered = true; // Tandai aksi sudah dilakukan
                }
                else if (countdownTimer != null)
                {
                    Debug.LogWarning($"Paracetamol: Bukan waktu yang tepat untuk memberi obat! Task aktif: {countdownTimer.GetCurrentActiveTaskIndex()}.");
                    // Opsional: Reset contact time atau berikan feedback negatif jika interaksi salah
                    isTouchingBaby = false;
                    currentContactTime = 0f;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("Paracetamol: Mulai bersentuhan dengan bayi.");
            isTouchingBaby = true;
            currentContactTime = 0f;
            actionHasBeenTriggered = false; // Reset untuk interaksi baru
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("Paracetamol: Berhenti bersentuhan dengan bayi.");
            isTouchingBaby = false;
            currentContactTime = 0f;
            // actionHasBeenTriggered = false; // Ini bisa tetap true atau direset tergantung kebutuhan
                                             // Biasanya direset di OnCollisionEnter
        }
    }
}