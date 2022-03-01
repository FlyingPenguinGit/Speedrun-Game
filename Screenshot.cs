using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            print("screenshot");
            ScreenCapture.CaptureScreenshot("IconScreenshot.png",2);
        }
    }
}
