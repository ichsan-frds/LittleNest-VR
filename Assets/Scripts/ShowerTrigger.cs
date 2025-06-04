using UnityEngine;

public class ShowerTrigger : MonoBehaviour
{
    public ParticleSystem foamEffect;               // Efek busa dari sabun (ParticleSystem)
    public ParticleSystem showerEffect;             // Visual efek air shower (keran)
    public WetEffectManager wetEffectManager;       // Manager efek basah di bayi

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SHOWER] Trigger aktif. Objek masuk: {other.gameObject.name} (Tag: {other.tag})");

        if (other.CompareTag("Baby"))
        {
            Debug.Log("[SHOWER] Bayi masuk ke area shower.");

            // Cek apakah shower sedang menyala
            if (ShowerToggle.isShowerRunning)
            {
                // Matikan busa jika masih aktif
                if (foamEffect != null && foamEffect.isPlaying)
                {
                    foamEffect.Stop();
                    Debug.Log("[SHOWER] Efek busa DIMATIKAN karena terkena air shower.");
                }

                // Nyalakan efek air shower (keran)
                if (showerEffect != null && !showerEffect.isPlaying)
                {
                    showerEffect.Play();
                    Debug.Log("[SHOWER] Efek air shower dinyalakan.");
                }

                // Nyalakan efek basah di tubuh bayi
                if (wetEffectManager != null)
                {
                    wetEffectManager.PlayEffect();
                }
                else
                {
                    Debug.LogWarning("[SHOWER] WetEffectManager belum di-assign di Inspector!");
                }
            }
            else
            {
                Debug.Log("[SHOWER] Shower belum menyala. Tidak ada aksi pada busa atau efek basah.");
            }
        }
    }
}
