using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCont : MonoBehaviour
{
    public void StartBtn()
    {
        SceneManager.LoadScene("Tutorial"); // Ganti dengan nama scene utama kamu
    }

    public void ExitBtn()
    {
        Application.Quit();
        Debug.Log("Keluar dari game"); // Hanya muncul saat dijalankan di editor
    }
}