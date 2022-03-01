using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public static AdsManager instance;
    public static bool adFree = false;
    public GameObject rewardButton;

    private void Awake()
    {
        if (GameObject.Find("AdsManager") != null)
        {
            if (GameObject.Find("AdsManager") == this.gameObject)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        instance = this;
        if (PlayerPrefs.GetInt("adfreeDay", 0) == System.DateTime.Now.DayOfYear)
        {
            adFree = true;
        }
    }
    IEnumerator CheckForAd()
    {
        while (SceneManager.GetActiveScene().name == "Main Menu")
        {
            if (Advertisement.IsReady("Rewarded_Android") && rewardButton != null)
            {
                rewardButton.SetActive(true);
            }
            yield return new WaitForSeconds(1);
        }
    }
    private void Start()
    {        
        Advertisement.Initialize("4555807");
        Advertisement.AddListener(this);
        SceneManager.sceneLoaded += NewSceneLoaded;
        NewSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void PlayForcedAd()
    {
        if (Advertisement.IsReady("Interstitial_Android"))
        {
            print("Interstitial_Android");
            Advertisement.Show("Interstitial_Android");
        }
    }
    public void PlayRewardedAd()
    {
        if (Advertisement.IsReady("Rewarded_Android"))
        {
            Advertisement.Show("Rewarded_Android");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        print("adREADY");
    }

    public void OnUnityAdsDidError(string message)
    {
        print("adERROR" + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        print("videoStart" + placementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == "Rewarded_Android" && showResult == ShowResult.Finished)
        {
            adFree = true;
            PlayerPrefs.SetInt("adfreeDay", System.DateTime.Now.DayOfYear);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + 50);
        }
    }
    void NewSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            rewardButton = GameObject.Find("AdButton");
            rewardButton.SetActive(false);
            StartCoroutine(CheckForAd());
        }
        if (!adFree && PlayerPrefs.GetInt("adFree", 0) != 1)
        {
            float randValue = Random.value;
            if (randValue * 2 < .01f) // 1% of the time
            {
                PlayForcedAd();
            }
        }
    }
}
