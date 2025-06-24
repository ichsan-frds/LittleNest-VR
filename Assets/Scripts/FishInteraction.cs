using UnityEngine;

public class FishInteraction : MonoBehaviour
{
    [Header("Referensi Objek")]
    public EmoteController emoteController;
    public TaskManager taskManager;
    public CountdownTimerStage1 countdownTimer; // ✅ WAJIB assign via Inspector

    private bool hasCalmedBaby = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasCalmedBaby || other == null) return;

        string objName = other.gameObject.name.ToLower();
        Debug.Log($"[FISH DEBUG] OnTriggerEnter oleh: {other.name}, tag: {other.tag}");

        // Deteksi apakah menyentuh bagian bayi
        if (objName.Contains("mouth") || objName.Contains("tongue") || objName.Contains("baby"))
        {
            int currentTask = taskManager?.GetCurrentTaskIndex() ?? -1;
            Debug.Log($"[FISH DEBUG] Menyentuh bayi. Task saat ini: {currentTask}");

            if (currentTask == 2)
            {
                // ✅ Emote senang
                emoteController?.ShowHappy();

                // ✅ Tandai sukses ke TaskManager
                taskManager?.MarkCurrentTaskComplete();

                // ✅ Sinkron ke Timer
                if (countdownTimer != null)
                {
                    countdownTimer.NotifyTaskSuccessFromInteraction();
                    Debug.Log("[FISH ✅] NotifyTaskSuccessFromInteraction dipanggil.");
                }
                else
                {
                    Debug.LogWarning("[FISH ⚠️] CountdownTimer belum di-assign. Fallback manual.");
                    taskManager?.NextTask();
                }

                hasCalmedBaby = true;
                Debug.Log("[FISH ✅] Task 'Menenangkan Bayi' berhasil diselesaikan.");
            }
            else
            {
                // ❌ Salah waktu
                emoteController?.ShowAngry();
                Debug.LogWarning($"[FISH ⚠️] Bukan saatnya bermain ikan. Task sekarang: {currentTask}");
            }
        }
        else
        {
            Debug.Log($"[FISH ❌] Objek bukan bagian bayi: {objName}");
        }
    }
}
