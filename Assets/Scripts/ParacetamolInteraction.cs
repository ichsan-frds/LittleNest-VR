using UnityEngine;

public class ParacetamolInteraction : MonoBehaviour
{
    [Header("Referensi")]
    public EmoteController emoteController; // Assign EmoteController dari scene Anda

    [Header("Pengaturan Interaksi")]
    public string babyTag = "Baby"; // Tag untuk GameObject bayi
    public float requiredContactDuration = 2.0f; // Durasi kontak yang dibutuhkan (dalam detik)

    private float currentContactTime = 0f;
    private bool isTouchingBaby = false;
    private bool actionHasBeenTriggered = false; // Untuk memastikan emote hanya muncul sekali per interaksi panjang

    // Jika interaksi ini spesifik untuk stage tertentu (misal Stage 2)
    // Anda bisa menambahkan referensi ke SceneController di sini jika ingin mengecek stage secara internal
    // public SceneController sceneController; 

    void Start()
    {
        if (emoteController == null)
        {
            Debug.LogError("❌ ParacetamolInteraction: EmoteController belum di-assign di Inspector!");
        }
        // Jika menggunakan pengecekan stage internal:
        // if (sceneController == null)
        // {
        //    Debug.LogWarning("ParacetamolInteraction: SceneController belum di-assign. Pengecekan stage mungkin tidak berfungsi.");
        // }
    }

    void Update()
    {
        // Jika interaksi ini spesifik untuk Stage 2 dan Anda ingin skrip ini mengeceknya:
        // if (sceneController != null && sceneController.currentActiveStage != SceneController.GameStage.Stage2)
        // {
        //     // Jika bukan Stage 2, jangan lakukan apa-apa atau pastikan state interaksi direset
        //     if (isTouchingBaby) // Jika sedang menyentuh bayi tapi stage salah, reset
        //     {
        //         isTouchingBaby = false;
        //         currentContactTime = 0f;
        //     }
        //     return; 
        // }

        if (isTouchingBaby && !actionHasBeenTriggered)
        {
            currentContactTime += Time.deltaTime;

            if (currentContactTime >= requiredContactDuration)
            {
                if (emoteController != null)
                {
                    Debug.Log("Paracetamol: Kontak cukup lama, bayi diberi paracetamol, menampilkan emote happy.");
                    emoteController.ShowHappy(); // Panggil emote happy
                    actionHasBeenTriggered = true; // Tandai bahwa aksi sudah dilakukan untuk interaksi ini
                }
                else
                {
                    Debug.LogError("❌ ParacetamolInteraction: EmoteController null, tidak bisa menampilkan emote.");
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            // Opsi: Cek stage di sini juga jika perlu, sebelum memulai timer
            // if (sceneController != null && sceneController.currentActiveStage != SceneController.GameStage.Stage2)
            // {
            //     return; // Jangan mulai interaksi jika bukan stage yang tepat
            // }

            Debug.Log("Paracetamol: Mulai bersentuhan dengan bayi.");
            isTouchingBaby = true;
            currentContactTime = 0f;
            actionHasBeenTriggered = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(babyTag))
        {
            Debug.Log("Paracetamol: Berhenti bersentuhan dengan bayi.");
            isTouchingBaby = false;
            currentContactTime = 0f;
            // Jika Anda ingin emote happy hilang saat paracetamol dilepas:
            // if (emoteController != null && actionHasBeenTriggered) {
            //     emoteController.HideAllDisplays();
            // }
            // actionHasBeenTriggered = false; // Tidak perlu direset di sini, sudah di OnCollisionEnter
        }
    }
}