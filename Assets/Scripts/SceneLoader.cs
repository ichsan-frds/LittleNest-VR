using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    // Fungsi ini dipanggil dari tombol
    public void LoadSceneByName()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("⚠️ Scene name belum diisi di Inspector!");
            return;
        }

        if (IsSceneInBuildSettings(sceneName))
        {
            Debug.Log("🔄 Memulai async load scene: " + sceneName);
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            Debug.LogError("❌ Scene '" + sceneName + "' tidak ada di Build Settings!");
        }
    }

    // Coroutine untuk load scene tanpa freeze
    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Opsional: tampilkan loading UI di sini

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            Debug.Log("📦 Progress loading: " + (asyncLoad.progress * 100f) + "%");

            // Progress akan mentok di 0.9 sebelum scene benar-benar siap
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log("✅ Scene siap, aktivasi...");
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    // Cek apakah scene ada di Build Settings
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
