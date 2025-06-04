using UnityEngine;

public class TowelInteraction : MonoBehaviour
{
    public TaskManager taskManager;
    public EmoteController emoteController;
    public WetEffectManager wetEffectManager; // Mengontrol efek basah di bayi

    private bool isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isUsed || other == null) return;

        if (other.CompareTag("Baby"))
        {
            // ✅ Cek apakah sedang di task "Memandikan" (index 0)
            if (taskManager != null && taskManager.GetCurrentTaskIndex() == 0)
            {
                // 🔽 Tandai task selesai dan lanjutkan
                taskManager.MarkCurrentTaskComplete();
                taskManager.NextTask();

                // 🔽 Matikan efek basah
                if (wetEffectManager != null)
                {
                    wetEffectManager.StopEffect();
                    Debug.Log("[TOWEL] Efek basah dimatikan oleh handuk.");
                }
                else
                {
                    Debug.LogWarning("[TOWEL] WetEffectManager belum di-assign!");
                }

                // 🔽 Tampilkan ekspresi senang
                if (emoteController != null)
                {
                    emoteController.ShowHappy();
                    Debug.Log("[TOWEL] Emote senang ditampilkan.");
                }
                else
                {
                    Debug.LogWarning("[TOWEL] EmoteController belum di-assign!");
                }

                isUsed = true;
                Debug.Log("[TOWEL] Bayi dikeringkan. Task 'Memandikan' selesai.");
            }
            else
            {
                // 🔽 Task belum waktunya: tampilkan emote marah
                if (emoteController != null)
                {
                    emoteController.ShowAngry();
                    Debug.Log("[TOWEL] Belum waktunya mengeringkan bayi! Emote marah ditampilkan.");
                }
                else
                {
                    Debug.LogWarning("[TOWEL] EmoteController belum di-assign!");
                }
            }
        }
    }
}
