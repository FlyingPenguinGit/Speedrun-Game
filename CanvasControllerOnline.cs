using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasControllerOnline : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject speedText;
    public Toggle speedToggle;
    public LevelEditorMovement levelEditorMovement;
    public PlayerOnline playerScript;
    bool gettingOverview;

    [SerializeField] GameObject startMatchButton;
    [SerializeField] PhotonView photonView;

    public void Pause()
    {
        Time.timeScale = 0;
        if (PhotonNetwork.player.IsMasterClient)
        {
            startMatchButton.SetActive(true);
        }
        pauseMenu.SetActive(true);
    }
    public void Resume()
    {
        if (gettingOverview)
        {
            Overview();
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
    public void Menu()
    {
        if (gettingOverview)
        {
            Overview();
        }
        PhotonNetwork.LeaveRoom();
    }
    void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void Restart()
    {
        playerScript.Respawn();
        Resume();
    }
    void OnApplicationFocus(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }
    public void ShowSpeed(bool enabled)
    {
        speedText.SetActive(enabled);
        if (enabled)
        {
            PlayerPrefs.SetInt("speed", 1);
        }
        else
        {
            PlayerPrefs.SetInt("speed", 0);
        }
    }
    public void Overview()
    {
        gettingOverview = !gettingOverview;
        levelEditorMovement.enabled = gettingOverview;
        levelEditorMovement.overview = gettingOverview;
        playerScript.enabled = !gettingOverview;
        Camera.main.GetComponent<CameraFollow>().enabled = !gettingOverview;

        if (!gettingOverview)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
        pauseMenu.SetActive(false);
    }
    public void SetCamZoom(float number)
    {
        Camera.main.orthographicSize = number;
        PlayerPrefs.SetFloat("camzoom", number);
    }
    public void StartMatch()
    {
        photonView.RPC("PlayerFinished", PhotonTargets.All, "");
        Resume();
    }
}
