using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownMatchStart : MonoBehaviour
{
    float timeLeft = 3f;
    [SerializeField] Text text;
    public string winner = "";

    private void OnEnable()
    {
        timeLeft = 3f;
    }
    private void Update()
    {
        timeLeft -= Time.unscaledDeltaTime;
        if (winner != "")
        {
            text.text = "Match starting in: " + Mathf.Round(timeLeft).ToString() + "\n" + "Winner: " + winner;
        }
        else
        {
            text.text = "Match starting in: " + Mathf.Round(timeLeft).ToString();
        }
    }
}
