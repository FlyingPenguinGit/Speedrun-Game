using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdButton : MonoBehaviour
{
    AdsManager adsManager;
    private void Awake()
    {
        adsManager = GameObject.Find("AdsManager").GetComponent<AdsManager>();
    }
    public void PlayAd()
    {
        adsManager.PlayRewardedAd();
    }
}
