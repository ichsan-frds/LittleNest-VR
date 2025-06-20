using UnityEngine;

public class SpoonInteraction : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject isiSendok;
    public EmoteController emoteController;
    public TaskManager taskManager;
    public CountdownTimer countdownTimer; // ✅ WAJIB assign di Inspector

    private bool hasFedBaby = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        string objName = other.gameObject.name.ToLower();
        Debug.Log($"[SPOON DEBUG] Menyentuh objek: {objName}");

        // ✅ Ambil bubur dari mangkok
        if (objName.Contains("bowl") || objName.Contains("mangkok") || objName.Contains("bubur"))
        {
            if (isiSendok != null && !isiSendok.activeSelf)
            {
                isiSendok.SetActive(true);
                Debug.Log("[SPOON INFO] Bubur di sendok ditampilkan.");
            }
            return;
        }

        // ✅ Ubah logika: asal menyentuh bayi (TAG "Baby"), langsung berhasil
        if (other.CompareTag("Baby"))
        {
            if (isiSendok == null || !isiSendok.activeSelf)
            {
                Debug.LogWarning("[SPOON ❌] Tidak ada bubur di sendok.");
                emoteController?.ShowAngry();
                return;
            }

            int currentTaskIndex = taskManager?.GetCurrentTaskIndex() ?? -1;

            Debug.Log($"[SPOON DEBUG] Current Task Index: {currentTaskIndex}");

            if (currentTaskIndex == 3 && !hasFedBaby)
            {
                Debug.Log("[SPOON ✅] Task 'Memberi Makan' dimulai.");

                isiSendok.SetActive(false);
                Debug.Log("[SPOON INFO] Bubur disembunyikan dari sendok.");

                if (countdownTimer != null)
                {
                    countdownTimer.NotifyTaskSuccessFromInteraction(); // ✅ Biarkan CountdownTimer handle next task
                    Debug.Log("[SPOON ✅] NotifyTaskSuccessFromInteraction() dipanggil.");
                }
                else if (taskManager != null)
                {
                    taskManager.MarkCurrentTaskComplete();
                    Debug.LogWarning("[SPOON ⚠️] CountdownTimer null! Tidak bisa lanjut task otomatis.");
                }

                taskManager?.SetBottleGiven(); // dibutuhkan untuk task tidur
                emoteController?.ShowHappy();
                hasFedBaby = true;

                Debug.Log("[SPOON ✅] Task 'Memberi Makan' selesai dan emote senang ditampilkan.");
            }
            else
            {
                Debug.LogWarning($"[SPOON ❌] Bukan saatnya memberi makan. Task sekarang: {currentTaskIndex}");
                emoteController?.ShowAngry();
            }
        }
    }

    // ✅ Reset state agar bisa digunakan ulang saat game direset
    public void ResetFeeding()
    {
        hasFedBaby = false;
        Debug.Log("[SPOON RESET] Feeding state di-reset.");
    }
}
