using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DiaperInteraction : MonoBehaviour
{
    public TaskManager taskManager;
    public CountdownTimerStage1 countdownTimer;
    public EmoteController emoteController;

    private bool isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[DIAPER DEBUG] Trigger: {gameObject.name} menyentuh: {other.name}, Tag: {other.tag}");

        // ✅ Debug lebih detail untuk troubleshooting
        if (taskManager != null)
        {
            Debug.Log($"[DIAPER DEBUG] TaskManager.GetCurrentTaskIndex() = {taskManager.GetCurrentTaskIndex()}");
        }
        
        if (countdownTimer != null)
        {
            Debug.Log($"[DIAPER DEBUG] CountdownTimer.GetCurrentTaskIndex() = {countdownTimer.GetCurrentTaskIndex()}");
        }

        if (isUsed || other == null) return;

        if (other.CompareTag("Baby"))
        {
            Debug.Log("[DIAPER DEBUG] BAYI terdeteksi!");

            // ✅ Cek dari kedua sumber untuk memastikan
            int taskIndex = taskManager != null ? taskManager.GetCurrentTaskIndex() : -1;
            int timerIndex = countdownTimer != null ? countdownTimer.GetCurrentTaskIndex() : -1;
            
            Debug.Log($"[DIAPER DEBUG] TaskManager Index: {taskIndex}, Timer Index: {timerIndex}");

            // ✅ Gunakan CountdownTimer sebagai sumber utama karena lebih akurat
            bool isDiaperTime = (timerIndex == 1) || (taskIndex == 1);
            
            if (isDiaperTime)
            {
                Debug.Log("[DIAPER ✅] Waktu ganti popok! Memproses...");
                
                // ✅ Panggil CountdownTimer dulu, biarkan dia yang handle TaskManager
                if (countdownTimer != null)
                {
                    countdownTimer.NotifyTaskSuccessFromInteraction();
                    Debug.Log("[DIAPER] NotifyTaskSuccessFromInteraction dipanggil.");
                }
                else if (taskManager != null)
                {
                    // Fallback jika CountdownTimer tidak ada
                    Debug.LogWarning("[DIAPER] CountdownTimer null! Menggunakan TaskManager langsung.");
                    taskManager.MarkCurrentTaskComplete();
                    taskManager.NextTask();
                }

                emoteController?.ShowHappy();
                Debug.Log("[DIAPER] Emote senang ditampilkan.");

                isUsed = true;

                // Lepaskan dari tangan dulu jika sedang dipegang
                var interactable = GetComponent<XRGrabInteractable>();
                if (interactable != null && interactable.isSelected)
                {
                    interactable.interactionManager.SelectExit(interactable.firstInteractorSelecting, interactable);
                    Debug.Log("[DIAPER] Popok dilepas dari interactor sebelum dihancurkan.");
                }

                // Tambahkan delay singkat sebelum destroy
                Invoke(nameof(DestroySelf), 0.1f);
            }
            else
            {
                Debug.LogWarning($"[DIAPER ⚠️] Bukan waktunya ganti popok! TaskManager Index: {taskIndex}, Timer Index: {timerIndex}");
                emoteController?.ShowAngry();
            }
        }
        else
        {
            Debug.Log($"[DIAPER DEBUG] Yang kena bukan bayi: {other.tag}");
        }
    }

    private void DestroySelf()
    {
        Debug.Log($"[DIAPER ✅] Popok ({gameObject.name}) dihancurkan dari scene.");
        Destroy(gameObject);
    }
}