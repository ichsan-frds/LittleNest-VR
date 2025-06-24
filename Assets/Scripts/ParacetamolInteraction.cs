using UnityEngine;

public class ParacetamolInteraction : MonoBehaviour
{
    [Header("Referensi")]
    public EmoteController emoteController;
    public CountdownTimerStage2 countdownTimer;

    [Header("Pengaturan Interaksi")]
    public string babyTag = "Baby";
    public float requiredContactDuration = 2.0f;

    private float currentContactTime = 0f;
    private bool isTouchingBaby = false;
    private bool actionHasBeenTriggered = false;

    void Start()
    {
        if (emoteController == null)
        {
            Debug.LogError("❌ ParacetamolInteraction: EmoteController belum di-assign di Inspector!", this);
        }
        else
        {
            Debug.Log("✅ ParacetamolInteraction: EmoteController = " + emoteController.name, this);
        }

        if (countdownTimer == null)
        {
            Debug.LogError("❌ ParacetamolInteraction: CountdownTimerStage2 belum di-assign di Inspector!", this);
        }
        else
        {
            Debug.Log("✅ ParacetamolInteraction: CountdownTimer = " + countdownTimer.name, this);
        }
    }

    void Update()
    {
        if (isTouchingBaby && !actionHasBeenTriggered)
        {
            currentContactTime += Time.deltaTime;

            if (currentContactTime >= requiredContactDuration)
            {
                if (countdownTimer != null && countdownTimer.GetCurrentActiveTaskIndex() == 1)
                {
                    Debug.Log("Paracetamol: Kontak cukup lama, bayi diberi paracetamol.");
                    countdownTimer.MarkCurrentTaskAsSuccess();
                    actionHasBeenTriggered = true;
                }
                else if (countdownTimer != null)
                {
                    Debug.LogWarning($"Paracetamol: Bukan waktu yang tepat! Task aktif: {countdownTimer.GetCurrentActiveTaskIndex()}");
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
            actionHasBeenTriggered = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("Paracetamol: Berhenti bersentuhan dengan bayi.");
            isTouchingBaby = false;
            currentContactTime = 0f;
        }
    }
}
