using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PBScreen : MonoBehaviour
{
    public GameObject PBtext;
    public Text time;
    void Start()
    {
        time.text = Timer.time.ToString() + " Seconds";
        if (Player.pbHit)
        {
            PBtext.SetActive(true);
        }
    }
    void Update()
    {
        if(Time.timeSinceLevelLoad > 4)
        {
            Player.pbHit = false;
            SceneManager.LoadScene("Main Menu");
        }
    }
    public void Skip()
    {
        Player.pbHit = false;
        SceneManager.LoadScene("Main Menu");
    }
    public void Retry()
    {
        Player.pbHit = false;
        SceneManager.LoadScene(PlayerPrefs.GetInt("playedlvl",0));
    }
}
