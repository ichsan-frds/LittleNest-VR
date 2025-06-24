using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("Canvas Stage 2 Saja")]
    public GameObject canvasStage2;

    public enum GameStage { Stage2 }
    [Header("Stage Settings")]
    public GameStage currentActiveStage;

    void Start()
    {
        ActivateStage(currentActiveStage);
    }

    public void ActivateStage(GameStage stageToActivate)
    {
        currentActiveStage = stageToActivate;

        if (canvasStage2 == null)
        {
            Debug.LogError("SceneController: canvasStage2 belum di-assign!");
            return;
        }

        if (currentActiveStage == GameStage.Stage2)
        {
            canvasStage2.SetActive(true);
            Debug.Log("SceneController: Stage 2 (Canvas) Activated");
        }
    }
}
