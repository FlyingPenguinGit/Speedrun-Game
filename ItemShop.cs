using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemShop : MonoBehaviour
{
    public Text coinCount;
    public GameObject[] priceTags;
    [SerializeField] GameObject adFreePriceTag;
    int[] prices = {500, 1000, 1500, 2000, 2500, 500, 1000, 2000, 2500};

    void Start()
    {
        coinCount.text = PlayerPrefs.GetInt("coins", 0).ToString();

        if (PlayerPrefs.GetInt("adFree", 0) == 1)
        {
            adFreePriceTag.SetActive(false);
        }

        for (int i = 1; i <= priceTags.Length; i++)
        {
            if(PlayerPrefs.GetInt("item" + i, 0) != 0)
            {
                priceTags[i - 1].SetActive(false);
            }
        }
        if(PlayerPrefs.GetInt("adFree", 0) == 1)
        {
            adFreePriceTag.SetActive(false);
        }
    }
    public void Back()
    {
        SceneManager.LoadScene("Options Menu");
    }
    public void Buy(int NR)
    {
        if(PlayerPrefs.GetInt("item" + NR, 0) == 0)
        {
            if(PlayerPrefs.GetInt("coins", 0) >= prices[NR - 1])
            {
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) - prices[NR - 1]);
                PlayerPrefs.SetInt("item" + NR, 1);
                priceTags[NR - 1].SetActive(false);
                coinCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
            }
        }
        else
        {
            if(NR <= 5)
            {
                PlayerPrefs.SetInt("skin", NR);
            }
            else
            {
                PlayerPrefs.SetInt("hat", NR - 5);
            }
        }
    }
    public void Deselect(bool skin)
    {
        if (!skin)
        {
            PlayerPrefs.SetInt("hat", 0);
        }
        else
        {
            PlayerPrefs.SetInt("skin", 0);
        }
    }
    public void AdFree()
    {
        if (PlayerPrefs.GetInt("coins", 0) >= 7500)
        {
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) - 7500);
            PlayerPrefs.SetInt("adFree", 1);
            adFreePriceTag.SetActive(false);
            coinCount.text = PlayerPrefs.GetInt("coins", 0).ToString();
        }
    }
}
