using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IndicatorStatus : MonoBehaviour
{
    public List<Image> indicatorCircles;  // Isi lewat Inspector: ke-4 lingkaran
    public Color colorBenar = Color.green; // Pastikan ini hijau di Inspector
    public Color colorSalah = Color.red;   // Pastikan ini merah di Inspector
    public Color colorNetral = Color.gray; // Pastikan ini abu-abu di Inspector

    public void ResetAll()
    {
        foreach (var img in indicatorCircles)
        {
            if (img != null) { // Tambahkan null check untuk keamanan
                img.color = colorNetral;
            }
        }
    }

    public void SetTaskStatus(int index, bool isSuccess)
    {
        if (index < 0 || index >= indicatorCircles.Count) {
            Debug.LogWarning($"IndicatorStatus: Index {index} di luar batas untuk indicatorCircles.");
            return;
        }
        if (indicatorCircles[index] == null) { // Tambahkan null check untuk elemen list
            Debug.LogWarning($"IndicatorStatus: indicatorCircles[{index}] adalah null.");
            return;
        }

        indicatorCircles[index].color = isSuccess ? colorBenar : colorSalah;
        Debug.Log($"IndicatorStatus: Set warna indikator {index} ke {(isSuccess ? "BENAR (Hijau)" : "SALAH (Merah)")}");
    }
}