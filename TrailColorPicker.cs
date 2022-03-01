using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailColorPicker : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    public GameObject allExceptCP, colorP;
    Color color, imageColor;
    public Image colorPickerButton;


    private void OnDestroy()
    {
        PlayerPrefs.SetString("TrailColor", ColorUtility.ToHtmlStringRGB(fcp.color));
    }
    private void Start()
    {
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("TrailColor"), out color);
        fcp.startingColor = color;
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("TrailColor"), out imageColor);
        colorPickerButton.color = imageColor;
    }
    public void CP()
    {
        allExceptCP.SetActive(!allExceptCP.active);
        colorP.SetActive(!colorP.active);
        if (colorP.active)
        {
            ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("TrailColor"), out color);
            fcp.startingColor = color;
        }
        else
        {
            PlayerPrefs.SetString("TrailColor", ColorUtility.ToHtmlStringRGB(fcp.color));
            colorPickerButton.color = fcp.color;
        }
    }
}
