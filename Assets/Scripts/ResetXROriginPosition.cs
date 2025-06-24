using UnityEngine;

public class ResetXROriginPosition : MonoBehaviour
{
    public Transform xrOrigin;
    public Vector3 newPosition = new Vector3(-4.06f, 0.172f, 3.37f);
    public Vector3 newRotationEuler = Vector3.zero;

    void Start()
    {
        if (xrOrigin != null)
        {
            xrOrigin.position = newPosition;
            xrOrigin.rotation = Quaternion.Euler(newRotationEuler);
            Debug.Log("🔄 XR Origin diposisikan ulang ke posisi default.");
        }
        else
        {
            Debug.LogWarning("⚠️ XR Origin belum di-assign.");
        }
    }
}
