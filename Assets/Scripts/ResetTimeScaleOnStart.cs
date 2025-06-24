using UnityEngine;

public class ResetTimeScaleOnStart : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log($"[ResetTimeScaleOnStart] Awake: Time.timeScale = {Time.timeScale}");
    }

    void Start()
    {
        Debug.Log($"[ResetTimeScaleOnStart] Start: Time.timeScale = {Time.timeScale}");

        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            Debug.Log("🔄 Time.timeScale = 0 terdeteksi, diubah ke 1.");
        }
        else
        {
            Debug.Log("✅ Time.timeScale sudah 1, tidak perlu diubah.");
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        // Tombol debug di Editor untuk resume manual
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            Debug.Log("⏩ [Debug] Tombol R ditekan. Time.timeScale diubah ke 1 secara manual.");
        }
    }
#endif
}
