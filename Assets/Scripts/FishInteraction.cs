using UnityEngine;

public class FishInteraction : MonoBehaviour
{
    [Header("Referensi Objek")]
    public EmoteController emoteController;      // Emote controller (happy/angry)
    public TaskManager taskManager;              // Sistem manajemen task

    private bool hasCalmedBaby = false;          // Supaya hanya satu kali sukses

    private void OnTriggerEnter(Collider other)
    {
        string objName = other.gameObject.name.ToLower();
        Debug.Log($"[DEBUG] Ikan bersentuhan dengan: {objName}");

        // 👶 Deteksi interaksi dengan bayi
        if (objName.Contains("mouth") || objName.Contains("tongue") || objName.Contains("baby"))
        {
            int currentTask = taskManager?.GetCurrentTaskIndex() ?? -1;

            if (currentTask == 2 && !hasCalmedBaby) // ✅ Task ke-3: Menenangkan Bayi
            {
                emoteController?.ShowHappy();
                taskManager?.MarkCurrentTaskComplete();
                taskManager?.NextTask();
                Debug.Log("[DEBUG] ✅ Task 'Menenangkan Bayi' diselesaikan dengan mainan ikan.");
                hasCalmedBaby = true;
            }
            else
            {
                emoteController?.ShowAngry();
                Debug.LogWarning($"❌ Bukan waktunya menenangkan bayi. Task sekarang: {currentTask}");
            }
        }
    }
}
