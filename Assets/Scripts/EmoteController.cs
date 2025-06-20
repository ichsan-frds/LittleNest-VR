using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // Komponen Image ini akan dipakai untuk semua 4 sprite
public class EmoteController : MonoBehaviour
{
    [Header("Sprite Emotes & Status")]
    public Sprite happySprite;
    public Sprite angrySprite;
    public Sprite feverEmoteSprite;         // Untuk emote/ekspresi bayi saat demam (misal: wajah merah)
    public Sprite thirtyNineDegreeSprite; // Untuk gambar spesifik yang menunjukkan "39 derajat" (misal: termometer dengan angka 39°C)

    private Image displayImage; // Komponen Image yang akan menampilkan semua sprite

    // GANTI Start() MENJADI Awake() untuk inisialisasi displayImage
    void Awake()
    {
        // Debug log untuk mengetahui GameObject mana yang menjalankan Awake ini
        Debug.Log($"EmoteController: Awake() dipanggil untuk GameObject: '{this.gameObject.name}', " +
                  $"activeSelf: {this.gameObject.activeSelf}, activeInHierarchy: {this.gameObject.activeInHierarchy}");

        displayImage = GetComponent<Image>();

        if (displayImage == null)
        {
            // Error ini lebih kritis jika terjadi di Awake karena berarti komponen Image benar-benar tidak ada
            Debug.LogError($"❌ EmoteController: Komponen Image TIDAK DITEMUKAN pada GameObject '{this.gameObject.name}' saat Awake!");
        }
        else
        {
            Debug.Log($"EmoteController: Komponen Image BERHASIL DITEMUKAN pada GameObject '{this.gameObject.name}'.");
            displayImage.enabled = false; // Sembunyikan gambar saat awal
        }

        // Validasi sprite bisa tetap di sini atau dipindahkan ke Start jika perlu,
        // tapi pastikan displayImage sudah diinisialisasi jika validasi ini mengaksesnya.
        // Untuk sekarang, kita bisa tinggalkan validasi sprite di sini.
        if (happySprite == null || angrySprite == null || feverEmoteSprite == null || thirtyNineDegreeSprite == null)
        {
            Debug.LogWarning("⚠️ EmoteController: Tidak semua sprite (happy, angry, fever, 39 degree) telah di-assign di Inspector.");
        }
    }

    // Anda bisa menghapus fungsi Start() jika semua logikanya sudah pindah ke Awake(),
    // atau biarkan Start() untuk logika lain jika ada. Untuk kasus ini, sepertinya semua bisa di Awake().

    // --- Fungsi internal untuk menampilkan sprite tertentu di displayImage ---
    private void ShowSprite(Sprite spriteToShow)
    {
        if (displayImage == null) // Pengecekan ini menjadi sangat penting
        {
            // Jika displayImage masih null di sini, berarti Awake() gagal atau belum dipanggil pada instance ini
            Debug.LogError($"⚠️ Tampilkan Sprite gagal: displayImage (komponen Image) masih null di GameObject '{this.gameObject.name}'. " +
                           "Pastikan Awake() berjalan dengan benar dan menemukan Image.");
            return;
        }
        if (spriteToShow == null)
        {
            Debug.LogWarning($"⚠️ Tampilkan Sprite gagal pada GameObject '{this.gameObject.name}': " +
                             "spriteToShow (gambar yang ingin ditampilkan) null. Pastikan sudah di-assign di Inspector.");
            displayImage.enabled = false; // Sembunyikan jika sprite tidak valid
            return;
        }

        displayImage.sprite = spriteToShow;
        displayImage.enabled = true;
        Debug.Log($"EmoteController: Menampilkan sprite '{spriteToShow.name}' pada GameObject '{this.gameObject.name}'.");
    }

    // --- Fungsi Publik untuk Mengontrol Tampilan ---

    public void ShowHappy()
    {
        ShowSprite(happySprite);
    }

    public void ShowAngry()
    {
        ShowSprite(angrySprite);
    }

    public void ShowFeverEmote()
    {
        ShowSprite(feverEmoteSprite);
    }

    public void Show39DegreeTemperature()
    {
        ShowSprite(thirtyNineDegreeSprite);
    }

    public void HideAllDisplays()
    {
        if (displayImage != null)
        {
            displayImage.enabled = false;
        }
        else
        {
            // Jika displayImage null saat HideAllDisplays dipanggil, ini juga indikasi masalah inisialisasi
            Debug.LogWarning($"⚠️ HideAllDisplays: displayImage null di GameObject '{this.gameObject.name}'.");
        }
    }
}