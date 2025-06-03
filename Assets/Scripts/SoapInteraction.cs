using UnityEngine;

public class SoapInteraction : MonoBehaviour
{
    public GameObject foamEffect;

    private void Start()
    {
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
                Debug.Log("[SOAP] Efek busa diaktifkan.");
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
}
