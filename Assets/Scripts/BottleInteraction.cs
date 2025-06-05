using UnityEngine;

public class BottleInteraction : MonoBehaviour
{
    public TaskManager taskManager;
    public EmoteController emoteController;
    public CountdownTimer countdownTimer; // ✅ Tambahkan jika pakai CountdownTimer

    private bool isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isUsed || other == null) return;

        Debug.Log($"[BOTTLE DEBUG] Menyentuh objek: {other.name} | Tag: {other.tag}");

        if (other.CompareTag("Baby"))
        {
            int currentTask = taskManager?.GetCurrentTaskIndex() ?? -1;
            Debug.Log($"[BOTTLE DEBUG] Task sekarang: {currentTask}");

            if (currentTask == 4 && taskManager != null) // ✅ Task ke-5: Menidurkan
            {
                if (taskManager.CanSleep())
                {
                    taskManager.MarkCurrentTaskComplete();

                    if (countdownTimer != null)
                    {
                        countdownTimer.NotifyTaskSuccessFromInteraction();
                        Debug.Log("[BOTTLE ✅] Timer diberi tahu task berhasil.");
                    }
                    else
                    {
                        Debug.LogWarning("[BOTTLE ⚠️] CountdownTimer belum diassign. Fallback manual.");
                        taskManager.NextTask();
                    }

                    emoteController?.ShowHappy();
                    Debug.Log("[BOTTLE ✅] Bayi diberi botol & bisa tidur.");
                    isUsed = true;
                }
                else
                {
                    Debug.LogWarning("[BOTTLE ⚠️] Botol belum diberikan atau belum siap tidur.");
                    emoteController?.ShowAngry();
                }
            }
            else if (currentTask == 3 && taskManager != null)
            {
                // Jika botol digunakan lebih awal (task ke-4), tetap tandai bahwa botol sudah diberikan
                taskManager.SetBottleGiven();
                Debug.Log("[BOTTLE INFO] Botol diberikan saat task 'Memberi Makan', disimpan untuk nanti.");
                emoteController?.ShowHappy();
                isUsed = true;
            }
            else
            {
                Debug.LogWarning($"[BOTTLE ❌] Bukan waktunya. Task saat ini: {currentTask}");
                emoteController?.ShowAngry();
            }
        }
    }
}
