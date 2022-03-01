using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PBs : MonoBehaviour
{
    public Text[] t;
    public Color[] color;

    public void PB()
    {
        for(int i = 1; i <= 7; i++)
        {
            t[i].color = color[Controller.worldNR - 1];
        }
        t[1].text = PlayerPrefs.GetFloat("PBTime" + ((Controller.worldNR - 1) * 6 + 1), 0).ToString() + " Seconds";
        t[2].text = PlayerPrefs.GetFloat("PBTime" + ((Controller.worldNR - 1) * 6 + 2), 0).ToString() + " Seconds";
        t[3].text = PlayerPrefs.GetFloat("PBTime" + ((Controller.worldNR - 1) * 6 + 3), 0).ToString() + " Seconds";
        t[4].text = PlayerPrefs.GetFloat("PBTime" + ((Controller.worldNR - 1) * 6 + 4), 0).ToString() + " Seconds";
        t[5].text = PlayerPrefs.GetFloat("PBTime" + ((Controller.worldNR - 1) * 6 + 5), 0).ToString() + " Seconds";
        t[6].text = PlayerPrefs.GetFloat("PBTime" + ((Controller.worldNR - 1) * 6 + 6), 0).ToString() + " Seconds";
        t[7].text = PlayerPrefs.GetFloat("WorldTime" + Controller.worldNR, 0).ToString() + " Seconds";
    }
}
