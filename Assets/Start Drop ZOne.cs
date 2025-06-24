using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonDropZone : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        // Cek apakah objek yang masuk adalah tombol start
        if (other.CompareTag("Target"))
        {
            // Load ke scene utama
            SceneManager.LoadScene("Tutorial");
        }
    }
}
