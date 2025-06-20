// Contoh: ThermometerInteraction.cs
using UnityEngine;

public class ThermometerInteraction : MonoBehaviour
{
    [Header("Referensi")]
    public EmoteController emoteController; 
    public CountdownTimerStage2 countdownTimer; // REFERENSI KE CountdownTimerStage2

    [Header("Pengaturan Interaksi")]
    public string babyTag = "Baby";
    public float requiredContactDuration = 2.0f; // Durasi kontak yang dibutuhkan

    private float currentContactTime = 0f;
    private bool isTouchingBaby = false;
    private bool actionHasBeenTriggered = false;

    void Start()
    {
        if (emoteController == null)
        {
            Debug.LogError("❌ ThermometerInteraction: EmoteController belum di-assign!");
        }
        if (countdownTimer == null)
        {
            Debug.LogError("❌ ThermometerInteraction: CountdownTimerStage2 belum di-assign!");
        }
    }

    void Update()
    {
        if (isTouchingBaby && !actionHasBeenTriggered)
        {
            currentContactTime += Time.deltaTime;

            if (currentContactTime >= requiredContactDuration)
            {
                // VALIDASI TASK: Pastikan ini adalah task "Mengukur Suhu Bayi" (asumsi Task 0)
                if (countdownTimer != null && countdownTimer.GetCurrentActiveTaskIndex() == 0) // Asumsi "Mengukur Suhu Bayi" adalah task index 0
                {
                    Debug.Log("Termometer: Kontak cukup lama, suhu bayi diukur.");
                    
                    // Ini adalah kunci: Beri tahu CountdownTimerStage2 bahwa task berhasil!
                    countdownTimer.MarkCurrentTaskAsSuccess();
                    actionHasBeenTriggered = true; // Tandai aksi sudah dilakukan
                }
                else if (countdownTimer != null)
                {
                    Debug.LogWarning($"Termometer: Bukan waktu yang tepat untuk mengukur suhu! Task aktif: {countdownTimer.GetCurrentActiveTaskIndex()}.");
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
            Debug.Log("Termometer: Mulai bersentuhan dengan bayi.");
            isTouchingBaby = true;
            currentContactTime = 0f;
            actionHasBeenTriggered = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("Termometer: Berhenti bersentuhan dengan bayi.");
            isTouchingBaby = false;
            currentContactTime = 0f;
        }
    }
}