using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{

    public TextMeshProUGUI bottomText;
    public GameObject plane;
    private Transform planeTransform;
    public GameObject planeFinder;
    public GameObject canvas;

    // Durasi waktu dalam detik untuk memanjang
    public float duration = 1.0f;

    // Skala akhir pada sumbu Z
    public float targetScaleZ = 0.1f;

    // Awal skala pada sumbu Z
    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        planeTransform = plane.transform;
        // Simpan skala awal
        initialScale = planeTransform.localScale;

        // Mulai coroutine untuk memanjang plane
        StartCoroutine(StretchOverTime(duration));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Fungsi untuk memulai perpanjangan plane
    public void StretchPlane()
    {
        if (planeTransform != null)
        {
            StartCoroutine(StretchOverTime(duration));
        }
    }

    IEnumerator StretchOverTime(float time)
    {

        planeTransform = plane.transform;
        // Simpan skala awal
        initialScale = new Vector3(planeTransform.localScale.x, planeTransform.localScale.y, (float) 0.01);
        
        float elapsedTime = 0;
        Vector3 targetScale = new Vector3(initialScale.x, initialScale.y, targetScaleZ);

        while (elapsedTime < time)
        {
            // Interpolasi skala dari awal ke target
            planeTransform.localScale = Vector3.Lerp(initialScale, targetScale, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            // Tunggu frame berikutnya
            yield return null;
        }

        // Pastikan skala akhir tepat pada target
        planeTransform.localScale = targetScale;
    }

    public void ChangeToFindPlane()
    {
        bottomText.text = "Cari sebuah bidang datang yang diterangi.";
    }

    public void ChangeToTap()
    {
        bottomText.text = "Tekan dimanapun untuk menetapkan Anchor.";
    }

    public void TurnOnCanvas()
    {
        // groundPlaneStage.SetActive(true);
        planeFinder.SetActive(true);
        canvas.SetActive(true);
    }

    public void TurnOffCanvas()
    {
        // groundPlaneStage.SetActive(false);
        planeFinder.SetActive(false);
        canvas.SetActive(false);
    }

}
