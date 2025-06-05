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

        // ✅ Mengambil bubur dari mangkok
        if (objName.Contains("bowl") || objName.Contains("mangkok") || objName.Contains("bubur"))
        {
            if (isiSendok != null && !isiSendok.activeSelf)
            {
                isiSendok.SetActive(true);
                Debug.Log("[SPOON INFO] Bubur di sendok ditampilkan.");
            }
            return;
        }

        // ✅ Memberi makan bayi
        if (objName.Contains("mouth") || objName.Contains("tongue") || objName.Contains("baby"))
        {
            if (isiSendok == null || !isiSendok.activeSelf)
            {
                Debug.LogWarning("[SPOON ❌] Tidak ada bubur di sendok.");
                emoteController?.ShowAngry();
                return;
            }

            // ✅ Cek task index
            int timerTask = countdownTimer != null ? countdownTimer.GetCurrentTaskIndex() : -1;
            int managerTask = taskManager != null ? taskManager.GetCurrentTaskIndex() : -1;

            Debug.Log($"[SPOON DEBUG] Timer Task: {timerTask}, Manager Task: {managerTask}");

            if ((timerTask == 3 || managerTask == 3) && !hasFedBaby)
            {
                Debug.Log("[SPOON ✅] Task 'Memberi Makan' dimulai.");

                isiSendok.SetActive(false);
                Debug.Log("[SPOON INFO] Bubur disembunyikan dari sendok.");

                if (countdownTimer != null)
                {
                    countdownTimer.NotifyTaskSuccessFromInteraction(); // ✅ Biarkan timer handle next task
                    Debug.Log("[SPOON ✅] NotifyTaskSuccessFromInteraction() dipanggil.");
                }
                else if (taskManager != null)
                {
                    taskManager.MarkCurrentTaskComplete();
                    // ❌ Jangan panggil taskManager.NextTask() di sini agar tidak desync
                    Debug.LogWarning("[SPOON ⚠️] CountdownTimer null! Tidak bisa lanjut task otomatis.");
                }

                taskManager?.SetBottleGiven(); // dibutuhkan untuk task 5 (tidur)
                emoteController?.ShowHappy();
                hasFedBaby = true;

                Debug.Log("[SPOON ✅] Task 'Memberi Makan' selesai dan emote senang ditampilkan.");
            }
            else
            {
                Debug.LogWarning($"[SPOON ❌] Bukan task 'Memberi Makan'. Task sekarang: {managerTask}");
                emoteController?.ShowAngry();
            }
        }
    }
}
