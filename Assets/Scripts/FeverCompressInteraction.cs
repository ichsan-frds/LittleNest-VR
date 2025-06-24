// Contoh: ThermometerInteraction.cs
using UnityEngine;

public class FeverCompressInteraction : MonoBehaviour
{
    [Header("Referensi")]
    public EmoteController emoteController;
    public CountdownTimerStage2 countdownTimer; // REFERENSI KE CountdownTimerStage2

    [Header("Pengaturan Interaksi")]
    public string babyTag = "Baby";
    public float requiredContactDuration = 5.0f; // Durasi kontak yang dibutuhkan

    private float currentContactTime = 0f;
    private bool isTouchingBaby = false;
    private bool actionHasBeenTriggered = false;

    void Start()
    {
        if (emoteController == null)
        {
            Debug.LogError("❌ FeverCompressInteraction: EmoteController belum di-assign!");
        }
        if (countdownTimer == null)
        {
            Debug.LogError("❌ FeverCompressInteraction: CountdownTimerStage2 belum di-assign!");
        }
    }

    void Update()
    {
        if (isTouchingBaby && !actionHasBeenTriggered)
        {
            currentContactTime += Time.deltaTime;

            if (currentContactTime >= requiredContactDuration)
            {
                // VALIDASI TASK: Pastikan ini adalah task "Mengompres Bayi" (asumsi Task Index 2)
                if (countdownTimer != null && countdownTimer.GetCurrentActiveTaskIndex() == 2) // Diubah ke indeks 2
                {
                    Debug.Log("Kompres: Kontak cukup lama, mengompress.");

                    // Ini adalah kunci: Beri tahu CountdownTimerStage2 bahwa task berhasil!
                    countdownTimer.MarkCurrentTaskAsSuccess();
                    actionHasBeenTriggered = true; // Tandai aksi sudah dilakukan
                }
                else if (countdownTimer != null)
                {
                    Debug.LogWarning($"Kompres: Bukan waktu yang tepat untuk mengompres bayi! Task aktif: {countdownTimer.GetCurrentActiveTaskIndex()}.");
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
            Debug.Log("Kompres: Mulai bersentuhan dengan bayi.");
            isTouchingBaby = true;
            currentContactTime = 0f;
            actionHasBeenTriggered = false; // Reset status saat mulai kontak baru
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("Kompres: Berhenti bersentuhan dengan bayi.");
            isTouchingBaby = false;
            currentContactTime = 0f;
        }
    }
}