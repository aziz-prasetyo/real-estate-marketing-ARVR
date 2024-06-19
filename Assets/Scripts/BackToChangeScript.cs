using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;


public class BackToChangeScript : MonoBehaviour
{

    bool turnedOn = false;

    // Start is called before the first frame update
    void Start()
    {
        CorInitXR();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Tangani aksi tombol "back" di sini
            SceneManager.LoadScene("SampleScene");
        }

        if (XRGeneralSettings.Instance.Manager.activeLoader == null && turnedOn)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            turnedOn = true;
        }
    }

    void CorInitXR() {
        StartCoroutine(InitXR());
    }

     public IEnumerator InitXR()
    {
        yield return  XRGeneralSettings.Instance.Manager.InitializeLoader();
    }
}
