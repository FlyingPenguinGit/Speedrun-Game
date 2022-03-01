using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LootLocker.Requests;

public class Leaderboard : MonoBehaviour
{
    public GameObject display, backToLeaderboard;
    public Text[] entries;
    public Color[] colors;
    int leaderboardID = 916;
    int count = 6;
    public GameObject rewardButton;
    int lvlnr;
    public GameObject coinDisplay;

    public GameObject[] worldButton, worldSelect;

    private void Start()
    {
        Controller.worldNR = 1;
        LootLockerSDKManager.StartSession("player", (response) =>
        {

        });
    }

    public void Open(int levelNR)
    {
        bool daily;
        lvlnr = levelNR;
        if(lvlnr == 0)
        {
            daily = true;
            //lvlnr = Controller.dailyLevel;
        }
        else
        {
            daily = false;
        }
        if(lvlnr != 26)
        {
            for (int i = 0; i < 6; i++)
            {
                entries[i].color = colors[Controller.worldNR - 1];
            }
        }
        else if(lvlnr == 26)
        {
            for (int i = 0; i < 6; i++)
            {
                entries[i].color = colors[5];
            }
        }
        backToLeaderboard.SetActive(true);
        worldButton[0].SetActive(false);
        worldButton[1].SetActive(false);
        worldButton[2].SetActive(false);
        worldButton[3].SetActive(false);
        worldButton[4].SetActive(false);
        worldSelect[0].SetActive(false);
        worldSelect[1].SetActive(false);
        worldSelect[2].SetActive(false);
        worldSelect[3].SetActive(false);
        worldSelect[4].SetActive(false);


        display.SetActive(true);

        if (lvlnr <= 6)
        {
            leaderboardID = 916;
        }
        else if (lvlnr > 6)
        {
            leaderboardID = 955;
            if (lvlnr > 12)
            {
                leaderboardID = 970;
                if(lvlnr > 18 && lvlnr != 26)
                {
                    leaderboardID = 1002;
                    if(lvlnr > 100)
                    {
                        leaderboardID = 1182 - 125;
                    }
                }
            }
        }
        if (daily)
        {
            //leaderboardID = 1033 + Controller.dailyLevel % 3;
            lvlnr = 0;
            coinDisplay.SetActive(true);
        }
        LootLockerSDKManager.GetScoreList(leaderboardID + lvlnr, count, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] scores = response.items;

                if (scores.Length < 6)
                {
                    for (int i = 0; i < scores.Length; i++)
                    {
                        entries[i].text = (scores[i].rank + ".  " + (float)scores[i].score / 100) + "  " + scores[i].member_id;
                    }
                    for (int i = scores.Length; i < 6; i++)
                    {
                        entries[i].text = ((i + 1).ToString() + ".  n/a");
                    }
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        entries[i].text = (scores[i].rank + ".  " + (float)scores[i].score / 100) + "  " + scores[i].member_id;
                    }
                }
                if (!daily)
                {
                    if (PlayerPrefs.GetString("playername") == scores[0].member_id && lvlnr != 26)
                    {
                        rewardButton.SetActive(true);
                        PlayerPrefs.SetInt("WRworld" + Controller.worldNR, 1);
                    }
                    else if (PlayerPrefs.GetString("playername") == scores[0].member_id && lvlnr == 26)
                    {
                        rewardButton.SetActive(true);
                        PlayerPrefs.SetInt("WRendless", 1);
                    }
                }
                /*
                else
                {
                    PlayerPrefs.SetInt("WRworld" + Controller.worldNR, 0);
                }
                */
            }
        });        
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Leaderboardselect()
    {
        worldButton[Controller.worldNR % 5].SetActive(true);
        worldSelect[Controller.worldNR - 1].SetActive(true);
        display.SetActive(false);
        rewardButton.SetActive(false);
        backToLeaderboard.SetActive(false);
        coinDisplay.SetActive(false);
    }
    public void GetReward()
    {
        rewardButton.SetActive(false);
    }
    public void SwitchWorld()
    {
        Controller.worldNR++;
        if (Controller.worldNR > 5)
        {
            Controller.worldNR = 1;
        }
        for (int i = 0; i < worldButton.Length; i++)
        {
            worldButton[i].SetActive(false);
        }
        worldButton[Controller.worldNR % 5].SetActive(true);
        for (int i = 0; i < worldSelect.Length; i++)
        {
            worldSelect[i].SetActive(false);
        }
        worldSelect[Controller.worldNR - 1].SetActive(true);
    }
}
