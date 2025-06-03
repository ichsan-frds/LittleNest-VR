using UnityEngine;

public class SpoonInteraction : MonoBehaviour
{
    [Header("Referensi Objek")]
    public GameObject isiSendok;                      // Bubur di atas sendok
    public EmoteController emoteController;           // Emote controller (happy/angry)
    public TaskManager taskManager;                   // Sistem manajemen task

    private bool hasFedBaby = false;                  // Supaya cuma sekali sukses

    private void OnTriggerEnter(Collider other)
    {
        string objName = other.gameObject.name.ToLower();
        Debug.Log($"[DEBUG] Sendok kena: {objName}");

        // 🥣 Saat menyentuh mangkuk/bubur
        if (objName.Contains("bowl") || objName.Contains("mangkok") || objName.Contains("bubur"))
        {
            if (isiSendok != null)
            {
                isiSendok.SetActive(true);
                Debug.Log("[DEBUG] Bubur di sendok ditampilkan.");
            }
            return;
        }

        // 👶 Saat menyentuh bayi/mulut
        if (objName.Contains("mouth") || objName.Contains("tongue") || objName.Contains("baby"))
        {
            // ❌ Tidak ada bubur? Batalin
            if (isiSendok == null || !isiSendok.activeSelf)
            {
                Debug.Log("[WARN] Tidak ada bubur di sendok.");
                return;
            }

            isiSendok.SetActive(false);
            Debug.Log("[DEBUG] Bubur di sendok disembunyikan.");

            int currentTask = taskManager?.GetCurrentTaskIndex() ?? -1;

            if (currentTask == 3 && !hasFedBaby) // ✅ Task ke-4: Memberi Makan
            {
                emoteController?.ShowHappy();
                taskManager?.MarkCurrentTaskComplete();
                taskManager?.NextTask();
                Debug.Log("[DEBUG] ✅ Task 'Memberi Makan' diselesaikan dan lanjut.");
                hasFedBaby = true;
            }
            else
            {
                emoteController?.ShowAngry();
                Debug.LogWarning($"❌ Bukan waktunya suapin. Task sekarang: {currentTask}");
            }
        }
    }
}
