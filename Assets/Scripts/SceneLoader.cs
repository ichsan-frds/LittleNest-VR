using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    // Fungsi ini dipanggil dari tombol
    public void LoadSceneByName()
    {
        try
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                Debug.Log("🔄 Memulai load scene: " + sceneName);

                // Cek apakah scene tersedia di build settings
                if (IsSceneInBuildSettings(sceneName))
                {
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.LogError("❌ Scene '" + sceneName + "' tidak ada di Build Settings!");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Scene name belum diisi di Inspector!");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("❌ Gagal load scene: " + ex.Message + "\nStackTrace:\n" + ex.StackTrace);
        }
    }

    // Fungsi bantu untuk mengecek apakah scene ada di Build Settings
    private bool IsSceneInBuildSettings(string sceneToCheck)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(path);

            if (sceneNameInBuild == sceneToCheck)
                return true;
        }

        return false;
    }
}
