using UnityEngine;
using System.Collections;

public class TowelInteraction : MonoBehaviour
{
    public TaskManager taskManager;
    public CountdownTimer countdownTimer; // ✅ Wajib di-assign di Inspector
    public EmoteController emoteController;
    public WetEffectManager wetEffectManager;

    private bool isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isUsed || other == null) return;

        if (other.CompareTag("Baby"))
        {
            StartCoroutine(HandleTowelLogic());
        }
    }

    private IEnumerator HandleTowelLogic()
    {
        yield return new WaitForSeconds(0.1f); // ✅ Delay 1 frame agar efek sabun & air sempat aktif

        // ✅ Cek dari CountdownTimer yang lebih akurat
        int currentTask = countdownTimer != null ? countdownTimer.GetCurrentTaskIndex() : 
                         (taskManager != null ? taskManager.GetCurrentTaskIndex() : -1);

        Debug.Log($"[TOWEL DEBUG] Current Task Index: {currentTask}");

        if (currentTask == 0) // Task memandikan
        {
            if (SoapInteraction.IsSoaped && wetEffectManager != null && wetEffectManager.IsPlaying())
            {
                Debug.Log("[TOWEL ✅] Syarat terpenuhi. Memproses...");

                // ✅ PERBAIKAN: Hanya panggil SATU fungsi, biar CountdownTimer yang handle semua
                if (countdownTimer != null)
                {
                    countdownTimer.NotifyTaskSuccessFromInteraction();
                    Debug.Log("[TOWEL ✅] NotifyTaskSuccessFromInteraction() dipanggil.");
                }
                else if (taskManager != null)
                {
                    // Fallback jika CountdownTimer tidak ada
                    Debug.LogWarning("[TOWEL ⚠️] CountdownTimer null! Menggunakan TaskManager langsung.");
                    taskManager.MarkCurrentTaskComplete();
                    taskManager.NextTask();
                }

                // ✅ Matikan efek air
                wetEffectManager.StopEffect();
                Debug.Log("[TOWEL] Efek basah dimatikan oleh handuk.");

                // ✅ Emote happy
                emoteController?.ShowHappy();
                Debug.Log("[TOWEL] Emote senang ditampilkan.");

                isUsed = true;
                Debug.Log("[TOWEL ✅] Bayi dikeringkan. Task 'Memandikan' selesai.");
            }
            else
            {
                Debug.LogWarning($"[TOWEL ❌] Syarat belum terpenuhi. Soap: {SoapInteraction.IsSoaped}, Air: {wetEffectManager?.IsPlaying()}");
                emoteController?.ShowAngry();
            }
        }
        else
        {
            Debug.Log($"[TOWEL ❌] Bukan task 'Memandikan'. Current index: {currentTask}");
            emoteController?.ShowAngry();
        }
    }
}