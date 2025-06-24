using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    // Dipanggil dari tombol
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

    // Coroutine untuk loading scene tanpa freeze
    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        // Loading sampai 90%
        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log("📦 Progress loading: " + (asyncLoad.progress * 100f) + "%");
            yield return null;
        }

        // Scene siap diaktifkan
        Debug.Log("✅ Scene siap, aktivasi...");
        yield return new WaitForSeconds(0.1f); // jeda opsional
        asyncLoad.allowSceneActivation = true;

        // Tunggu sampai selesai
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("🎯 Scene berhasil diaktifkan!");
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
