using UnityEngine;

public class ShowerToggle : MonoBehaviour
{
    [Header("Drag objek Particle System air di sini")]
    public ParticleSystem showerEffect;

    // Flag global yang bisa dicek dari script lain (contoh: ShowerTrigger)
    public static bool isShowerRunning = false;

    private bool isShowerOn = false;

    // Fungsi ini dipanggil dari XR Simple Interactable (misal saat tombol shower dipencet)
    public void ToggleShower()
    {
        Debug.Log("[SHOWER] ToggleShower() dipanggil."); // Debug untuk memastikan fungsi terpanggil

        if (showerEffect == null)
        {
            Debug.LogWarning("[SHOWER] showerEffect belum di-assign!");
            return;
        }

        isShowerOn = !isShowerOn;
        isShowerRunning = isShowerOn; // update flag global

        if (isShowerOn)
        {
            showerEffect.Play();
            Debug.Log("[SHOWER] Shower MENYALA.");
        }
        else
        {
            showerEffect.Stop();
            Debug.Log("[SHOWER] Shower DIMATIKAN.");
        }
    }
}
