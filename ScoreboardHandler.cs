using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardHandler : MonoBehaviour
{

    [SerializeField] Text scoreboard;
    [SerializeField] PhotonView photonView;

    public void UpdateScoreboard()
    {
        photonView.RPC("SetScoreboardText", PhotonTargets.All);
    }
    private void Start()
    {
        SetScoreboardText();
    }

    [PunRPC]
    void SetScoreboardText()
    {
        StringBuilder playerNames = new StringBuilder();

        foreach (var player in PhotonNetwork.playerList)
        {
            playerNames.Append(player.NickName + ":  " + player.GetScore().ToString() + "\n");
        }
        scoreboard.text = playerNames.ToString();
    }
}
