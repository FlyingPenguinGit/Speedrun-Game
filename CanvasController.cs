using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject speedText;
    public Toggle speedToggle;
    public LevelEditorMovement levelEditorMovement;
    public Player playerScript;
    bool gettingOverview;

    public void Pause()
    {
        Time.timeScale = 0;
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
        SceneManager.LoadScene("Main Menu");
    }
    public void Restart()
    {
        if (SceneManager.GetActiveScene().buildIndex % 6 != 1)
        {
            PlayerPrefs.SetFloat("tempWorldTime" + Controller.worldNR, PlayerPrefs.GetFloat("tempWorldTime" + Controller.worldNR, 0) + Timer.time);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}
