using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    public static float time;
    private void Start()
    {
        time = 0;
    }

    void Update()
    {
        time = Time.timeSinceLevelLoad;
        time = Mathf.Round(time * 100) / 100;
        timer.text = time.ToString();
    }
}
