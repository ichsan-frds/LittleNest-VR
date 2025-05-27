using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IndicatorStatus : MonoBehaviour
{
    public List<Image> indicatorCircles;  // Isi lewat Inspector: ke-4 lingkaran
    public Color colorBenar = Color.green;
    public Color colorSalah = Color.red;
    public Color colorNetral = Color.gray;

    // Set semua ke netral awal
    public void ResetAll()
    {
        foreach (var img in indicatorCircles)
        {
            img.color = colorNetral;
        }
    }

    // Panggil fungsi ini saat task ke-i selesai
    public void SetTaskStatus(int index, bool isSuccess)
    {
        if (index < 0 || index >= indicatorCircles.Count) return;

        indicatorCircles[index].color = isSuccess ? colorBenar : colorSalah;
    }
}
