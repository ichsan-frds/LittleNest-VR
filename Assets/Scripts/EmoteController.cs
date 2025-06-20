using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EmoteController : MonoBehaviour
{
    [Header("Sprite Emotes & Status")]
    public Sprite happySprite;
    public Sprite angrySprite;
    public Sprite feverEmoteSprite;
    public Sprite thirtyNineDegreeSprite;
    public Sprite loveSprite; // BARU: Untuk emote love

    private Image displayImage;

    void Awake()
    {
        Debug.Log($"EmoteController: Awake() dipanggil untuk GameObject: '{this.gameObject.name}', " +
                  $"activeSelf: {this.gameObject.activeSelf}, activeInHierarchy: {this.gameObject.activeInHierarchy}");

        displayImage = GetComponent<Image>();

        if (displayImage == null)
        {
            Debug.LogError($"❌ EmoteController: Komponen Image TIDAK DITEMUKAN pada GameObject '{this.gameObject.name}' saat Awake!");
        }
        else
        {
            Debug.Log($"EmoteController: Komponen Image BERHASIL DITEMUKAN pada GameObject '{this.gameObject.name}'.");
            displayImage.enabled = false;
        }

        if (happySprite == null || angrySprite == null || feverEmoteSprite == null || thirtyNineDegreeSprite == null || loveSprite == null) // Perbarui validasi
        {
            Debug.LogWarning("⚠️ EmoteController: Tidak semua sprite (happy, angry, fever, 39 degree, love) telah di-assign di Inspector.");
        }
    }

    private void ShowSprite(Sprite spriteToShow)
    {
        if (displayImage == null)
        {
            Debug.LogError($"⚠️ Tampilkan Sprite gagal: displayImage (komponen Image) masih null di GameObject '{this.gameObject.name}'. " +
                           "Pastikan Awake() berjalan dengan benar dan menemukan Image.");
            return;
        }
        if (spriteToShow == null)
        {
            Debug.LogWarning($"⚠️ Tampilkan Sprite gagal pada GameObject '{this.gameObject.name}': " +
                             "spriteToShow (gambar yang ingin ditampilkan) null. Pastikan sudah di-assign di Inspector.");
            displayImage.enabled = false;
            return;
        }

        displayImage.sprite = spriteToShow;
        displayImage.enabled = true;
        Debug.Log($"EmoteController: Menampilkan sprite '{spriteToShow.name}' pada GameObject '{this.gameObject.name}'.");
    }

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

    public void ShowLove() // BARU
    {
        ShowSprite(loveSprite);
    }

    public void HideAllDisplays()
    {
        if (displayImage != null)
        {
            displayImage.enabled = false;
        }
        else
        {
            Debug.LogWarning($"⚠️ HideAllDisplays: displayImage null di GameObject '{this.gameObject.name}'.");
        }
    }
}