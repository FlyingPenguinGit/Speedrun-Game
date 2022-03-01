using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LootLocker.Requests;
using UnityEngine.Events;
using System.IO;


public class Controller : MonoBehaviour
{
    public Text playtimeText;
    public Text endlessPBtext;
    public GameObject endlessPB, endlessButton;

    //leaderboards
    public InputField memberID;
    string playerName;
    int ID = 916;
    public GameObject placeholder, nameObject;
    public Text nameText;

    //stars
    public float[] starTimes;//[lvlnr - 1][sternanzahl - 1]
    public GameObject[] starObject;
    [SerializeField] GameObject starObj;
    int starCount;

    //challenge
    public static bool challengePlaying;
    System.DateTime current = System.DateTime.Now;
    public Text timeLeft;

    //worlds
    public GameObject[] worldlvlselect;
    public static bool inPBScreen;
    public static int worldNR = 1;
    public GameObject PBObject;
    public PBs PBs;
    public static bool inWorldSpeedrun;

    //special
    public GameObject container;
    [SerializeField] GameObject[] nonSpecial;
    bool inSpecialLevels = false;
    public Dropdown dropdown;
    List<string> files = new List<string>();
    string[] paths;
    public static string choosenLevel;

    //abilities


    private void Start()
    {
        Time.timeScale = 1;
        if(PlayerPrefs.GetInt("playedBefore", 0) == 0)
        {
            PlayerPrefs.SetInt("playedBefore", 1);
            SceneManager.LoadScene("Tutorial Level");
        }
        dropdown.ClearOptions();
        paths = System.IO.Directory.GetFiles(Application.persistentDataPath);
        foreach (string file in paths)
        {
            files.Add(Path.GetFileName(file));
        }
        dropdown.AddOptions(files);
        playtimeText.text = ((int)(PlayerPrefs.GetFloat("playtime", 0) + Time.time) / 3600).ToString() + ":" + ((int)(PlayerPrefs.GetFloat("playtime", 0) + Time.time) % 3600 / 60).ToString() + " hours";
        for (int i = 0; i < 30; i++)
        {
            for (int j = 2; j >= 0; j--)
            {
                if(PlayerPrefs.GetFloat("PBTime" + (i + 1), 0) != 0 && PlayerPrefs.GetFloat("PBTime" + (i + 1), 0) <= starTimes[i * 3 + j])
                {
                    starCount += j + 1;
                    j = -1;
                }
            }
        }
        int beforeStars = PlayerPrefs.GetInt("stars", 0);
        if(beforeStars < starCount)
        {
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + (starCount - PlayerPrefs.GetInt("stars", 0)) * 7);
            PlayerPrefs.SetInt("stars", starCount);
        }
        inPBScreen = false;
        if(worldNR == 0)
        {
            worldNR = 1;
        }
        for (int i = (worldNR - 1) * 6; i < worldNR * 6; i++)
        {
            for (int j = 2; j >= 0; j--)
            {
                float relevantTime = PlayerPrefs.GetFloat("PBTime" + (i + 1), 0);
                if (relevantTime != 0 && relevantTime <= starTimes[i * 3 + j])
                {
                    starObject[(i - (worldNR - 1) * 6) * 3 + j].SetActive(true);
                    j = -1;
                }
            }
        }

        playerName = PlayerPrefs.GetString("playername", "null");
        if(playerName == "null")
        {
            placeholder.SetActive(true);
            nameObject.SetActive(false);
        }
        else
        {
            nameText.text = playerName;
            placeholder.SetActive(false);
            nameObject.SetActive(true);
        }
        inWorldSpeedrun = false;
        OptionsMenu.sensi = PlayerPrefs.GetFloat("sensi", 200f);

