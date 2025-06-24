using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Dipanggil dari tombol UI atau script lain
    public void Quit()
    {
        Debug.Log("Game is quitting...");

#if UNITY_EDITOR
        // Berhenti jika di dalam Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Keluar dari game jika sudah dibuild
        Application.Quit();
#endif
    }

    void Update()
    {
        // Keluar saat tombol Escape ditekan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }
}
