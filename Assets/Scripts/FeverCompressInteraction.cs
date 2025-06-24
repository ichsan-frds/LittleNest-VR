using UnityEngine;

public class FeverCompressInteraction : MonoBehaviour
{
    [Header("Referensi")]
    public EmoteController emoteController;
    public CountdownTimerStage2 countdownTimer;

    [Header("Pengaturan Interaksi")]
    public string babyTag = "Baby";
    public float requiredContactDuration = 5.0f;

    private float currentContactTime = 0f;
    private bool isTouchingBaby = false;
    private bool actionHasBeenTriggered = false;

    void Start()
    {
        if (emoteController == null)
        {
            Debug.LogError("❌ FeverCompressInteraction: EmoteController belum di-assign!", this);
        }
        else
        {
            Debug.Log("✅ FeverCompressInteraction: EmoteController = " + emoteController.name, this);
        }

        if (countdownTimer == null)
        {
            Debug.LogError("❌ FeverCompressInteraction: CountdownTimerStage2 belum di-assign!", this);
        }
        else
        {
            Debug.Log("✅ FeverCompressInteraction: CountdownTimer = " + countdownTimer.name, this);
        }
    }

    void Update()
    {
        if (isTouchingBaby && !actionHasBeenTriggered)
        {
            currentContactTime += Time.deltaTime;

            if (currentContactTime >= requiredContactDuration)
            {
                if (countdownTimer != null && countdownTimer.GetCurrentActiveTaskIndex() == 2)
                {
                    Debug.Log("Kompres: Kontak cukup lama, mengompress.");
                    countdownTimer.MarkCurrentTaskAsSuccess();
                    actionHasBeenTriggered = true;
                }
                else if (countdownTimer != null)
                {
                    Debug.LogWarning($"Kompres: Bukan waktu yang tepat! Task aktif: {countdownTimer.GetCurrentActiveTaskIndex()}");
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
            actionHasBeenTriggered = false;
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
