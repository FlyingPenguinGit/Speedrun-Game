using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    public static ApplicationQuit instance;
    static float saveTime = 0;

    void Awake()
    {
        saveTime = 0;
        if (GameObject.Find("TimeSaver") != null)
        {
            if(GameObject.Find("TimeSaver") == this.gameObject)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        instance = this;
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            saveTime = Time.time;
            PlayerPrefs.SetFloat("playtime", PlayerPrefs.GetFloat("playtime", 0) + Time.time);
        }
        else
        {
            PlayerPrefs.SetFloat("playtime", PlayerPrefs.GetFloat("playtime", 0) - saveTime);
        }
    }
}
