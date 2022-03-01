using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerLobby : MonoBehaviour
{
    string versionName = "0.1";
    [SerializeField] private InputField gameName;
    [SerializeField] Dropdown dropdown;
    int lvl = 1;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
        PhotonNetwork.player.NickName = PlayerPrefs.GetString("playername", "Guest#" + Random.Range(0, 1000).ToString());
    }
    private void OnConnectedToServer()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }
    public void Join()
    {
        if (PhotonNetwork.connectedAndReady)
        {
            lvl = dropdown.value + 1;
            PhotonNetwork.JoinOrCreateRoom(lvl.ToString() + gameName.text, new RoomOptions() { maxPlayers = 15 }, TypedLobby.Default);
        }
    }
    void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Multiplayer Level" + lvl.ToString());
    }
    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
