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

        // ✅ Cari otomatis CountdownTimerStage2 jika belum di-assign
        if (countdownTimer == null)
        {
            countdownTimer = FindObjectOfType<CountdownTimerStage2>();

            if (countdownTimer != null)
            {
                Debug.Log("🔁 CountdownTimerStage2 ditemukan otomatis.");
            }
            else
            {
                Debug.LogError("❌ ThermometerInteraction: CountdownTimerStage2 belum di-assign dan tidak ditemukan di scene!");
            }
        }
    }

    void Update()
    {
        if (isTouchingBaby && !actionHasBeenTriggered && countdownTimer != null)
        {
            currentContactTime += Time.deltaTime;

            if (currentContactTime >= requiredContactDuration)
            {
                // VALIDASI TASK: Asumsi Task 0 adalah "Mengukur Suhu Bayi"
                int currentTask = countdownTimer.GetCurrentActiveTaskIndex();
                if (currentTask == 0)
                {
                    Debug.Log("✅ Termometer: Kontak cukup lama, suhu bayi diukur.");
                    countdownTimer.MarkCurrentTaskAsSuccess();
                    actionHasBeenTriggered = true;
                }
                else
                {
                    Debug.LogWarning($"⚠️ Termometer: Bukan waktunya mengukur suhu! Task aktif: {currentTask}");
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
            Debug.Log("👶 Termometer: Mulai bersentuhan dengan bayi.");
            isTouchingBaby = true;
            currentContactTime = 0f;
            actionHasBeenTriggered = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("🛑 Termometer: Berhenti bersentuhan dengan bayi.");
            isTouchingBaby = false;
            currentContactTime = 0f;
        }
    }
}
