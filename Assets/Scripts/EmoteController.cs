using UnityEngine;
using UnityEngine.UI;

public class EmoteController : MonoBehaviour
{
    public Sprite happySprite;
    public Sprite angrySprite;

    private Image emoteImage;

    void Start()
    {
        emoteImage = GetComponent<Image>();
        emoteImage.enabled = false; // Default: sembunyi
    }

    void Update()
    {
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
        emoteImage.sprite = happySprite;
        emoteImage.enabled = true;
    }

    public void ShowAngry()
    {
        emoteImage.sprite = angrySprite;
        emoteImage.enabled = true;
    }

    public void HideEmote()
    {
        emoteImage.enabled = false;
    }
}
