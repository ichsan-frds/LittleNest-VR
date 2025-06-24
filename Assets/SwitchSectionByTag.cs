using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LoadSceneOnGrab : MonoBehaviour
{
    private XRGrabInteractable grab;

    private void OnEnable()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(OnGrabbed);
        }
    }

    private void OnDisable()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(OnGrabbed);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log("Grabbed! Trying to load scene...");

        if (grab.interactionManager != null && args.interactorObject != null)
        {
            grab.interactionManager.SelectExit(args.interactorObject, grab);
        }

        SceneManager.LoadScene("Tutorial");
    }

}
