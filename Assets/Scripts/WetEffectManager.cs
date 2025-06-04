using UnityEngine;

public class WetEffectManager : MonoBehaviour
{
    private ParticleSystem wetParticle;

    void Awake()
    {
        // Ambil komponen ParticleSystem dari objek ini
        wetParticle = GetComponent<ParticleSystem>();

        if (wetParticle == null)
        {
            Debug.LogError("[WetEffectManager] ❌ ParticleSystem tidak ditemukan pada: " + gameObject.name);
        }
    }

    // 🔵 Nyalakan efek air (basah)
    public void PlayEffect()
    {
        if (wetParticle == null) return;

        if (!wetParticle.isPlaying)
        {
            wetParticle.Play();
            Debug.Log("[WetEffectManager] ✅ Efek basah DINYALAKAN.");
        }
    }

    // 🔴 Matikan efek air
    public void StopEffect()
    {
        if (wetParticle == null) return;

        if (wetParticle.isPlaying)
        {
            wetParticle.Stop();
            Debug.Log("[WetEffectManager] 🛑 Efek basah DIMATIKAN.");
        }
    }

    // 🟡 Cek apakah efek sedang aktif
    public bool IsPlaying()
    {
        return wetParticle != null && wetParticle.isPlaying;
    }
}
