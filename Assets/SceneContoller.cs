using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("Stage Canvases")]
    public GameObject canvasStage1;
    public GameObject canvasStage2;

    public enum GameStage { Stage1, Stage2 }
    [Header("Stage Settings")]
    public GameStage currentActiveStage;

    void Start()
    {
        ActivateStage(currentActiveStage);
    }

    public void ActivateStage(GameStage stageToActivate)
    {
        currentActiveStage = stageToActivate;

        if (canvasStage1 == null || canvasStage2 == null)
        {
            Debug.LogError("SceneController: Pastikan canvasStage1 dan canvasStage2 sudah di-assign!");
            return;
        }

        canvasStage1.SetActive(currentActiveStage == GameStage.Stage1);
        canvasStage2.SetActive(currentActiveStage == GameStage.Stage2);

        if (currentActiveStage == GameStage.Stage1)
        {
            Debug.Log("SceneController: Stage 1 (Canvas) Activated");
            // Panggil fungsi Init di TaskManager_Stage1.cs jika perlu
        }
        else if (currentActiveStage == GameStage.Stage2)
        {
            Debug.Log("SceneController: Stage 2 (Canvas) Activated");
            // Panggil fungsi Init di TaskManager_Stage2.cs jika perlu
        }
    }
}