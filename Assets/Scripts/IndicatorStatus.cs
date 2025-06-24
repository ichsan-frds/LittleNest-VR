using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IndicatorStatus : MonoBehaviour
{
    public List<Image> indicatorCircles;
    public Color colorBenar = Color.green;
    public Color colorSalah = Color.red;
    public Color colorNetral = Color.gray;

    private bool[] statusLocked;

    void Start()
    {
        statusLocked = new bool[indicatorCircles.Count];
        ResetAll();
    }

    // Set semua ke netral
    public void ResetAll()
    {
        for (int i = 0; i < indicatorCircles.Count; i++)
        {
            indicatorCircles[i].color = colorNetral;
        }

        statusLocked = new bool[indicatorCircles.Count];
    }

    // Tandai task berhasil/gagal satu kali
    public void SetTaskStatus(int index, bool isSuccess)
    {
        if (index < 0 || index >= indicatorCircles.Count) return;

        if (statusLocked[index]) return; // ❗ Cegah perubahan ulang

        indicatorCircles[index].color = isSuccess ? colorBenar : colorSalah;
        statusLocked[index] = true;
    }
}
