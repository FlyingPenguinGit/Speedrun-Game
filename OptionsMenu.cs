using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public Slider slider;
    public Toggle toggle;
    public Toggle PBToggle;

    public static float sensi;
    public bool postP;

    public GameObject[] trailRewards;
    public GameObject customTrailColor;
    public Image[] colors;
    public Dropdown dropdown;

    public GameObject really;
    public Toggle customColorToggle;

    private void Start()
    {
        sensi = PlayerPrefs.GetFloat("sensi", 200f);
        dropdown.value = PlayerPrefs.GetInt("quality", 2);
        slider.value = 475 - sensi;

        if (PlayerPrefs.GetInt("postP", 0) == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        if (PlayerPrefs.GetInt("showpb", 0) == 1)
        {
            PBToggle.isOn = true;
        }
        else
        {
            PBToggle.isOn = false;
        }
        if (PlayerPrefs.GetInt("customColor", 0) == 1)
        {
            customColorToggle.isOn = true;
        }
        else
        {
            customColorToggle.isOn = false;
        }
        int rewardCount = 0;
        int possibleRewards = 5;
        for (int i = 1; i <= possibleRewards; i++)
        {
            if(PlayerPrefs.GetInt("WRworld" + i) == 1)
            {
                trailRewards[i - 1].SetActive(true);
                rewardCount++;
            }
        }
        if(rewardCount == possibleRewards)
        {
            customTrailColor.SetActive(true);
        }
        if (PlayerPrefs.GetInt("WRendless") == 1)
        {
            trailRewards[5].SetActive(true);
        }
    }
    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void SensiChange(float newSens)
    {
        sensi = 475 - newSens;
        PlayerPrefs.SetFloat("sensi", sensi);
    }
    public void PostProcess(bool check)
    {
        if (check)
        {
            postP = true;
            PlayerPrefs.SetInt("postP", 1);
        }
        else
        {
            postP = false;
            PlayerPrefs.SetInt("postP", 0);
        }
    }
    public void Rewardchange(int i)
    {
        PlayerPrefs.SetInt("SelectedReward", i);
    }
    public void SetQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("quality", qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void Check()
    {
        really.SetActive(true);
    }
    public void Cancel()
    {
        really.SetActive(false);
    }
    public void ResetName()
    {
        PlayerPrefs.SetString("playername", "null");
        really.SetActive(false);
    }
    public void OpenShop()
     {
        SceneManager.LoadScene("Item Shop");
     }
    public void ShowPBScreen(bool pb)
    {
        if (pb)
        {
            PlayerPrefs.SetInt("showpb", 1);
        }
        else
        {
            PlayerPrefs.SetInt("showpb", 0);
        }
        
    }
    public void OnDestroy()
    {
        if (customColorToggle.isOn)
        {
            PlayerPrefs.SetInt("customColor", 1);
        }
        else
        {
            PlayerPrefs.SetInt("customColor", 0);
        }
    }
}
