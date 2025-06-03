using UnityEngine;

public class TowelInteraction : MonoBehaviour
{
    public TaskManager taskManager;
    public EmoteController emoteController;

    private bool isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isUsed) return;

        if (other.CompareTag("Baby"))
        {
            if (taskManager != null && taskManager.GetCurrentTaskIndex() == 0) // index 0 = Memandikan
            {
                taskManager.MarkCurrentTaskComplete();
                taskManager.NextTask();
                emoteController?.ShowHappy();
                isUsed = true;

                Debug.Log("[TOWEL] Bayi dikeringkan. Task Mandi selesai.");
            }
            else
            {
                emoteController?.ShowAngry();
                Debug.LogWarning("[TOWEL] Belum waktunya handukan!");
            }
        }
    }
}