        /*
        if (PlayerPrefs.GetInt("dailylvlnr", 0) != (int)dailyLevel)
        {
            LootLockerSDKManager.StartSession("player", (response) =>
            {
            });
            LootLockerSDKManager.GetScoreList(1033 + PlayerPrefs.GetInt("dailylvlnr", 0) % 3, 6, (response) =>
            {
                LootLockerLeaderboardMember[] scores = response.items;
                int playerRank = 0;
                if (scores != null)
                {
                    for (int i = 0; i < scores.Length; i++)
                    {
                        if (playerName == scores[i].member_id)
                        {
                            playerRank = scores[i].rank;
                        }
                    }
                    if (playerRank != 0)
                    {
                        int coinsToAdd = 0;
                        for (int i = 6; i > 2; i--)
                        {
                            if (i >= playerRank)
                            {
                                coinsToAdd += 25;
                            }
                        }
                        if (playerRank == 1)
                        {
                            coinsToAdd = 200;
                        }
                        else if (playerRank == 2)
                        {
                            coinsToAdd = 150;
                        }
                        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + coinsToAdd);
                    }
                    PlayerPrefs.SetInt("dailylvlnr", dailyLevel);
                }
            });
            PlayerPrefs.SetFloat("challegeTime", 0);
        }
        */
        System.DateTime tomorrow = current.AddDays(1).Date;
        timeLeft.text = (Mathf.Round((float)(tomorrow - current).TotalHours * 10) / 10).ToString() + " hours left";
    }
    public void Submit()
    {
        LootLockerSDKManager.StartSession("player", (response) =>
        {

        });
        if (playerName == "null")
        {
            playerName = memberID.text;
            PlayerPrefs.SetString("playername", playerName);
            nameText.text = playerName;
        }
        if (playerName == "secretcode")
        {
            PlayerPrefs.SetInt("adFree", 1);
        }
        else if (playerName != "null")
        {
            /*
            if (PlayerPrefs.GetFloat("challegeTime") != 0)
            {
                LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("challegeTime") * 100), 1033 + Controller.dailyLevel % 3, (response) =>
                {

                });
            }
            */
            for (int i = 1; i <= 6; i++)
            {
                if (PlayerPrefs.GetFloat("PBTime" + i) != 0)
                {
                    LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("PBTime" + i) * 100), ID + i, (response) =>
                    {

                    });
                }

            }
            for(int i = 7; i <= 12; i++)
            {
                if (PlayerPrefs.GetFloat("PBTime" + i) != 0)
                {
                    LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("PBTime" + i) * 100), 955 + i, (response) =>
                    {

                    });
                }
            }
            for (int i = 13; i <= 18; i++)
            {
                if (PlayerPrefs.GetFloat("PBTime" + i) != 0)
                {
                    LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("PBTime" + i) * 100), 970 + i, (response) =>
                    {

                    });
                }
            }
            for (int i = 19; i <= 24; i++)
            {
                if (PlayerPrefs.GetFloat("PBTime" + i) != 0)
                {
                    LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("PBTime" + i) * 100), 1002 + i, (response) =>
                    {

                    });
                }
            }
            for (int i = 25; i <= 30; i++)
            {
                if (PlayerPrefs.GetFloat("PBTime" + i) != 0)
                {
                    LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("PBTime" + i) * 100), 1157 + i, (response) =>
                    {

                    });
                }
            }
            for (int i = 1; i <= 5; i++)
            {
                if (PlayerPrefs.GetFloat("WorldTime" + i) != 0)
                {
                    if (i != 5)
                    {
                        LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("WorldTime" + i) * 100), 1026 + i, (response) =>
                        {

                        });
                    }
                    else
                    {
                        LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("WorldTime" + i) * 100), 1188, (response) =>
                        {

                        });
                    }
                }
            }
            if (PlayerPrefs.GetFloat("Endless", 0) != 0)
            {
                LootLockerSDKManager.SubmitScore(playerName, (int)(PlayerPrefs.GetFloat("Endless", 0) * 100), 996, (response) =>
                {

                });
            }
            placeholder.SetActive(false);
            nameObject.SetActive(true);
        }        
    }    
    public void LevelLoad(int levelNR)
    {
        SceneManager.LoadScene(levelNR);
    }
    public void PersonalBest()
    {
        if (!inSpecialLevels)
        {
            worldlvlselect[worldNR - 1].SetActive(!worldlvlselect[worldNR - 1].active);
            if (!inPBScreen)
            {
                PBs.PB();
                inPBScreen = true;
                endlessPB.SetActive(true);
            }
            else
            {
                inPBScreen = false;
                endlessPB.SetActive(false);
                endlessButton.SetActive(true);
            }
            PBObject.SetActive(!PBObject.active);
        }
        else
        {
            if (!inPBScreen)
            {
                endlessButton.SetActive(false);
                endlessPBtext.text = PlayerPrefs.GetFloat("Endless", 0).ToString() + " Seconds";
                inPBScreen = true;
                endlessPB.SetActive(true);
            }
            else
            {
                inPBScreen = false;
                endlessPB.SetActive(false);
                endlessButton.SetActive(true);
            }
        }
    }
    public void Leaderboards()
    {
        SceneManager.LoadScene("Leaderboards");
    }
    public void Options()
    {
        SceneManager.LoadScene("Options Menu");
    }
    /*
    public void SwitchWorld()
    {
        if(destroyStars != null)
        {
            destroyStars.Invoke();
        }
        worldNR++;
        if(worldNR == 6)
        {
            worldNR = 1;
        }

        //worldSpeedrundText.text = "World " + worldNR + " Speedrun";
        for (int i = (worldNR - 1) * 6; i < worldNR * 6; i++)
        {
            for (int j = 2; j >= 0; j--)
            {
                float relevantTime = PlayerPrefs.GetFloat("PBTime" + (i + 1), 0);
                if (relevantTime != 0 && relevantTime <= starTimes[i * 3 + j])
                {
                    starObject[(i - (worldNR - 1) * 6) * 3 + j].SetActive(true);
                    j = -1;
                }
            }
        }
        if (!inPBScreen)
        {
            for (int i = 0; i < worldlvlselect.Length; i++)
            {
                worldlvlselect[i].SetActive(true);
            }
        }
        else
        {
            PBs.PB();
        }
    }
    */ //SwitchWorld()
    public void StartWorldSpeedrun()
    {
        inWorldSpeedrun = true;
        PlayerPrefs.SetFloat("tempWorldTime" + worldNR, 0);
        SceneManager.LoadScene((worldNR - 1) * 6 + 1);
    }
    public void DailyChallenge()
    {
        if (!challengePlaying)
        {
            challengePlaying = true;

            PlayerPrefs.SetInt("lastDay", System.DateTime.Now.DayOfYear);
            AdsManager.adFree = true;
            SceneManager.LoadScene(PlayerPrefs.GetInt("dailylevel", 1));
        }
    }
    public void Endless()
    {
        SceneManager.LoadScene("Endless Level");
    }
    public void LevelEditor()
    {
        SceneManager.LoadScene("Level Editor");
    }
    public void SpecialLevel()
    {
        if (!inSpecialLevels)
        {
            starObj.SetActive(false);
            PBObject.SetActive(false);

            endlessPB.SetActive(inPBScreen);
            endlessButton.SetActive(!inPBScreen);
            for (int i = 0; i < worldlvlselect.Length; i++)
            {
                worldlvlselect[i].SetActive(false);
            }
        }
        else
        {
            starObj.SetActive(true);
            for (int i = 0; i < worldlvlselect.Length; i++)
            {
                worldlvlselect[i].SetActive(true);
            }
        }
        foreach(var obj in nonSpecial)
        {
            obj.SetActive(!obj.activeInHierarchy);
        }
        inSpecialLevels = !inSpecialLevels;
        container.SetActive(!container.active);
    }
    public void LoadCustom()
    {
        choosenLevel = paths[dropdown.value];
        SceneManager.LoadScene("Level Player");
    }
    public void Multiplayer()
    {
        SceneManager.LoadScene("Lobby");
    }
}
