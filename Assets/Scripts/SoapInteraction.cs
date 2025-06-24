using UnityEngine;

public class SoapInteraction : MonoBehaviour
{
    public GameObject foamEffect;

    // ✅ Tambahkan penanda global agar bisa dicek dari script lain (misalnya TowelInteraction)
    public static bool IsSoaped = false;

    private void Start()
    {
        IsSoaped = false; // Reset setiap awal permainan
        Debug.Log($"[SOAP] Script aktif di: {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SOAP] Trigger terdeteksi. Objek terkena: {other.gameObject.name} (Tag: {other.tag})");

        if (other.CompareTag("Baby"))
        {
            Debug.Log("[SOAP] Sabun berhasil menyentuh bayi.");

            if (foamEffect != null)
            {
                foamEffect.SetActive(true);
                IsSoaped = true; // ✅ Tandai bahwa sabun sudah digunakan
                Debug.Log("[SOAP] Efek busa diaktifkan & status IsSoaped = true.");
            }
            else
            {
                Debug.LogWarning("[SOAP] foamEffect belum di-assign di Inspector!");
            }
        }
        else
        {
            Debug.Log("[SOAP] Objek yang disentuh BUKAN bayi. Tidak ada aksi.");
        }
    }

    // Optional: fungsi untuk reset jika mau digunakan ulang
    public static void ResetSoapStatus()
    {
        IsSoaped = false;
        Debug.Log("[SOAP] Status sabun di-reset.");
    }
}
