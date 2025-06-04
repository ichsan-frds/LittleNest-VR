using UnityEngine;

public class DiaperInteraction : MonoBehaviour
{
    public TaskManager taskManager;
    public EmoteController emoteController;
    
    private bool isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[DIAPER DEBUG] Trigger terdeteksi. Objek masuk: {other.name}, Tag: {other.tag}");

        if (isUsed || other == null)
        {
            Debug.Log("[DIAPER DEBUG] Sudah pernah dipakai atau collider null.");
            return;
        }

        if (other.CompareTag("Baby"))
        {
            if (taskManager != null && taskManager.GetCurrentTaskIndex() == 1)
            {
                taskManager.MarkCurrentTaskComplete();
                taskManager.NextTask();

                emoteController?.ShowHappy();
                isUsed = true;

                Debug.Log("[DIAPER ✅] Popok diganti. Task selesai.");

                // Cari objek popok berdasarkan tag
                GameObject diaperObj = GameObject.FindGameObjectWithTag("Diaper");
                if (diaperObj != null)
                {
                    Debug.Log("[DIAPER INFO] Popok akan dihancurkan.");
                    Destroy(diaperObj);
                }
                else
                {
                    Debug.LogWarning("[DIAPER ❌] Objek dengan tag 'Diaper' tidak ditemukan.");
                }
            }
            else
            {
                emoteController?.ShowAngry();
                Debug.LogWarning("[DIAPER ⚠️] Belum waktunya mengganti popok.");
            }
        }
        else
        {
            Debug.Log("[DIAPER DEBUG] Objek bukan bayi. Abaikan.");
        }
    }
}
