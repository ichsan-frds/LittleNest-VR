using UnityEngine;

public class ThermometerInteraction : MonoBehaviour
{
    [Header("Referensi")]
    public EmoteController emoteController; // Assign EmoteController dari scene Anda ke sini

    [Header("Pengaturan Interaksi")]
    public string babyTag = "Baby"; // Tag untuk GameObject bayi
    public float requiredContactDuration = 2.0f; // Durasi kontak yang dibutuhkan (dalam detik)

    private float currentContactTime = 0f;
    private bool isTouchingBaby = false;
    private bool temperatureHasBeenShown = false; // Untuk memastikan emote hanya muncul sekali per interaksi panjang

    void Start()
    {
        if (emoteController == null)
        {
            Debug.LogError("❌ ThermometerInteraction: EmoteController belum di-assign di Inspector!");
            // Anda bisa mencoba mencarinya secara otomatis jika diperlukan, tapi assign manual lebih aman
            // emoteController = FindObjectOfType<EmoteController>(); 
        }
    }

    void Update()
    {
        if (isTouchingBaby && !temperatureHasBeenShown)
        {
            currentContactTime += Time.deltaTime;

            if (currentContactTime >= requiredContactDuration)
            {
                if (emoteController != null)
                {
                    Debug.Log("Thermometer: Kontak cukup lama, menampilkan suhu 39 derajat.");
                    emoteController.Show39DegreeTemperature();
                    temperatureHasBeenShown = true; // Tandai bahwa suhu sudah ditampilkan untuk interaksi ini
                }
                else
                {
                    Debug.LogError("❌ ThermometerInteraction: EmoteController null, tidak bisa menampilkan suhu.");
                }
            }
        }
    }

    // Fungsi ini dipanggil ketika collider ini mulai bersentuhan dengan collider lain
    void OnCollisionEnter(Collision collision)
    {
        // Cek apakah objek yang disentuh adalah bayi (berdasarkan tag)
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("Thermometer: Mulai bersentuhan dengan bayi.");
            isTouchingBaby = true;
            currentContactTime = 0f; // Reset timer setiap kali sentuhan baru dimulai
            temperatureHasBeenShown = false; // Reset status penampilan suhu
        }
    }

    // Fungsi ini dipanggil ketika collider ini berhenti bersentuhan dengan collider lain
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("Thermometer: Berhenti bersentuhan dengan bayi.");
            isTouchingBaby = false;
            currentContactTime = 0f; // Reset timer
            // Tidak perlu mereset temperatureHasBeenShown di sini, karena interaksi sudah berakhir.
            // Atau jika Anda ingin emote suhu hilang saat termometer dilepas:
            // if (emoteController != null && temperatureHasBeenShown) {
            //     emoteController.HideAllDisplays();
            // }
            // temperatureHasBeenShown = false; // Reset agar bisa muncul lagi di interaksi berikutnya
        }
    }
}