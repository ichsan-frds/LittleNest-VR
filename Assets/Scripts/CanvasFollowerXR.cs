using UnityEngine;

public class CanvasFollowerXR : MonoBehaviour
{
    public Transform cameraTransform;
    public float forwardOffset = 1.2f;
    public float heightOffset = 0.3f;
    public float horizontalOffset = 0f;

    void Update()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null) return;
        }

        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 targetPosition = cameraTransform.position +
                                 forward * forwardOffset +
                                 Vector3.up * heightOffset +
                                 cameraTransform.right * horizontalOffset;

        transform.position = targetPosition;
        transform.rotation = Quaternion.LookRotation(forward);
    }
}
