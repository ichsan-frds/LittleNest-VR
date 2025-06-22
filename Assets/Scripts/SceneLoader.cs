using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject loadingScreen; // Optional UI
    [SerializeField] private Slider loadingProgressBar; // Optional UI

    public void LoadSceneByName()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("⚠️ Scene name belum diisi di Inspector!");
            return;
        }

        if (!IsSceneInBuildSettings(sceneName))
        {
            Debug.LogError("❌ Scene '" + sceneName + "' tidak ada di Build Settings!");
            return;
        }

        Debug.Log("🔄 Memulai async load scene: " + sceneName);
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            float progress = Mathf.Clamp01(asyncOp.progress / 0.9f);
            Debug.Log("📈 Progress: " + (progress * 100f).ToString("F2") + "%");

            if (loadingProgressBar != null)
                loadingProgressBar.value = progress;

            // Scene sudah siap diaktifkan
            if (asyncOp.progress >= 0.9f)
            {
                Debug.Log("✅ Scene loaded. Activating...");
                yield return new WaitForSeconds(0.5f); // Sedikit delay biar smooth
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }

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
