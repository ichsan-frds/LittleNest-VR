using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EmoteController : MonoBehaviour
{
    [Header("Sprite Emote")]
    public Sprite happySprite;
    public Sprite angrySprite;

    private Image emoteImage;

    void Start()
    {
        emoteImage = GetComponent<Image>();

        if (emoteImage == null)
        {
            Debug.LogError("❌ EmoteController: Komponen Image tidak ditemukan!");
            return;
        }

        emoteImage.enabled = false; // Sembunyikan saat awal
    }

    void Update()
    {
        // Tes manual: 1 = hide, 2 = angry, 3 = happy
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HideEmote();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowAngry();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ShowHappy();
        }
    }

    public void ShowHappy()
    {
        if (emoteImage == null || happySprite == null)
        {
            Debug.LogWarning("⚠️ ShowHappy gagal: komponen/sprite belum diisi.");
            return;
        }

        emoteImage.sprite = happySprite;
        emoteImage.enabled = true;
    }

    public void ShowAngry()
    {
        if (emoteImage == null || angrySprite == null)
        {
            Debug.LogWarning("⚠️ ShowAngry gagal: komponen/sprite belum diisi.");
            return;
        }

        emoteImage.sprite = angrySprite;
        emoteImage.enabled = true;
    }

    public void HideEmote()
    {
        if (emoteImage != null)
            emoteImage.enabled = false;
    }
}
